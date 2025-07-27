using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietDonate.Infrastructure.Common.Mediator;
using VietDonate.Application.Common.Mediator;
using VietDonate.Infrastructure.Common.Persistance;
using Microsoft.EntityFrameworkCore;
using VietDonate.Infrastructure.Security.TokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Infrastructure.Security.TokenGenerator;
using VietDonate.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using VietDonate.Infrastructure.Identity;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Infrastructure.Repositories;

namespace VietDonate.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfigurations(configuration);

            services
                .AddBackgroundServices(configuration)
                .AddAuthentication(configuration)
                .AddAuthorization()
                .AddMediator()
                .AddPersistence(configuration)
                .AddRepositories()
                .AddIdentityServices();

            return services;
        }

        private static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DbConfig>(configuration.GetSection("DBConfig"));
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
            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                var dbConfig = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<DbConfig>>().Value;
                options.UseNpgsql(dbConfig.BuildConnectionString());
            });

            return services;
        }

        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentityApiEndpoints<AppIdentityUser>()
                .AddEntityFrameworkStores<AppDbContext>();
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICampaignRepository, CampaignRepository>();
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
