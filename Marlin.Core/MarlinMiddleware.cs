using JWT.Algorithms;
using JWT.Builder;
using Marlin.Core.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Marlin.Core
{
    public class MarlinMiddleware
    {
        private readonly MarlinConfiguration _configuration;
        private readonly RequestDelegate _next;
        private readonly ILogger<MarlinMiddleware> _logger;

        public MarlinMiddleware(RequestDelegate next, MarlinConfiguration configuration,
            IServiceProvider serviceProvider, ILogger<MarlinMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));

            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            if (configuration.JwtConfiguration == null)
            {
                throw new ArgumentNullException(nameof(configuration.JwtConfiguration));
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            var contentType = ContentType.TextPlain;
            
            _logger.LogDebug($"Request started at {DateTime.UtcNow}");

            try
            {
                Context.Current.Reset();

                if (string.IsNullOrEmpty(context.Request.ContentType))
                {
                    context.Request.ContentType = contentType;
                }

                var resource = Context.GetResource(context.Request.Path, context.Request.Method);

                if (resource.Claims != null && resource.Claims.Any())
                {
                    ManageResource(context, resource);
                }

                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                var statusCode = StatusCodes.Status500InternalServerError;
                contentType = ContentType.ApplicationJson;
                var message = _configuration.PropagateApplicationError ? e.Message : Messages.GenericFailure;
                var content = JsonConvert.SerializeObject(new { Message = message });

                statusCode = e switch
                {
                    ArgumentException => StatusCodes.Status400BadRequest,
                    SecurityException => StatusCodes.Status401Unauthorized,
                    UnauthorizedAccessException => StatusCodes.Status403Forbidden,
                    EntryPointNotFoundException => StatusCodes.Status404NotFound,
                    HttpListenerException exception => exception.ErrorCode,
                    _ => StatusCodes.Status500InternalServerError
                };
                
                context.Response.ContentType = contentType;
                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsync(content);
            }
        }
     
        private void ManageResource(HttpContext context, Resource resource)
        {
            if (Context.Current == null || !Context.IsLoaded)
            {
                this.LoadContext(context);
            }

            if (Context.Current == null || !Context.IsLoaded)
            {
                throw new SecurityException(Messages.Unauthorized);
            }

            foreach (var claim in resource.Claims)
            {
                if (!Context.HasClaim(claim.Key))
                {
                    throw new UnauthorizedAccessException(string.Format(Messages.ApiNotAuthorizedClaim,
                        context.Request.Path,
                        context.Request.Method));
                }

                if (claim.Value.Equals("*"))
                {
                    continue;
                }

                var claimContent = Context.GetClaim<string>(claim.Key);

                if (string.IsNullOrEmpty(claimContent) || !claim.Value.ToString().Contains(claimContent))
                {
                    throw new UnauthorizedAccessException(string.Format(Messages.ApiNotAuthorizedClaim,
                        context.Request.Path,
                        context.Request.Method));
                }
            }
        }

        private static void SetContext(Dictionary<string, object> claims)
        {
            foreach (var claim in claims)
            {
                Context.Add(claim.Key, claim.Value);
            }
        }

        private void LoadContext(HttpContext context)
        {
            var tokenString = context.Request.Headers[Messages.HeaderAuthorization].FirstOrDefault();

            Console.WriteLine($"Token received: {tokenString}");

            if (string.IsNullOrEmpty(tokenString))
            {
                throw new SecurityException(Messages.TokenNotProvided);
            }

            var jwtContent = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_configuration.JwtConfiguration.JwtSecret)
                .MustVerifySignature()
                .WithVerifySignature(true)
                .Decode<Dictionary<string, object>>(tokenString);

            if (jwtContent == null)
            {
                throw new SecurityException(Messages.TokenInvalidPayload);
            }

            var claims = jwtContent.Where(x =>
                x.Key != "exp"
                && x.Key != "iss"
                && x.Key != "aud"
                && x.Key != "iat"
                && x.Key != "jti").ToDictionary(i => i.Key, i => i.Value);

            if (claims == null || claims.Count == 0)
            {
                throw new SecurityException(Messages.TokenInvalidClaims);
            }

            var hasExpiring = long.TryParse(jwtContent["exp"].ToString(), out long expiring);

            if (!hasExpiring)
            {
                throw new SecurityException(string.Format(Messages.TokenClaimNotProvided, "exp"));
            }

            if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > expiring)
            {
                throw new SecurityException(Messages.TokenExpired);
            }

            var hasIssuer = jwtContent.TryGetValue("iss", out object issuer);

            if (!hasIssuer && (issuer == null || !issuer.ToString().Equals(_configuration.JwtConfiguration.JwtIssuer)))
            {
                throw new SecurityException(Messages.TokenInvalidIss);
            }

            var hasAudience = jwtContent.TryGetValue("aud", out object audience);

            if (!hasAudience && (audience == null ||
                                 !audience.ToString().Equals(_configuration.JwtConfiguration.JwtAudience)))
            {
                throw new SecurityException(Messages.TokenInvalidAud);
            }

            SetContext(claims);
        }
    }
}