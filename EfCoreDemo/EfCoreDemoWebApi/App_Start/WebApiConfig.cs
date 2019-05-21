using EfCoreDemoWebApi.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemoWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BloggingContext>(options => options.UseInMemoryDatabase("BloggingContext"));
        }
    }
}
