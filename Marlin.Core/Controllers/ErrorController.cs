using Marlin.Core.Common;
using Marlin.Core.Entities;
using Marlin.Core.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security;

namespace Marlin.Core.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private static readonly NLog.Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        readonly bool propagateApplicationErrorInFault = Settings.Get("PropagateApplicationErrorInFault", false);

        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var e = context?.Error;

            string message = propagateApplicationErrorInFault ? e.Message : "An error has occurred";

            logger.Error(e);

            int statusCode = 500;

            if (e is ArgumentException || e is ArgumentNullException)
            {
                statusCode = 400;
            }
            else if (e is EntryPointNotFoundException)
            {
                statusCode = 404;
            }
            else if (e is UnauthorizedAccessException)
            {
                Context.Current.Reset();

                statusCode = 401;
            }
            else if (e is SecurityException)
            {
                Context.Current.Reset();

                statusCode = 403;
            }

            Business.System.TraceWrite(new Trace()
            {
                Created = Helper.GetUnixTimestamp(DateTime.Now),
                ClientIp = HttpContext.Connection.RemoteIpAddress.ToString(),
                Method = HttpContext.Request.Method,
                Url = HttpContext.Request.Path,
                Query = HttpContext.Request.QueryString.Value,
                Username = Context.Current.User?.Username,
                Payload = new StreamReader(HttpContext.Request.Body).ReadToEnd(),
                ClassName = this.GetType().Name,
                Error = e.StackTrace,
                Hostname = HttpContext.Connection.LocalIpAddress.ToString(),
                Message = e.Message
            });

            return new ContentResult()
            {
                Content = Helper.Serialize(new ErrorResponse { Message = message }),
                ContentType = ContentType.ApplicationJson,
                StatusCode = statusCode
            };
        }
    }
}
