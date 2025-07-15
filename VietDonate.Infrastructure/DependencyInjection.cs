using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.Common.Mediator;
using VietDonate.Application.Common.Mediator;
using VietDonate.Infrastructure.Common.Persistance;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Infrastructure.Security.TokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Infrastructure.Security.TokenGenerator;

namespace VietDonate.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddBackgroundServices(configuration)
                .AddAuthentication(configuration)
                .AddAuthorization()
                .AddPersistence(configuration)
                .AddMediator();
            return services;
        }

        private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            services
                .ConfigureOptions<JwtBearerTokenValidationConfiguration>()
                .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            return services;
        }

        private static IServiceCollection AddAuthorization(this IServiceCollection services)
        {
            return services;
        }

        private static IServiceCollection AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("123")));

            // DI Repositories

            return services;
        }

        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddSingleton<IMediator, SimpleMediator>();
            services.AddSingleton<ISender>(sp => sp.GetRequiredService<IMediator>());
            services.AddSingleton<IPublisher>(sp => sp.GetRequiredService<IMediator>());
            return services;
        }

    }
}
