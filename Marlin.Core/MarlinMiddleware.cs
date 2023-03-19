using JWT.Algorithms;
using JWT.Builder;
using Marlin.Core.Attributes;
using Marlin.Core.Common;
using Marlin.Core.Entities;
using Marlin.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Threading.Tasks;

namespace Marlin.Core
{
    public class MarlinMiddleware
    {
        private readonly MarlinConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventHandler _loggerHandler;

        public MarlinMiddleware(RequestDelegate next, MarlinConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));   
            }
            
            Console.WriteLine(next.Target);

            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (configuration.JwtConfiguration == null)
            {
                throw new ArgumentNullException(nameof(configuration.JwtConfiguration));
            }

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            if (_configuration.EventLoggerEnabled)
            {
                _loggerHandler = serviceProvider.GetService<IEventHandler>();
            }
        }

        public async Task Invoke(HttpContext context)
        {
            string content = null;
            string message = null;
            string requestBody = null;
            var contentType = ContentType.TextPlain;
            var statusCode = StatusCodes.Status200OK;

            var timeKeeper = new TimeKeeper();

            Console.WriteLine($"Request started at {DateTime.UtcNow}");

            try
            {
                Context.Current.Reset();

                if (string.IsNullOrEmpty(context.Request.ContentType))
                {
                    context.Request.ContentType = contentType;
                }

                if (context.Request.ContentType != contentType)
                {
                    throw new InvalidOperationException(string.Format(Messages.InvalidContentType,
                        context.Request.ContentType, contentType));
                }

                requestBody = new StreamReader(context.Request.Body).ReadToEndAsync().Result;

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
                content = JsonConvert.SerializeObject(new { Message = message });

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

                //logger service, write event
                _loggerHandler?.WriteEvent(new Event()
                {
                    Level = string.IsNullOrEmpty(message)
                        ? EventLevels.Info.ToString()
                        : EventLevels.Error.ToString(),
                    Claims = Context.Current.Claims,
                    Protocol = context.Request.Protocol,
                    Url = context.Request.Path,
                    Method = context.Request.Method,
                    Request = context.Request.QueryString.Value,
                    Response = content,
                    Host = context.Request.Host.Value,
                    Client = context.Connection.RemoteIpAddress.ToString(),
                    Payload = requestBody,
                    Message = message,
                    Milliseconds = ms
                });

                Console.WriteLine($"Request completed in {ms} ms");

                context.Response.ContentType = contentType;
                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsync(content);
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
                throw new HttpListenerException(StatusCodes.Status409Conflict,
                    (string.Format(Messages.ApiConflict, input.Url, input.Method)));
            }

            var apiHandler = apiHandlers.FirstOrDefault();

            if (apiHandler == null)
            {
                throw new EntryPointNotFoundException(string.Format(Messages.ApiNotFound, input.Url, input.Method));
            }

            var api = apiHandler.GetType().GetMethods()
                .Where(t => t.GetCustomAttributes<ApiRoute>().Any(x => x.Url == input.Url && x.Method == input.Method))
                .Select(x => x).FirstOrDefault();

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
            var parameters = api.GetParameters().Where(x =>
                x.GetCustomAttribute<ApiParameter>() != null || x.GetCustomAttribute<ApiBody>() != null ||
                x.GetCustomAttribute<ApiHeader>() != null).ToArray();

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
                    value = input.Context.Request.Query[parameters[i].Name].FirstOrDefault();

                    if (value == null && !parameters[i].HasDefaultValue)
                    {
                        throw new ArgumentException(string.Format(Messages.ParameterNotProvided, parameters[i].Name));
                    }

                    args[i] = Convert.ChangeType(value, type);
                }
                else if (parameters[i].GetCustomAttribute<ApiBody>() != null)
                {
                    value = JsonConvert.DeserializeObject(body, type);

                    args[i] = value ??
                              throw new ArgumentException(string.Format(Messages.EntityNotProvided,
                                  parameters[i].Name));
                }
                else if (parameters[i].GetCustomAttribute<ApiHeader>() != null)
                {
                    value = input.Context.Request.Headers[parameters[i].Name].FirstOrDefault();

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
                    throw new UnauthorizedAccessException(string.Format(Messages.ApiNotAuthorizedClaim, input.Url,
                        input.Method));
                }

                if (secured.Claim.Equals("*"))
                {
                    continue;
                }

                var claimContent = Context.GetClaim<string>(secured.Claim);

                if (string.IsNullOrEmpty(claimContent) || !secured.Value.Contains(claimContent))
                {
                    throw new UnauthorizedAccessException(string.Format(Messages.ApiNotAuthorizedClaim, input.Url,
                        input.Method));
                }
            }
        }

        private List<ApiHandler> FindApiHandlers(ApiInput input)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .First(x => x.GetName().Name == AppDomain.CurrentDomain.FriendlyName).GetTypes().Where(t =>
                    t.IsSubclassOf(typeof(ApiHandler))
                    && !t.IsAbstract && !t.IsInterface).Where(tp => tp.GetMethods().Any(n =>
                    n.GetCustomAttributes<ApiRoute>().Any(x => x.Url == input.Url && x.Method == input.Method)))
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

        private void SetContext(Dictionary<string, object> claims)
        {
            foreach (var claim in claims)
            {
                Context.Add(claim.Key, claim.Value);
            }
        }

        private void LoadContext(HttpContext context)
        {
            var tokenString = context.Request.Headers[Messages.HeaderAuthorization].FirstOrDefault();

            Console.WriteLine($"Token received: {tokenString}");

            if (string.IsNullOrEmpty(tokenString))
            {
                throw new SecurityException(Messages.TokenNotProvided);
            }

            var jwtContent = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_configuration.JwtConfiguration.JwtSecret)
                .MustVerifySignature()
                .WithVerifySignature(true)
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

            if (claims == null || claims.Count == 0)
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

            var hasIssuer = jwtContent.TryGetValue("iss", out object issuer);

            if (!hasIssuer && (issuer == null || !issuer.ToString().Equals(_configuration.JwtConfiguration.JwtIssuer)))
            {
                throw new SecurityException(Messages.TokenInvalidIss);
            }

            var hasAudience = jwtContent.TryGetValue("aud", out object audience);

            if (!hasAudience && (audience == null ||
                                 !audience.ToString().Equals(_configuration.JwtConfiguration.JwtAudience)))
            {
                throw new SecurityException(Messages.TokenInvalidAud);
            }

            SetContext(claims);
        }
    }
}