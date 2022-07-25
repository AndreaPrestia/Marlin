using Marlin.Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security;
using Marlin.Core.Encrypt;

namespace Marlin.Core
{
    public class Context
    {
        [ThreadStatic]
        private static Context _context;

        private readonly MarlinConfiguration _marlinConfiguration;

        private Context()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configBuilder = new ConfigurationBuilder().AddJsonFile(!string.IsNullOrEmpty(environment) ? $"appsettings.{environment}.json" : "appsettings.json");
            var configuration = configBuilder.Build();

            _marlinConfiguration = configuration.GetSection("Marlin").Get<MarlinConfiguration>();

            if (_marlinConfiguration == null || _marlinConfiguration.JwtConfiguration == null)
            {
                throw new ArgumentNullException(nameof(_marlinConfiguration));
            }
        }

        public Dictionary<string, object> Claims { get; private set; }

        public static T GetClaim<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var claimFound = _context.Claims.TryGetValue(name.ToLowerInvariant().Trim(), out var claimContent);

            if (!claimFound || claimContent == null)
            {
                throw new SecurityException(string.Format(Messages.TokenClaimNotProvided, name));
            }

            return (T)claimContent;
        }

        public static void Add<T>(string name, T value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _context.Claims ??= new Dictionary<string, object>();

            if (_context.Claims.ContainsKey(name.ToLowerInvariant().Trim()))
            {
                _context.Claims[name] = value;
            }
            else
            {
                _context.Claims.Add(name.ToLowerInvariant().Trim(), value);
            }
        }

        public static bool IsLoaded => _context?.Claims != null;

        public static bool HasClaim(string name) => _context.Claims.ContainsKey(name.ToLowerInvariant().Trim());

        public void Reset()
        {
            this.Claims = null;
        }

        public string Jwt
        {
            get
            {
                if (!IsLoaded)
                {
                    throw new SecurityException(Messages.ContextNotLoaded);
                }

                var tokenBuilder = JwtBuilder.Create()
                    .WithAlgorithm<HMACSHA256>()
                    .WithSecret(_marlinConfiguration.JwtConfiguration.JwtSecret)
                    .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(_marlinConfiguration.JwtConfiguration.JwtDurationHours).ToUnixTimeSeconds())
                    .AddClaim("aud", _marlinConfiguration.JwtConfiguration.JwtAudience)
                    .AddClaim("iss", _marlinConfiguration.JwtConfiguration.JwtIssuer)
                    .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds());

                foreach (var claim in Current.Claims)
                {
                    tokenBuilder.AddClaim(claim.Key, claim.Value);
                }

                return tokenBuilder.Encode().GetTokenString();
            }
        }

        public static Context Current
        {
            get
            {
                _context ??= new Context();

                return _context;
            }
        }
    }
}
