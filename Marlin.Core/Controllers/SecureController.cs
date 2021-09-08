using JWT.Algorithms;
using JWT.Builder;
using Marlin.Core.Common;
using Marlin.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Security;

namespace Marlin.Core.Controllers
{
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    public class SecureController : AnonymousController
    {
        private static readonly NLog.Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        readonly string refreshResource = Settings.Get("RefreshResourceName", "system/refresh.api");

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            string scheme = HttpContext.Request.Scheme;
            string host = HttpContext.Request.Host.Value;
            string path = HttpContext.Request.Path;
            string queryString = HttpContext.Request.QueryString.HasValue ? HttpContext.Request.QueryString.Value : "";
            string method = HttpContext.Request.Method;
            string bearer = HttpContext.Request.Headers["Authorization"];

            string url = $"{scheme}://{host}{path}{queryString}";

            logger.Info($"Request: {url}");

            if (!path.EndsWith(".api"))
            {
                logger.Warn("Invalid request;");

                throw new InvalidOperationException("Invalid request path");
            }

            bool isPublic = Business.Authorization.IsPublicResource(path, method);

            if (isPublic)
            {
                logger.Info($"Resource {path};{method} is public");

                return;
            }

            LoadUser(bearer);

            bool canAccess = Business.Authorization.CanAccessResource(path, method);

            if (!canAccess)
            {
                if (Settings.Get<bool>("ThrowsOnAuthorizationFailed"))
                {
                    logger.Warn($"Access denied for resource {path}");

                    Business.System.TraceWrite(new Trace()
                    {
                        Millis = TimeKeeperDuration,
                        Created = Helper.GetUnixTimestamp(DateTime.Now),
                        ClientIp = HttpContext.Connection.RemoteIpAddress.ToString(),
                        Method = HttpContext.Request.Method,
                        Url = HttpContext.Request.Path,
                        Query = HttpContext.Request.QueryString.Value,
                        Username = Context.Current.User?.Username,
                        Payload = new StreamReader(HttpContext.Request.Body).ReadToEnd(),
                        ClassName = this.GetType().Name,
                        Error = "Authorization failed",
                        Hostname = HttpContext.Connection.LocalIpAddress.ToString()
                    });
                   
                    throw new SecurityException("Authorization failed");
                }
                else
                {
                    logger.Warn($"Access denied for resource {path}");

                    Business.System.TraceWrite(new Trace()
                    {
                        Millis = TimeKeeperDuration,
                        Created = Helper.GetUnixTimestamp(DateTime.Now),
                        ClientIp = HttpContext.Connection.RemoteIpAddress.ToString(),
                        Method = HttpContext.Request.Method,
                        Url = HttpContext.Request.Path,
                        Query = HttpContext.Request.QueryString.Value,
                        Username = Context.Current.User?.Username,
                        Payload = new StreamReader(HttpContext.Request.Body).ReadToEnd(),
                        ClassName = this.GetType().Name,
                        Error = "Authorization failed",
                        Hostname = HttpContext.Connection.LocalIpAddress.ToString()
                    });

                    Redirect(Settings.Get<string>("RedirectPathOnAccessDenied"));
                }
            }

            logger.Info($"User {Context.Current.User.Id} can access path {path}: {canAccess}");
        }

        private void LoadUser(string bearer)
        {
            logger.Info($"Bearer received: {bearer}");

            string secret = Settings.Get<string>("ServerSecret");

            if (String.IsNullOrWhiteSpace(secret))
            {
                throw new SecurityException("Invalid token");
            }

            var json = JwtBuilder.Create()
       .WithAlgorithm(new HMACSHA256Algorithm())
       .WithSecret(secret)
       .MustVerifySignature()
       .Decode(bearer);

            logger.Debug($"Json from bearer: {json}");

            Bearer b = Helper.Deserialize<Bearer>(json);

            if (b == null || b.User == null)
            {
                return;
            }

            DateTime expiration = Helper.GetFromUnixTimestamp(b.Exp);

            if (expiration < DateTime.Now && HttpContext.Request.Path != refreshResource)
            {
                throw new SecurityException("Token expired");
            }

            Context.Current.User = b.User;
        }
    }
}
