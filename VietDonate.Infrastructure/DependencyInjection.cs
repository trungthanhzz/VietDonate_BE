using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VietDonate.Infrastructure.Common.Mediator;
using VietDonate.Application.Common.Mediator;
using VietDonate.Infrastructure.Common.Persistance;
using Microsoft.EntityFrameworkCore;
using VietDonate.Infrastructure.Security.TokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Infrastructure.Security.TokenGenerator;
using VietDonate.Infrastructure.Configurations;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Infrastructure.Repositories;
using VietDonate.Infrastructure.Common.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using VietDonate.Infrastructure.Common.Middleware;

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
                .AddRedis(configuration)
                .AddS3FileStorage(configuration)
                .AddJwtBlacklist(options =>
                {
                    options.EnableBlacklistCheck = true;
                    options.ExcludedPaths = new[] { "/swagger", "/health", "/api/auth/login", "/api/auth/register" };
                    options.ExcludedMethods = new[] { "OPTIONS" };
                    options.LogBlockedRequests = true;
                    options.BlacklistKeyPrefix = "bl:acc:";
                });

            return services;
        }

        private static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));
            services.Configure<RedisConfig>(configuration.GetSection(nameof(RedisConfig)));
            return services;
        }

        private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            
            services.AddHttpContextAccessor();

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

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IPasswordHasher, VietDonate.Infrastructure.Security.PasswordHasher.PasswordHasher>();
            services.AddTransient<VietDonate.Application.Common.Interfaces.IRequestContextService, VietDonate.Infrastructure.Common.RequestContextService>();
            
            return services;
        }

        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddScoped<IMediator, SimpleMediator>();
            services.AddScoped<ISender>(sp => sp.GetRequiredService<IMediator>());
            services.AddScoped<IPublisher>(sp => sp.GetRequiredService<IMediator>());
            return services;
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisConfig>(configuration.GetSection(nameof(RedisConfig)));
            
            var redisConfig = new RedisConfig();
            configuration.GetSection(nameof(RedisConfig)).Bind(redisConfig);
            
            if (!string.IsNullOrEmpty(redisConfig.ConnectionString))
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConfig.ConnectionString;
                    options.InstanceName = redisConfig.InstanceName;
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }
            
            services.AddSingleton<IRedisService, VietDonate.Infrastructure.Common.Redis.RedisService>();
            services.AddScoped<IRefreshTokenCacheService, VietDonate.Infrastructure.Common.Redis.RefreshTokenCacheService>();
            return services;
        }
        
        public static IServiceCollection AddS3FileStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<S3Config>(configuration.GetSection(nameof(S3Config)));
            services.AddAWSService<Amazon.S3.IAmazonS3>();
            services.AddScoped<VietDonate.Application.Common.Interfaces.IService.IFileStorageService, Common.S3.S3FileStorageService>();
            return services;
        }
    }
}
