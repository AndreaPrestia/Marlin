using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security;

namespace Marlin.Core
{
    public class Context
    {
        [ThreadStatic]
        private static Context _context;

        private readonly IConfiguration _configuration;

        private Context()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configBuilder = new ConfigurationBuilder().AddJsonFile(!string.IsNullOrEmpty(environment) ? $"appsettings.{environment}.json" : "appsettings.json");
            _configuration = configBuilder.Build();
        }

        private Dictionary<string, object> Claims { get; set; }

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

        public static bool IsLoaded => _context is { Claims: { } };

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
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(_configuration["Jwt:Secret"])
                    .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:Minutes"])).ToUnixTimeSeconds())
                    .AddClaim("aud", _configuration["Jwt:Audience"])
                    .AddClaim("iss",  _configuration["Jwt:Issuer"])
                    .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds());

                foreach (var claim in Current.Claims)
                {
                    tokenBuilder.AddClaim(claim.Key, claim.Value);
                }

                return tokenBuilder.Encode();
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
