using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace Marlin.Core
{
    public sealed class TokenManager
    {
        private const string configurationJwtSecretKey = "Marlin:JwtSecret";
        private const string configurationJwtIssuerKey = "Marlin:JwtIssuer";
        private const string configurationJwtAudienceKey = "Marlin:JwtAudience";
        private const string configurationJwtDurationHoursKey = "Marlin:JwtDurationHours";
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtDurationHours;
        private readonly string _jwtSecret;

        public TokenManager([NotNull]IConfiguration configuration)
        {
            _jwtAudience = configuration[configurationJwtAudienceKey] ?? throw new ArgumentNullException(string.Format(Messages.ConfigurationNotValidOrNotProvided, configurationJwtAudienceKey));
            _jwtIssuer = configuration[configurationJwtIssuerKey] ?? throw new ArgumentNullException(string.Format(Messages.ConfigurationNotValidOrNotProvided, configurationJwtIssuerKey));
            _jwtSecret = configuration[configurationJwtSecretKey] ?? throw new ArgumentNullException(string.Format(Messages.ConfigurationNotValidOrNotProvided, configurationJwtSecretKey));
            bool jwtDurationConversion = int.TryParse(configuration[configurationJwtDurationHoursKey], out _jwtDurationHours);

            if (!jwtDurationConversion)
            {
                throw new ArgumentException(string.Format(Messages.ConfigurationNotValidOrNotProvided, configurationJwtDurationHoursKey));
            }
        }

        /// <summary>
        /// Gets a jwt Token using the Current context claims
        /// </summary>
        /// <returns></returns>
        public string Jwt()
        {
            if (!Context.IsLoaded)
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

            foreach (var claim in Context.Current.Claims)
            {
                tokenBuilder.AddClaim(claim.Key, claim.Value);
            }

            return tokenBuilder.Encode();
        }
    }
}
