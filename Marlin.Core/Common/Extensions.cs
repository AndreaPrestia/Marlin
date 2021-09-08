using Marlin.Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Marlin.Core.Common
{
    public static class Extensions
    {
        public static T Get<T>(this DbDataReader reader, string name)
        {
            object o = reader[name];

            if (o == null || o == DBNull.Value)
            {
                return default(T);
            }

            return (T)o;
        }

        /// <summary>
        /// Extension that add cors, cookie policy options and controllers
        /// </summary>
        /// <param name="services"></param>
        public static void AddMarlin(this IServiceCollection services, string apiVersion = null, string title = null, string description = null, string termsOfService = null, OpenApiContact contact = null, OpenApiLicense license = null)
        {
            services.AddCors();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.None;
                options.HttpOnly = HttpOnlyPolicy.None;
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(Settings.Get("SessionLifetimeInMinutes", 30));
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<AuthorizationHeaderParameter>();

                c.SwaggerDoc(!string.IsNullOrEmpty(apiVersion) ? apiVersion : "v1", new OpenApiInfo
                {
                    Version = !string.IsNullOrEmpty(apiVersion) ? apiVersion : "v1",
                    Title = !string.IsNullOrEmpty(title) ? title : "v1",
                    Description = !string.IsNullOrEmpty(description) ? description : "v1",
                    TermsOfService = !string.IsNullOrEmpty(termsOfService) ? new Uri(termsOfService) : new Uri("https://github.com/AndreaPrestia/marlin"),
                    Contact = contact == null ? new OpenApiContact
                    {
                        Name = "Andrea Prestia",
                        Email = "prestia.andrea94@gmail.com",
                        Url = new Uri("https://github.com/AndreaPrestia"),
                    } : contact,
                    License = license == null ? new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://opensource.org/licenses/CDDL-1.0"),
                    } : license
                });
            });
        }

        /// <summary>
        /// Extension that use cors with origins from configurations, use cookie policy and adds the exception handler
        /// </summary>
        /// <param name="builder"></param>
        public static void UseMarlin(this IApplicationBuilder builder, bool useHttpsRedirection = true, bool useStaticFiles = true, bool useRouting = true, bool useAuthorization = true, bool useSwagger = true, string swaggerUrl = null, string swaggerName = null, string swaggerRoutePrefix = null)
        {
            if (useHttpsRedirection)
                builder.UseHttpsRedirection();

            if (useStaticFiles)
                builder.UseStaticFiles();

            if (useRouting)
                builder.UseRouting();

            if (useAuthorization)
                builder.UseAuthorization();

            List<string> origins = Settings.GetList<string>("CorsOrigins");

            builder.UseCors(builder => builder
                .WithOrigins(origins.ToArray())
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            builder.UseSession(new SessionOptions() { Cookie = new CookieBuilder() { Name = "MarlinId" } });

            builder.UseCookiePolicy();

            builder.UseExceptionHandler("/error");

            if (useSwagger)
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                builder.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                builder.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(!string.IsNullOrEmpty(swaggerUrl) ? swaggerUrl : "/swagger/v1/swagger.json", !string.IsNullOrEmpty(swaggerName) ? swaggerName : "Marlin V1");
                    c.RoutePrefix = string.IsNullOrEmpty(swaggerRoutePrefix) ? string.Empty : swaggerRoutePrefix;
                });
            }

            string defaultControllerRoute = Settings.Get("DefaultControllerRoute", "{controller=Marlin}/{action=Index}/{id?}");

            builder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: defaultControllerRoute);
            });
        }
        public static List<Resource> BuildResourceTree(this List<Resource> source)
        {
            if (source == null || source.Count() == 0)
            {
                return new List<Resource>();
            }

            var resources = source.GroupBy(i => i.ParentId);

            List<Resource> roots = source.GroupBy(i => i.ParentId).FirstOrDefault(g => g.Key == Guid.Empty).ToList();

            if (roots.Count > 0)
            {
                var dict = resources.Where(g => g.Key != Guid.Empty).ToDictionary(g => g.Key, g => g.ToList());

                for (int i = 0; i < roots.Count; i++)
                {
                    AddChildren(roots[i], dict);
                }
            }

            return roots;
        }

        private static void AddChildren(Resource node, IDictionary<Guid, List<Resource>> source)
        {
            if (source.ContainsKey(node.Id))
            {
                node.Children = source[node.Id];

                for (int i = 0; i < node.Children.Count; i++)
                {
                    AddChildren(node.Children[i], source);
                }
            }
            else
            {
                node.Children = new List<Resource>();
            }
        }
    }
}
