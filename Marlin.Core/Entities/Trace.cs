namespace Marlin.Core.Entities
{
    public class Trace
    {
        public string ClassName { get; set; }
        public string Method { get; set; }
        public string Hostname { get; set; }
        public string ClientIp { get; set; }
        public string Username { get; set; }
        public string Url { get; set; }
        public string Payload { get; set; }
        public string Query { get; set; }
        public long Created { get; set; }
        public double Millis { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
    }
}
