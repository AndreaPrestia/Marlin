using JWT.Algorithms;
using JWT.Builder;
using Marlin.Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Marlin.Core
{
    public class Context
    {
        [ThreadStatic] private static Context _context;

        private readonly MarlinConfiguration _marlinConfiguration;

        private Context()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configBuilder = new ConfigurationBuilder().AddJsonFile(!string.IsNullOrEmpty(environment)
                ? $"appsettings.{environment}.json"
                : "appsettings.json");
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

        public static Resource GetResource(string url, string method)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            if (method == null) throw new ArgumentNullException(nameof(method));


            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var resourcesContent = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(),
                !string.IsNullOrEmpty(environment)
                    ? $"resources.{environment}.json"
                    : "resources.json"));

            var resources = JsonConvert.DeserializeObject<List<Resource>>(resourcesContent)?.Where(x =>
                x.Url?.ToLower() == url.ToLower() && x.Method?.ToUpper() == method.ToUpper()).ToList();

            if (resources == null || resources.Count == 0)
            {
                throw new EntryPointNotFoundException(string.Format(Messages.ApiNotFound, url, method));
            }

            if (resources.Count > 1)
            {
                throw new HttpListenerException(StatusCodes.Status409Conflict,
                    (string.Format(Messages.ApiConflict, url, method)));
            }

            return resources.FirstOrDefault();
        }

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
                    .WithSecret(_marlinConfiguration.JwtConfiguration.JwtSecret)
                    .AddClaim("exp",
                        DateTimeOffset.UtcNow.AddHours(_marlinConfiguration.JwtConfiguration.JwtDurationHours)
                            .ToUnixTimeSeconds())
                    .AddClaim("aud", _marlinConfiguration.JwtConfiguration.JwtAudience)
                    .AddClaim("iss", _marlinConfiguration.JwtConfiguration.JwtIssuer)
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