using Microsoft.AspNetCore.Http;

namespace Marlin.Core
{
    public class ApiInput 
    {
        public string Url => Context.Request?.Path;
        public string Method => Context.Request?.Method;
        public HttpContext Context { get; }
        public ApiInput(HttpContext context)
        {
            Context = context;
        }
    }
}
