using EfCoreDemoWebApi.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.EntityFrameworkCore;
using System.Web.Http.Dependencies;
using System.Web.Http.Controllers;

namespace EfCoreDemoWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var services = new ServiceCollection();

            ConfigureServices(services);

            config.DependencyResolver = new DefaultDependencyResolver(services.BuildServiceProvider());

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
            services
                .AddControllersAsServices()
                .AddDbContext<BloggingContext>(options => options.UseInMemoryDatabase("BloggingContext"));
        }
    }

    public class DefaultDependencyResolver : IDependencyResolver
    {
        protected IServiceProvider ServiceProvider { get; set; }

        public DefaultDependencyResolver(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return ServiceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return ServiceProvider.GetServices(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return new DefaultDependencyResolver(ServiceProvider.CreateScope().ServiceProvider);
        }

        public void Dispose()
        {
            // you can implement this interface just when you use .net core 2.0
            // ServiceProvider.Dispose();
        }
    }

    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddControllersAsServices(this IServiceCollection services)
        {
            var controllerTypes = typeof(WebApiConfig)
                .Assembly
                .GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(IHttpController).IsAssignableFrom(t)
                            || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase));

            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            return services;
        }
    }
}
