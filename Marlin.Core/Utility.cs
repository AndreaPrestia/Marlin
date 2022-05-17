using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marlin.Core
{
   internal static  class Utility
    {
        private static readonly JsonSerializerSettings Options = new JsonSerializerSettings() { StringEscapeHandling = StringEscapeHandling.Default };

        public static T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, Options);

        public static object Deserialize(string json, Type returnType) => JsonConvert.DeserializeObject(json, returnType);

        public static string Serialize(object o) => JsonConvert.SerializeObject(o, Options);

    }
}
