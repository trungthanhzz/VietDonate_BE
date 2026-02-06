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
using VietDonate.Infrastructure.Common.Middleware;
using VietDonate.Domain.Common;
using VietDonate.Application.Common.Constants;

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
            services.Configure<CookieConfig>(configuration.GetSection(CookieConfig.Section));
            services.Configure<S3Config>(configuration.GetSection(nameof(S3Config)));
            return services;
        }

        private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));
            services.Configure<CookieConfig>(configuration.GetSection(CookieConfig.Section));

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            
            services.AddHttpContextAccessor();

            var cookieConfig = configuration.GetSection(CookieConfig.Section).Get<CookieConfig>() ?? new CookieConfig();
            
            services
                .ConfigureOptions<JwtBearerTokenValidationConfiguration>()
                .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer()
                .AddCookie(options =>
                {
                    options.Cookie.Name = cookieConfig.AccessTokenCookieName;
                    options.Cookie.HttpOnly = cookieConfig.HttpOnly;
                    options.Cookie.SecurePolicy = cookieConfig.Secure 
                        ? Microsoft.AspNetCore.Http.CookieSecurePolicy.Always 
                        : Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
                    options.Cookie.SameSite = cookieConfig.SameSite switch
                    {
                        "None" => Microsoft.AspNetCore.Http.SameSiteMode.None,
                        "Lax" => Microsoft.AspNetCore.Http.SameSiteMode.Lax,
                        "Strict" => Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                        _ => Microsoft.AspNetCore.Http.SameSiteMode.None
                    };
                    options.Cookie.Path = cookieConfig.Path;
                    if (!string.IsNullOrEmpty(cookieConfig.Domain))
                    {
                        options.Cookie.Domain = cookieConfig.Domain;
                    }
                });

            return services;
        }

        private static IServiceCollection AddAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy(AuthorizationPolicies.RequireAdmin, policy => 
                    policy.RequireRole(nameof(RoleType.Admin)))
                .AddPolicy(AuthorizationPolicies.RequireStaff, policy => 
                    policy.RequireRole(nameof(RoleType.Admin), nameof(RoleType.Staff)))
                .AddPolicy(AuthorizationPolicies.RequireStaffOnly, policy => 
                    policy.RequireRole(nameof(RoleType.Staff)))
                .AddPolicy(AuthorizationPolicies.RequireUser, policy => 
                    policy.RequireRole(nameof(RoleType.Admin), nameof(RoleType.Staff), nameof(RoleType.User)));
            
            return services;
        }

        private static IServiceCollection AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Enable legacy timestamp behavior for Npgsql to handle DateTime with Kind=Unspecified
            // This allows Npgsql to accept DateTime values without explicit UTC kind
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            
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
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IPasswordHasher, VietDonate.Infrastructure.Security.PasswordHasher.PasswordHasher>();
            services.AddTransient<VietDonate.Application.Common.Interfaces.IRequestContextService, VietDonate.Infrastructure.Common.RequestContextService>();
            
            // Register Storage Services
            services.AddScoped<IStorageService, VietDonate.Infrastructure.Common.Storage.S3StorageService>();
            services.AddScoped<ITempMediaService, VietDonate.Infrastructure.Common.Storage.TempMediaService>();
            services.AddSingleton<IFileValidationSettings, VietDonate.Infrastructure.Common.Storage.FileValidationSettings>();
            
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
    }
}
