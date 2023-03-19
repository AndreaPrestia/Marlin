using Microsoft.AspNetCore.Http;

namespace Marlin.Core
{
    internal class ApiInput 
    {
        internal string Url => Context.Request?.Path;
        internal string Method => Context.Request?.Method;
        internal HttpContext Context { get; }
        internal ApiInput(HttpContext context)
        {
            Context = context;
        }
    }
}
