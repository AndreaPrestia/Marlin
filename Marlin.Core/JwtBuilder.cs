using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marlin.Core
{
    public class JwtBuilder
    {
        private Dictionary<string, object> Claims;

        private string Algorithm;

        private string Secret;

        private object Header;

        private object Payload;

        private string Token;

        private JwtBuilder()
        {

        }

        public static JwtBuilder Create()
        {
            return new JwtBuilder();
        }

        public JwtBuilder WithAlgorithm(string algorithm)
        {
            Algorithm = algorithm;

            return this;
        }

        public JwtBuilder WithSecret(string secret)
        {
            Secret = secret;

            return this;
        }

        public JwtBuilder AddClaim(string key, object value)
        {
            if (Claims == null)
            {
                Claims = new Dictionary<string, object>();
            }

            if (!Claims.ContainsKey(key))
            {
                Claims.Add(key, value);
            }

            return this;
        }

        public JwtBuilder Encode()
        {
            Header = new
            {
                alg = Algorithm,
                typ = "JWT"
            };

            Payload = new
            {
                Claims
            };

            //TODO encrypt signature

            //TODO generate token string base64 encoded

            return this;
        }

        public T Decode<T>(string token)
        {
            //TODO split by .
            //TODO assign 0 to header (convert from base64 to string and deserialize)
            //TODO assign 1 to body (convert from base64 to string and deserialize)
            //TODO decrypt signature with alg specified in header and Secret value.

            return default(T);
        }

        public string GetTokenString()
        {
            return Token;
        }
    }
}
