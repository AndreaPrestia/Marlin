using Marlin.Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Marlin.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Extension that add cors, cookie policy options
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddMarlin(this IServiceCollection services)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configBuilder = new ConfigurationBuilder().AddJsonFile(!string.IsNullOrEmpty(environment) ? $"appsettings.{environment}.json" : "appsettings.json");
            var configuration = configBuilder.Build();

            var marlinConfiguration = configuration.GetSection("Marlin").Get<MarlinConfiguration>();

            if (marlinConfiguration == null || marlinConfiguration.JwtConfiguration == null)
            {
                throw new ArgumentNullException(nameof(marlinConfiguration));
            }

            services.AddSingleton(marlinConfiguration);

            services.AddCors();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.None;
                options.HttpOnly = HttpOnlyPolicy.None;
            });
        }

        /// <summary>
        /// Extension that use cors with origins from app.settings.json, use cookie policy and adds the middleware
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void UseMarlin(this IApplicationBuilder builder)
        {
            var configuration = builder.ApplicationServices.GetService<MarlinConfiguration>();

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var origins = configuration.CorsOrigins?.Split(';');

            builder.UseCors(corsPolicyBuilder => corsPolicyBuilder
                .WithOrigins(origins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            builder.UseCookiePolicy();

            builder.UseMiddleware<MarlinMiddleware>();
        }
    }
}
