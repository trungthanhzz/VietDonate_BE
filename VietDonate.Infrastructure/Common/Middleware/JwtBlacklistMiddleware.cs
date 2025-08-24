using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Domain.Common;

namespace VietDonate.Infrastructure.Common.Middleware
{
    public class JwtBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtBlacklistMiddleware> _logger;
        private readonly IRedisService _redisService;
        private readonly JwtBlacklistOptions _options;

        public JwtBlacklistMiddleware(
            RequestDelegate next,
            ILogger<JwtBlacklistMiddleware> logger,
            IRedisService redisService,
            IOptions<JwtBlacklistOptions> options)
        {
            _next = next;
            _logger = logger;
            _redisService = redisService;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (!_options.EnableBlacklistCheck)
                {
                    await _next(context);
                    return;
                }

                if (ShouldSkipBlacklistCheck(context))
                {
                    await _next(context);
                    return;
                }

                var token = ExtractTokenFromRequest(context);
                
                if (!string.IsNullOrEmpty(token))
                {
                    var isBlacklisted = await CheckIfTokenIsBlacklisted(token);
                    
                    if (isBlacklisted)
                    {
                        if (_options.LogBlockedRequests)
                        {
                            _logger.LogWarning("Request blocked: JWT token is blacklisted. Path: {Path}, Method: {Method}", 
                                context.Request.Path, context.Request.Method);
                        }
                        
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync("{\"error\":\"Token has been revoked\",\"code\":\"TOKEN_BLACKLISTED\"}");
                        return;
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in JWT blacklist middleware");
                await _next(context);
            }
        }

        private bool ShouldSkipBlacklistCheck(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant();
            var method = context.Request.Method.ToUpperInvariant();

            if (string.IsNullOrEmpty(path))
                return false;

            if (_options.ExcludedPaths.Any(excludedPath => 
                path.StartsWith(excludedPath.ToLowerInvariant(), StringComparison.OrdinalIgnoreCase)))
                return true;

            if (_options.ExcludedMethods.Any(excludedMethod => 
                method.Equals(excludedMethod, StringComparison.OrdinalIgnoreCase)))
                return true;

            return false;
        }

        private static string? ExtractTokenFromRequest(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return null;

            return authHeader.Substring("Bearer ".Length).Trim();
        }

        private async Task<bool> CheckIfTokenIsBlacklisted(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                
                if (!handler.CanReadToken(token))
                    return false;

                var jwtToken = handler.ReadJwtToken(token);
                var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
                
                if (string.IsNullOrEmpty(jti))
                    return false;

                var blacklistKey = ObjectExtentions.GetKeyBlackListRedis(jti);
                var blacklistedValue = await _redisService.GetAsync<string>(blacklistKey);
                
                return !string.IsNullOrEmpty(blacklistedValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking JWT blacklist for token");
                return false;
            }
        }
    }
}
