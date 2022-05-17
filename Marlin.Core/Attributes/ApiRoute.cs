using System;

namespace Marlin.Core.Attributes
{
    public sealed class ApiRoute : Attribute
    {
        private string _url;
        private string _method;

        public ApiRoute(string url, string method)
        {
            _url = url;
            _method = method;
        }

        public string Url { get => _url; }
        public string Method { get => _method; }
    }
}
