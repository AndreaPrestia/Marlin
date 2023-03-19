using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Marlin.Core
{
    public static class Extensions
    {
        private static readonly string
            Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        /// <summary>
        /// Extension that add cors, cookie policy options
        /// </summary>
        /// <param name="services"></param>
        public static void AddMarlin(this IServiceCollection services)
        {
            var origins = GetCorsOrigins();

            if (origins != null && !origins.All(x => x.Equals("*", StringComparison.InvariantCultureIgnoreCase)))
            {
                services.AddCors();
            }
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = _ => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.None;
                options.HttpOnly = HttpOnlyPolicy.None;
            });
        }

        /// <summary>
        /// Extension that use cors with origins from app.settings.json, use cookie policy and adds the middleware
        /// </summary>
        /// <param name="builder"></param>
        public static void UseMarlin(this IApplicationBuilder builder)
        {
            var origins = GetCorsOrigins();

            if (origins != null && !origins.All(x => x.Equals("*", StringComparison.InvariantCultureIgnoreCase)))
            {
                builder.UseCors(corsPolicyBuilder => corsPolicyBuilder
                    .WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            }

            builder.UseCookiePolicy();

            builder.MapWhen(x => x.Request.Path.Equals("/"),
                appBranch => appBranch.Run(async context =>
                {
                    await context.Response.WriteAsync("Server is up and running :)");
                }));

            builder.UseWhen(x => !x.Request.Path.Equals("/"), appBranch => appBranch.UseMiddleware<MarlinMiddleware>());
        }

        private static string[] GetCorsOrigins()
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile(!string.IsNullOrEmpty(Environment)
                ? $"appsettings.{Environment}.json"
                : "appsettings.json");
            
            var configuration = configBuilder.Build();

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return configuration["AllowedOrigins"]?.Split(';');
        }
    }
}