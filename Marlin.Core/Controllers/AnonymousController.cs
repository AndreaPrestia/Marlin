using Marlin.Core.Common;
using Marlin.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;

namespace Marlin.Core.Controllers
{
    [ResponseCache(Duration = -1, Location = ResponseCacheLocation.None, NoStore = true)]
    public class AnonymousController : Controller
    {
        private static readonly NLog.Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        private readonly TimeKeeper timeKeeper = new TimeKeeper();

        protected double TimeKeeperDuration
        {
            get
            {
                return timeKeeper.Stop().TotalMilliseconds;
            }
        }

        protected HttpContext CurrentHttpContext
        {
            get
            {
                return HttpContext;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            Context.Current.Reset();

            string scheme = HttpContext.Request.Scheme;
            string host = HttpContext.Request.Host.Value;
            string path = HttpContext.Request.Path;
            string queryString = HttpContext.Request.QueryString.HasValue ? HttpContext.Request.QueryString.Value : "";

            string url = $"{scheme}://{host}{path}{queryString}";

            logger.Info($"Request {url} from ip {HttpContext.Connection.RemoteIpAddress}");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            double executionMS = timeKeeper.Stop().TotalMilliseconds;

            logger.Info($"Execution require {executionMS} ms");

            Business.System.TraceWrite(new Trace()
            {
                Millis = executionMS,
                Created = Helper.GetUnixTimestamp(DateTime.Now),
                ClientIp = HttpContext.Connection.RemoteIpAddress.ToString(),
                Method = HttpContext.Request.Method,
                Url = HttpContext.Request.Path,
                Query = HttpContext.Request.QueryString.Value,
                Username = Context.Current.User?.Username,
                Payload = new StreamReader(HttpContext.Request.Body).ReadToEnd(),
                ClassName = this.GetType().Name,
                Error = context.Exception?.StackTrace,
                Hostname = HttpContext.Connection.LocalIpAddress.ToString(),
                Message = context.Exception?.Message
            });
        }

        protected void ValidateEntity<T>(T entity)
        {
            if (entity == null)
            {
                throw new ApplicationException("Request is null");
            }

            PropertyInfo[] props = entity.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                IEnumerable<Attribute> attributes = prop.GetCustomAttributes();

                foreach (Attribute attribute in attributes)
                {
                    if (attribute is RequiredAttribute)
                    {
                        if (prop.PropertyType == typeof(string))
                        {
                            string s = prop.GetValue(entity) as string;

                            if (string.IsNullOrWhiteSpace(s))
                            {
                                throw new KeyNotFoundException(prop.Name);
                            }
                        }
                        else if (prop.PropertyType == typeof(IList) || prop.PropertyType == typeof(Array))
                        {
                            if (prop.GetValue(entity) == null)
                            {
                                throw new KeyNotFoundException(prop.Name);
                            }
                        }
                        else if (prop.PropertyType == typeof(IList) || prop.PropertyType == typeof(Array))
                        {
                            if (prop.GetValue(entity) == null)
                            {
                                throw new KeyNotFoundException(prop.Name);
                            }
                        }
                        else if (prop.PropertyType == typeof(object))
                        {
                            if (prop.GetValue(entity) == null)
                            {
                                throw new KeyNotFoundException(prop.Name);
                            }
                        }
                        else if (prop.PropertyType == typeof(Guid))
                        {
                            //TODO fix
                            //string s = prop.GetValue(t) as string;

                            //if (string.IsNullOrWhiteSpace(s))
                            //{
                            //    throw new MissingRequiredProperty(prop.Name);
                            //}

                            //Guid g = Guid.Parse(s);

                            //if (g == Guid.Empty)
                            //{
                            //    throw new MissingRequiredProperty(prop.Name);
                            //}
                        }
                    }
                }
            }
        }

        protected IActionResult GetOutput(object response = null, int statusCode = 200, string contentType = null)
        {
            return new ContentResult() { Content = response != null ? Helper.Serialize(response) : string.Empty, ContentType = string.IsNullOrEmpty(contentType) ? "application/json" : contentType, StatusCode = response == null ? StatusCodes.Status204NoContent : statusCode };
        }

        protected void RequireUser()
        {
            if (Context.Current.User == null)
            {
                throw new ApplicationException("User is null");
            }
        }

        protected void ThrowCustomJson(string json)
        {
            throw new InvalidDataException(json);
        }

        protected void ThrowInvalidArgument()
        {
            throw new ArgumentException("Argument not found");
        }

        protected void ThrowInvalidArgument(string message)
        {
            throw new ArgumentException(message);
        }

        protected void ThrowAccessDenied(string message)
        {
            throw new UnauthorizedAccessException(message);
        }

        protected void ThrowAccessDenied()
        {
            throw new UnauthorizedAccessException("Access denied");
        }

        protected void ThrowNotFound()
        {
            throw new ArgumentNullException("Object not found");
        }

        protected void ThrowNotFound(string message)
        {
            throw new ArgumentNullException(message);
        }

        protected void ThrowApplicationError(string message)
        {
            throw new ApplicationException(message);
        }
    }
}
