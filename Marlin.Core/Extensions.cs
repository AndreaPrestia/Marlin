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

            var origins = configuration["Marlin:CorsOrigins"]?.Split(';');

            builder.UseCors(builder => builder
                .WithOrigins(origins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            builder.UseCookiePolicy();

            builder.MapWhen(context => context.Request.Path.ToString().StartsWith("/"), appBranch =>
            {
                appBranch.UseMiddleware<MarlinMiddleware>();
            });
        }

        /// <summary>
        /// Extension that adds IEventHandler as singleton
        /// </summary>
        /// <param name="services"></param>
        public static void AddEventHandler(this IServiceCollection services)
        {
            services.AddSingleton<IEventHandler>();
        }
    }
}
