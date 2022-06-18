using Marlin.Core.Entities;
using Marlin.Core.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Marlin.Core
{
    internal class MarlinServer
    {
        X509Certificate2 serverCertificate = null;

        private readonly string _certificateName;
        private readonly string _safePassword;
        private readonly MarlinConfiguration _marlinConfiguration;
        private readonly IServiceProvider _serviceProvider;
        private TcpListener _tcpListener;
        private readonly IEventHandler _loggerHandler;

        public MarlinServer([NotNull] IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            _marlinConfiguration = (MarlinConfiguration)serviceProvider.GetService(typeof(MarlinConfiguration));

            _certificateName = _marlinConfiguration.CertificateName ?? "MarlinCert";
            _safePassword = _marlinConfiguration.CertificatePassword ?? "MarlinPassword.6%$";

            if (_marlinConfiguration.EventLoggerEnabled)
            {
                _loggerHandler = (IEventHandler)_serviceProvider.GetService(typeof(IEventHandler));
            }
        }

        internal void RunServer()
        {
            serverCertificate = GetCert();

            if (serverCertificate == null)
            {
                serverCertificate = GenerateCert();
                AddCertToStore(serverCertificate, StoreName.Root, StoreLocation.CurrentUser);
            }

            _tcpListener = new TcpListener(IPAddress.Any, _marlinConfiguration.Port);
            _tcpListener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                // Application blocks while waiting for an incoming connection.
                // Type CNTL-C to terminate the server.
                TcpClient client = _tcpListener.AcceptTcpClient();
                ProcessClient(client);
            }
        }

        internal void StopServer()
        {
            _tcpListener.Stop();
        }

        void ProcessClient(TcpClient client)
        {
            // A client has connected. Create the
            // SslStream using the client's network stream.
            SslStream sslStream = new SslStream(
                client.GetStream(), false);
            // Authenticate the server but don't require the client to authenticate.
            try
            {
                sslStream.AuthenticateAsServer(serverCertificate, clientCertificateRequired: false, checkCertificateRevocation: true);

                // Set timeouts for the read and write to 5 seconds.
                sslStream.ReadTimeout = 5000;
                sslStream.WriteTimeout = 5000;
                // Read a message from the client.
                string messageData = ReadMessage(sslStream);

                // Write a message to the client.
                byte[] message = Encoding.UTF8.GetBytes("Hello from the server.<EOF>");
                sslStream.Write(message);
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
                Console.WriteLine("Authentication failed - closing the connection.");
                sslStream.Close();
                client.Close();
                return;
            }
            finally
            {
                // The client stream will be closed with the sslStream
                // because we specified this behavior when creating
                // the sslStream.
                sslStream.Close();
                client.Close();
            }
        }
        string ReadMessage(SslStream sslStream)
        {
            // Read the  message sent by the client.
            // The client signals the end of the message using the
            // "<EOF>" marker.
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                // Read the client's test message.
                bytes = sslStream.Read(buffer, 0, buffer.Length);

                // Use Decoder class to convert from bytes to UTF8
                // in case a character spans two buffers.
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);
                // Check for EOF or an empty message.
                if (messageData.ToString().IndexOf("<EOF>") != -1)
                {
                    break;
                }
            } while (bytes != 0);

            return messageData.ToString();
        }

        private X509Certificate2 GenerateCert()
        {
            var sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName("localhost");
            sanBuilder.AddDnsName(Environment.MachineName);


            var distinguishedName = new X500DistinguishedName($"CN={_certificateName}");

            using var rsa = RSA.Create(2048);
            var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));


            request.CertificateExtensions.Add(
                new X509EnhancedKeyUsageExtension(
                    new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));

            request.CertificateExtensions.Add(sanBuilder.Build());

            var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));
            certificate.FriendlyName = _certificateName;

            return new X509Certificate2(certificate.Export(X509ContentType.Pfx, _safePassword), _safePassword, X509KeyStorageFlags.MachineKeySet);
        }

        private void AddCertToStore(X509Certificate2 cert, StoreName st, StoreLocation sl)
        {
            var store = new X509Store(st, sl);
            store.Open(OpenFlags.ReadWrite);
            store.Add(cert);

            store.Close();
        }

        private X509Certificate2 GetCert()
        {
            var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);

            store.Open(OpenFlags.ReadOnly);

            var certificates = store.Certificates.Find(
                X509FindType.FindBySubjectName,
                _certificateName,
                false);

            return certificates?[0];
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
                    throw new InvalidOperationException(string.Format(Messages.InvalidContentType, context.Request.ContentType, contentType));
                }

                if (context.Request.Path.Equals("/") || context.Request.Path.Equals("/favicon.ico"))
                {
                    content = "Server is up and running :)";
                }
                else
                {
                    requestBody = new StreamReader(context.Request.Body).ReadToEndAsync().Result;

                    var apiOutput = Process(new ApiInput(context), requestBody);

                    contentType = apiOutput.ContentType;
                    statusCode = apiOutput.StatusCode;
                    content = apiOutput.Response;
                }
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

                if (_loggerHandler != null)
                {
                    //logger service, write event
                    _loggerHandler?.WriteEvent(new Entities.Event()
                    {
                        Level = string.IsNullOrEmpty(message) ? EventLevels.Info.ToString() : EventLevels.Error.ToString(),
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
                }

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

                    args[i] = value ?? throw new ArgumentException(string.Format(Messages.EntityNotProvided, parameters[i].Name));
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
            var friendlyName = AppDomain.CurrentDomain.FriendlyName;

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

        public void SetContext(Dictionary<string, object> claims)
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

            if (!hasAudience && (audience == null || !audience.ToString().Equals(_configuration.JwtConfiguration.JwtAudience)))
            {
                throw new SecurityException(Messages.TokenInvalidAud);
            }

            SetContext(claims);
        }
    }
}
