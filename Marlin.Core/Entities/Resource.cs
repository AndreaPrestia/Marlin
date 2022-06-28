using System.Collections.Generic;

namespace Marlin.Core.Entities
{
    public class Resource
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public string Type { get; set; }
        public Dictionary<string, object> Claims { get; set; }
    }
}

