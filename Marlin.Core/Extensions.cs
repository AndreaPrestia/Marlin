using System;
using Marlin.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marlin.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Extension that add cors, cookie policy options
        /// </summary>
        /// <param name="services"></param>
        public static void AddMarlin(this IServiceCollection services)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configBuilder = new ConfigurationBuilder().AddJsonFile(!string.IsNullOrEmpty(environment) ? $"appsettings.{environment}.json" : "appsettings.json");
            var configuration = configBuilder.Build();

            services.AddSingleton<IConfiguration>(configuration);

            var isEventLoggerEnabled = bool.TryParse(configuration["Marlin:EventLoggerEnabled"], out var eventLoggerEnabled);

            if (isEventLoggerEnabled && eventLoggerEnabled)
            {
                services.AddSingleton<IEventHandler>();
            }

            services.AddScoped<TokenManager>();

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
        public static void UseMarlin(this IApplicationBuilder builder)
        {
            var configuration = builder.ApplicationServices.GetService<IConfiguration>();

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var origins = configuration["Marlin:CorsOrigins"]?.Split(';');

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
