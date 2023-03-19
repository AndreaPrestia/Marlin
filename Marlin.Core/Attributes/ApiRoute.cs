using System;

namespace Marlin.Core.Attributes
{
    public sealed class ApiRoute : Attribute
    {
        public ApiRoute(string url, string method)
        {
            Url = url;
            Method = method;
        }

        public string Url { get; }
        public string Method { get; }
    }
}
