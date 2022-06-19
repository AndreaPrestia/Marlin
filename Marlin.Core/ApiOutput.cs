namespace Marlin.Core
{
    public class ApiOutput
    {
        public ApiOutput(string data = null, int statusCode = StatusCodes.Status200Ok, string contentType = Core.ContentType.ApplicationJson)
        {
            Response = data ?? string.Empty;
            StatusCode = statusCode;
            ContentType = contentType;
        }

        public string Response { get; set; }
        public int StatusCode { get; set; }
        public string ContentType { get; set; }
    }
}
