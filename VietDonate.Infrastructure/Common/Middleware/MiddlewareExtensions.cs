using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace VietDonate.Infrastructure.Common.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddJwtBlacklist(this IServiceCollection services, Action<JwtBlacklistOptions>? configureOptions = null)
        {
            var options = new JwtBlacklistOptions();
            configureOptions?.Invoke(options);
            
            services.Configure<JwtBlacklistOptions>(opt =>
            {
                opt.EnableBlacklistCheck = options.EnableBlacklistCheck;
                opt.ExcludedPaths = options.ExcludedPaths;
                opt.ExcludedMethods = options.ExcludedMethods;
                opt.LogBlockedRequests = options.LogBlockedRequests;
                opt.BlacklistKeyPrefix = options.BlacklistKeyPrefix;
            });

            return services;
        }

        public static IApplicationBuilder UseJwtBlacklist(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JwtBlacklistMiddleware>();
        }
    }
}
