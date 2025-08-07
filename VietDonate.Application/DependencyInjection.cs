using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.SwaggerExtension;

namespace VietDonate.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                //.AddSwaggerExtension()
                .RegisterSimpleMediatorHandlersFromAssembly(typeof(DependencyInjection).Assembly);
            return services;
        }

        private static IServiceCollection RegisterSimpleMediatorHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            RegisterHandler(services, assembly, typeof(ICommandHandler<,>));
            RegisterHandler(services, assembly, typeof(IQueryHandler<,>));
            RegisterHandler(services, assembly, typeof(INotificationHandler<>));
            return services;
        }

        private static void RegisterHandler(IServiceCollection services, Assembly assembly, Type handlerOpenGeneric)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic);

            foreach (var handlerType in handlerTypes)
            {
                var interfaces = handlerType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerOpenGeneric);

                foreach (var handlerInterface in interfaces)
                {
                    Console.WriteLine($"Registering handler: {handlerType.Name} -> {handlerInterface.Name}");
                    services.AddScoped(handlerInterface, handlerType);
                }
            }
        }

        private static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new QueryStringApiVersionReader("v");
            });
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerGenConfigurationOptions>();
            services.AddSwaggerGen();
            return services;
        }
    }
}
