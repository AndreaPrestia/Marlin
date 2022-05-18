using System;
using System.Collections.Generic;

namespace Marlin.Core.Entities
{
    public class Event
    {
        public Event()
        {
            Id = Guid.NewGuid();
            Created = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
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
