using System.Net;

namespace Marlin.Core
{
    public class ApiInput 
    {
        public string Url => Context.Request?.Url.AbsolutePath;
        public string Method => Context.Request?.HttpMethod;
        public HttpListenerContext Context { get; }
        public ApiInput(HttpListenerContext context)
        {
            Context = context;
        }
    }
}
