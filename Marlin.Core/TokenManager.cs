using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Configuration;
using System;

namespace Marlin.Core
{
    public sealed class TokenManager
    {
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtDurationHours;
        private readonly string _jwtSecret;

        public TokenManager(IConfiguration configuration)
        {
            var configuration1 = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _jwtAudience = configuration1["Marlin:JwtAudience"];
            _jwtIssuer = configuration1["Marlin:JwtIssuer"];
            _jwtSecret = configuration1["Marlin:JwtSecret"];
            int.TryParse(configuration1["Marlin:JwtSecret"], out _jwtDurationHours);
        }

        /// <summary>
        /// Gets a jwt Token using the Current context claims
        /// </summary>
        /// <returns></returns>
        public string Jwt()
        {
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
