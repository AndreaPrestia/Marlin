using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Text.Json;
using Marlin.Core.Interfaces;

namespace Marlin.Core
{
    public class JwtBuilder
    {
        private Dictionary<string, object> _claims;

        private ICryptoAlghorithm _algorithm;

        private string _signature;

        private string _secret;

        private object _header;

        private object _payload;

        private string _token;

        private JwtBuilder()
        {

        }

        public static JwtBuilder Create()
        {
            return new JwtBuilder();
        }

        public JwtBuilder WithAlgorithm<T>() where T : ICryptoAlghorithm
        {
            _algorithm = Activator.CreateInstance<T>();

            return this;
        }

        public JwtBuilder WithSecret(string secret)
        {
            _secret = secret;

            return this;
        }

        public JwtBuilder AddClaim(string key, object value)
        {
            _claims ??= new Dictionary<string, object>();

            if (!_claims.ContainsKey(key))
            {
                _claims.Add(key, value);
            }

            return this;
        }

        public JwtBuilder Encode()
        {
            if (string.IsNullOrEmpty(_secret))
            {
                throw new ArgumentNullException(Messages.TokenSecretNotProvided);
            }
            
            _header = new
            {
                alg = _algorithm.GetType().Name,
                typ = Messages.TokenFormat
            };

            _payload = new
            {
                Claims = _claims
            };

            var encodedHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(_header)));
            var encodedPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(_payload)));

            try
            {
                var message = string.Join(".", encodedHeader , encodedPayload);

                _signature = _algorithm.GetSignature(message, _secret);
            }
            catch (Exception ex)
            {
                _header = null;
                _payload = null;
                
                throw new SecurityException(ex.Message);
            }

            _token = string.Join(".", encodedHeader, encodedPayload, _signature);

            return this;
        }

        public T Decode<T>(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new SecurityException(Messages.TokenNotProvided);
            }

            var tokenMembers = token.Split('.');

            if (tokenMembers.Length != 3)
            {
                throw new SecurityException(Messages.TokenInvalid);
            }
            
            try
            {
                var encodedHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(tokenMembers[0])));
                var encodedPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(tokenMembers[1])));

                var message = string.Join(".", encodedHeader , encodedPayload);

                var signature = _algorithm.GetSignature(message, _secret);

                if (!signature.Equals(tokenMembers[2]))
                {
                    throw new SecurityException(Messages.TokenInvalidSignature);
                }
            }
            catch (Exception ex)
            {
                throw new SecurityException(ex.Message);
            }

            return JsonSerializer.Deserialize<T>(tokenMembers[1]);
        }

        public string GetTokenString()
        {
            return _token;
        }
    }
}
