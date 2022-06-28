using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Marlin.Core
{
    public class ApiInput 
    {
        public string Url => Context.Request?.Path;
        public string Method => Context.Request?.Method;
        public  string Body { get; private set; }
        public HttpContext Context { get; }
        public ApiInput(HttpContext context, string body)
        {
            Context = context;
            Body = body;
        }
        
        public T DeserializeBody<T>()
        {
            if (string.IsNullOrEmpty(Body))
            {
                throw new ArgumentNullException(Messages.RequestBodyNotProvided);
            }
            
            var entity =  JsonConvert.DeserializeObject<T>(Body);

            return entity;
        }

        public T GetHeader<T>(string name, bool required = false)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            
            var value = Context.Request.Headers[name].FirstOrDefault();

            if (value == null && required)
            {
                throw new ArgumentException(string.Format(Messages.HeaderNotProvided, name));
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
        
        public T GetQueryParameter<T>(string name, bool required = false)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            
            var value = Context.Request.Query[name].FirstOrDefault();

            if (value == null && required)
            {
                throw new ArgumentException(string.Format(Messages.ParameterNotProvided, name));
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
