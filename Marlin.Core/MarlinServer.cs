using Marlin.Core.Attributes;
using Marlin.Core.Common;
using Marlin.Core.Entities;
using Marlin.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.Json;
using Marlin.Core.Encrypt;

namespace Marlin.Core
{
    internal class MarlinServer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MarlinConfiguration _configuration;
        private readonly IEventHandler _eventHandler;
        private HttpListener _listener;

        public MarlinServer([NotNull] IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            _configuration = (MarlinConfiguration)_serviceProvider.GetService(typeof(MarlinConfiguration));

            if (_configuration?.JwtConfiguration == null)
            {
                throw new ArgumentNullException(nameof(_configuration.JwtConfiguration));
            }

            if (_configuration.EventLoggerEnabled)
            {
                _eventHandler = (IEventHandler)serviceProvider.GetService(typeof(IEventHandler));
            }
        }

        internal void Start()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"https://localhost:{_configuration.Port}/");
            _listener.Start();
            Receive();
        }

        internal void Stop()
        {
            _listener.Stop();
        }

        private void Receive()
        {
            _listener.BeginGetContext(ListenerCallback, _listener);
        }

        private void ListenerCallback(IAsyncResult result)
        {
            if (!_listener.IsListening) return;
            
            var context = _listener.EndGetContext(result);

            Next(context);

            Receive();
        }

        private void Next(HttpListenerContext context)
        {
            var contentType = ContentType.ApplicationJson;
            var statusCode = StatusCodes.Status200Ok;
            var contentEncoding = Encoding.UTF8;
            byte[] buffer;

            if (context.Request.Url != null && context.Request.Url.LocalPath.Equals("/"))
            {
                buffer = Encoding.UTF8.GetBytes("Server is up and running :)");

                context.Response.ContentType = contentType;
                context.Response.StatusCode = statusCode;
                context.Response.ContentEncoding = contentEncoding;
                context.Response.ContentLength64 = buffer.LongLength;

                context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false).GetAwaiter().GetResult();

                context.Response.Close();
            }
            else
            {
                var timeKeeper = new TimeKeeper();

                string content = null;
                string message = null;
                string requestBody = null;

                Console.WriteLine($"Request started at {DateTime.UtcNow}");

                try
                {
                    Context.Current.Reset();

                    requestBody = new StreamReader(context.Request.InputStream).ReadToEndAsync().Result;

                    var apiOutput = Process(new ApiInput(context), requestBody);

                    contentType = apiOutput.ContentType;
                    statusCode = apiOutput.StatusCode;
                    content = apiOutput.Response;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    statusCode = StatusCodes.Status500InternalServerError;
                    contentType = ContentType.ApplicationJson;
                    message = _configuration.PropagateApplicationError ? e.Message : Messages.GenericFailure;
                    content = JsonSerializer.Serialize(new { Message = message });

                    statusCode = e switch
                    {
                        ArgumentException => StatusCodes.Status400BadRequest,
                        SecurityException => StatusCodes.Status401Unauthorized,
                        UnauthorizedAccessException => StatusCodes.Status403Forbidden,
                        EntryPointNotFoundException => StatusCodes.Status404NotFound,
                        HttpListenerException exception => exception.ErrorCode,
                        _ => StatusCodes.Status500InternalServerError
                    };
                }
                finally
                {
                    Console.WriteLine($"ContentType: {contentType}");
                    Console.WriteLine($"Content: {content}");
                    Console.WriteLine($"statusCode: {statusCode}");

                    var ms = timeKeeper.Stop().TotalMilliseconds;

                    buffer = content != null ? Encoding.UTF8.GetBytes(content) : Array.Empty<byte>();

                    if (_eventHandler != null)
                    {
                        //logger service, write event
                        _eventHandler?.WriteEvent(new Event()
                        {
                            Level = string.IsNullOrEmpty(message) ? EventLevels.Info.ToString() : EventLevels.Error.ToString(),
                            Claims = Context.Current.Claims,
                            Protocol = context.Request.ProtocolVersion.ToString(),
                            Url = context.Request.Url?.AbsolutePath,
                            Method = context.Request.HttpMethod,
                            Request = context.Request.QueryString.ToString(),
                            Response = content,
                            Host = context.Request.UserHostName,
                            Client = context.Request.RemoteEndPoint?.Address.ToString(),
                            Payload = requestBody,
                            Message = message,
                            Milliseconds = ms
                        });
                    }

                    Console.WriteLine($"Request completed in {ms} ms");

                    context.Response.ContentType = contentType;
                    context.Response.StatusCode = statusCode;
                    context.Response.ContentEncoding = contentEncoding;
                    context.Response.ContentLength64 = buffer.LongLength;

                    context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false).GetAwaiter().GetResult();

                    context.Response.Close();
                }
            }
        }

        private ApiOutput Process(ApiInput input, string body)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }


            if (string.IsNullOrEmpty(input.Url))
            {
                throw new ArgumentNullException(nameof(input.Url));
            }

            if (string.IsNullOrEmpty(input.Method))
            {
                throw new ArgumentNullException(nameof(input.Method));
            }

            List<ApiHandler> apiHandlers = FindApiHandlers(input);

            if (apiHandlers == null || apiHandlers.Count == 0)
            {
                throw new EntryPointNotFoundException(string.Format(Messages.ApiNotFound, input.Url, input.Method));
            }

            if (apiHandlers.Count > 1)
            {
                throw new HttpListenerException(StatusCodes.Status409Conflict, (string.Format(Messages.ApiConflict, input.Url, input.Method)));
            }

            var apiHandler = apiHandlers.FirstOrDefault();

            if (apiHandler == null)
            {
                throw new EntryPointNotFoundException(string.Format(Messages.ApiNotFound, input.Url, input.Method));
            }

            var api = apiHandler.GetType().GetMethods().Where(t => t.GetCustomAttributes<ApiRoute>().Any(x => x.Url == input.Url && x.Method == input.Method)).Select(x => x).FirstOrDefault();

            if (api == null)
            {
                throw new EntryPointNotFoundException(string.Format(Messages.ApiNotFound, input.Url, input.Method));
            }

            ManageSecuredAttributes(input, api);

            object[] args = GetApiArguments(input, body, api);

            var apiOutput = (ApiOutput)api.Invoke(apiHandler, args);

            return apiOutput;
        }

        private static object[] GetApiArguments(ApiInput input, string body, MethodInfo api)
        {
            var parameters = api.GetParameters().Where(x => x.GetCustomAttribute<ApiParameter>() != null || x.GetCustomAttribute<ApiBody>() != null || x.GetCustomAttribute<ApiHeader>() != null).ToArray();

            var args = new object[parameters.Length];

            if (parameters.Length == 0)
            {
                return args;
            }

            if (parameters.Any(x => x.GetCustomAttributes<ApiBody>().Count() > 1))
            {
                throw new ArgumentException(Messages.ApiBodyOnlyOne);
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                var type = parameters[i].ParameterType;

                object value;

                if (parameters[i].GetCustomAttribute<ApiParameter>() != null)
                {
                    value = input.Context.Request.QueryString[parameters[i].Name]?.FirstOrDefault();

                    if (value == null && !parameters[i].HasDefaultValue)
                    {
                        throw new ArgumentException(string.Format(Messages.ParameterNotProvided, parameters[i].Name));
                    }

                    args[i] = Convert.ChangeType(value, type);
                }
                else if (parameters[i].GetCustomAttribute<ApiBody>() != null)
                {
                    value = JsonSerializer.Deserialize(body, type);

                    args[i] = value ?? throw new ArgumentException(string.Format(Messages.EntityNotProvided, parameters[i].Name));
                }
                else if (parameters[i].GetCustomAttribute<ApiHeader>() != null)
                {
                    value = input.Context.Request.Headers[parameters[i].Name]?.FirstOrDefault();

                    if (value == null && !parameters[i].HasDefaultValue)
                    {
                        throw new ArgumentException(string.Format(Messages.HeaderNotProvided, parameters[i].Name));
                    }

                    args[i] = Convert.ChangeType(value, type);
                }
            }

            return args;
        }

        private void ManageSecuredAttributes(ApiInput input, MethodInfo api)
        {
            var securedAttributes = api.GetCustomAttributes<Secured>().ToList();

            if (!securedAttributes.Any())
            {
                return;
            }

            if (Context.Current == null || !Context.IsLoaded)
            {
                this.LoadContext(input.Context);
            }

            if (Context.Current == null || !Context.IsLoaded)
            {
                throw new SecurityException(Messages.Unauthorized);
            }

            foreach (var secured in securedAttributes)
            {
                if (!Context.HasClaim(secured.Claim))
                {
                    throw new UnauthorizedAccessException(string.Format(Messages.ApiNotAuthorizedClaim, input.Url, input.Method));
                }

                if (secured.Claim.Equals("*"))
                {
                    continue;
                }

                var claimContent = Context.GetClaim<string>(secured.Claim);

                if (string.IsNullOrEmpty(claimContent) || !secured.Value.Contains(claimContent))
                {
                    throw new UnauthorizedAccessException(string.Format(Messages.ApiNotAuthorizedClaim, input.Url, input.Method));
                }
            }
        }

        private List<ApiHandler> FindApiHandlers(ApiInput input)
        {
            return AppDomain.CurrentDomain.GetAssemblies().First(x => x.GetName().Name == AppDomain.CurrentDomain.FriendlyName).GetTypes().Where(t =>
          t.IsSubclassOf(typeof(ApiHandler))
          && !t.IsAbstract && !t.IsInterface).Where(tp => tp.GetMethods().Any(n => n.GetCustomAttributes<ApiRoute>().Any(x => x.Url == input.Url && x.Method == input.Method)))
              .Select(t =>
              {
                  var firstConstructor = t.GetConstructors().FirstOrDefault();

                  var parameters = new List<object>();

                  if (firstConstructor == null)
                  {
                      throw new NotImplementedException(string.Format(Messages.NotImplementedConstructor, t.Name));
                  }

                  foreach (var param in firstConstructor.GetParameters())
                  {
                      using var serviceScope = _serviceProvider.CreateScope();
                      var provider = serviceScope.ServiceProvider;

                      var service = provider.GetService(param.ParameterType);

                      parameters.Add(service);
                  }

                  return (ApiHandler)Activator.CreateInstance(t, parameters.ToArray());
              }).ToList();
        }

        private static void SetContext(Dictionary<string, object> claims)
        {
            foreach (var claim in claims)
            {
                Context.Add(claim.Key, claim.Value);
            }
        }

        private void LoadContext(HttpListenerContext context)
        {
            var tokenString = context.Request.Headers[Messages.HeaderAuthorization];

            if (string.IsNullOrEmpty(tokenString))
            {
                throw new SecurityException(Messages.TokenNotProvided);
            }

            var jwtContent = JwtBuilder.Create()
    .WithAlgorithm<HMACSHA256>()
    .WithSecret(_configuration.JwtConfiguration.JwtSecret)
    .Decode<Dictionary<string, object>>(tokenString);

            if (jwtContent == null)
            {
                throw new SecurityException(Messages.TokenInvalidPayload);
            }

            var claims = jwtContent.Where(x =>
            x.Key != "exp"
            && x.Key != "iss"
            && x.Key != "aud"
            && x.Key != "iat"
            && x.Key != "jti").ToDictionary(i => i.Key, i => i.Value);

            if (claims == null || !claims.Any())
            {
                throw new SecurityException(Messages.TokenInvalidClaims);
            }

            var hasExpiring = long.TryParse(jwtContent["exp"].ToString(), out long expiring);

            if (!hasExpiring)
            {
                throw new SecurityException(string.Format(Messages.TokenClaimNotProvided, "exp"));
            }

            if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > expiring)
            {
                throw new SecurityException(Messages.TokenExpired);
            }

            var hasIssuer = jwtContent.TryGetValue("iss", out var issuer);

            if (!hasIssuer && (issuer == null || !issuer.ToString().Equals(_configuration.JwtConfiguration.JwtIssuer)))
            {
                throw new SecurityException(Messages.TokenInvalidIss);
            }

            var hasAudience = jwtContent.TryGetValue("aud", out var audience);

            if (!hasAudience && (audience == null || !audience.ToString().Equals(_configuration.JwtConfiguration.JwtAudience)))
            {
                throw new SecurityException(Messages.TokenInvalidAud);
            }

            SetContext(claims);
        }
    }
}
