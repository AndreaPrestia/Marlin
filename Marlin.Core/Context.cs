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

        private const string configurationJwtSecretKey = "Marlin:JwtSecret";
        private const string configurationJwtIssuerKey = "Marlin:JwtIssuer";
        private const string configurationJwtAudienceKey = "Marlin:JwtAudience";
        private const string configurationJwtDurationHoursKey = "Marlin:JwtDurationHours";
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtDurationHours;
        private readonly string _jwtSecret;

        private Context()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configBuilder = new ConfigurationBuilder().AddJsonFile(!string.IsNullOrEmpty(environment) ? $"appsettings.{environment}.json" : "appsettings.json");
            var configuration = configBuilder.Build();

            _jwtAudience = configuration[configurationJwtAudienceKey] ?? throw new ArgumentNullException(string.Format(Messages.ConfigurationNotValidOrNotProvided, configurationJwtAudienceKey));
            _jwtIssuer = configuration[configurationJwtIssuerKey] ?? throw new ArgumentNullException(string.Format(Messages.ConfigurationNotValidOrNotProvided, configurationJwtIssuerKey));
            _jwtSecret = configuration[configurationJwtSecretKey] ?? throw new ArgumentNullException(string.Format(Messages.ConfigurationNotValidOrNotProvided, configurationJwtSecretKey));

            _jwtDurationHours = int.Parse(configuration[configurationJwtDurationHoursKey]);
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

        public static bool IsLoaded => _context != null && _context.Claims != null;

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
                    .WithSecret(_jwtSecret)
                    .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(_jwtDurationHours).ToUnixTimeSeconds())
                    .AddClaim("aud", _jwtAudience)
                    .AddClaim("iss", _jwtIssuer)
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
