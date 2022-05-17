using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marlin.Core.Entities
{
    public class Event
    {
        public Event()
        {

        }

        public Event(HttpContext httpContext, double durationMilliseconds, string message, string requestBody, string response, IOrganization organization, IUser user)
        {
            Level = new Level() { Id = string.IsNullOrEmpty(message) ? (int)EventLevels.Info : (int)EventLevels.Error };
            Organization = organization;
            User = user;
            Protocol = httpContext.Request.ProtocolVersion.ToString();
            Url = httpContext.Request.Url?.LocalPath;
            Method = httpContext.Request.HttpMethod;
            Request = httpContext.Request.QueryString.ToString();
            Response = response;
            Host = httpContext.Request.UserHostAddress;
            Client = httpContext.Request.RemoteEndPoint?.ToString();
            Payload = requestBody;
            Message = message;
            Milliseconds = durationMilliseconds;
        }

        public Guid Id { get; set; }
        public long Created { get; set; }
        public string Level { get; set; }
        public Dictionary<string, object> Claims { get; set; }
        public string Protocol { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string Host { get; set; }
        public string Client { get; set; }
        public string Payload { get; set; }
        public string Message { get; set; }
        public double Milliseconds { get; set; }
    }
}
