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
        private static readonly char[] Padding = { '=' };

        private readonly Dictionary<string, object> _claims;

        private ICryptoAlghorithm _algorithm;

        private string _signature;

        private string _secret;

        private object _header;

        private object _payload;

        private string _token;

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
            { AllowTrailingCommas = true, PropertyNameCaseInsensitive = true };

        private JwtBuilder()
        {
            _claims = new Dictionary<string, object>();
            _signature = string.Empty;
            _secret = string.Empty;
            _header = null;
            _payload = null;
            _token = string.Empty;
            _algorithm = null;
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
                throw new ArgumentNullException($"Secret not provided");
            }

            _header = new
            {
                alg = _algorithm?.AlgorithmName,
                typ = "JWT"
            };

            _payload = new
            {
                Claims = _claims
            };

            var encodedHeader = EncodeBase64(_header);
            var encodedPayload = EncodeBase64(_payload);

            try
            {
                var message = string.Join(".", encodedHeader, encodedPayload);

                if (_algorithm == null)
                {
                    throw new ArgumentNullException(nameof(_algorithm));
                }

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
                throw new SecurityException("Token not provided");
            }

            var tokenMembers = token.Split('.');

            if (tokenMembers.Length != 3)
            {
                throw new SecurityException("Invalid token");
            }

            try
            {
                var encodedHeader = DecodeBase64(tokenMembers[0]);
                var encodedPayload = DecodeBase64(tokenMembers[1]);

                var message = string.Join(".", encodedHeader, encodedPayload);

                var signature = _algorithm?.GetSignature(message, _secret);

                if (string.IsNullOrEmpty(signature))
                {
                    throw new ArgumentNullException(nameof(signature));
                }

                if (!signature.Equals(tokenMembers[2], StringComparison.Ordinal))
                {
                    throw new SecurityException("Invalid token signature");
                }

                return JsonSerializer.Deserialize<T>(
                    Encoding.UTF8.GetString(Convert.FromBase64String(SetPadding(encodedPayload))), Options);
            }
            catch (Exception ex)
            {
                throw new SecurityException(ex.Message);
            }
        }

        public string GetTokenString()
        {
            return _token;
        }

        private static string DecodeBase64(string input)
        {
            return input
                .Replace('_', '/').Replace('-', '+');
        }

        private static string SetPadding(string input)
        {
            switch (input.Length % 4)
            {
                case 2:
                    input += "==";
                    break;
                case 3:
                    input += "=";
                    break;
            }

            return input;
        }

        private static string EncodeBase64(object content)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(JsonSerializer.Serialize(content, Options)))
                .TrimEnd(Padding).Replace('+', '-').Replace('/', '_');
        }
    }
}