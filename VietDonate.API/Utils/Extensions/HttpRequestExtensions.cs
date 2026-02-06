using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace VietDonate.API.Utils.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string? GetUserId(this HttpRequest request)
        {
            return request.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
        }

        public static Guid? GetUserIdAsGuid(this HttpRequest request)
        {
            var userId = request.GetUserId();
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var id))
            {
                return null;
            }

            return id;
        }

        public static string? GetJti(this HttpRequest request)
        {
            return request.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        }

        public static List<string> GetRoles(this HttpRequest request)
        {
            var user = request.HttpContext?.User;
            return user?.FindAll(ClaimTypes.Role)
                       .Select(c => c.Value)
                       .ToList() ?? new List<string>();
        }

        public static List<string> GetPermissions(this HttpRequest request)
        {
            var user = request.HttpContext?.User;
            return user?.FindAll("permissions")
                       .Select(c => c.Value)
                       .ToList() ?? new List<string>();
        }

        public static bool IsAuthenticated(this HttpRequest request)
        {
            return request.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }

        public static string? GetIpAddress(this HttpRequest request)
        {
            return request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }

        public static string? GetUserAgent(this HttpRequest request)
        {
            return request.Headers["User-Agent"].ToString();
        }

        public static string? GetRequestId(this HttpRequest request)
        {
            return request.HttpContext?.TraceIdentifier;
        }

        private static DateTime? GetJwtExpiresAt(this HttpRequest request)
        {
            var expClaim = request.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
            if (long.TryParse(expClaim, out var exp))
            {
                return DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
            }

            return null;
        }

        public static TimeSpan? GetJwtTtl(this HttpRequest request)
        {
            var expiresAt = request.GetJwtExpiresAt();
            if (expiresAt.HasValue)
            {
                var ttl = expiresAt.Value - DateTime.UtcNow;
                return ttl > TimeSpan.Zero ? ttl : TimeSpan.Zero;
            }

            return null;
        }

        public static int? GetJwtTtlMinutes(this HttpRequest request)
        {
            var ttl = request.GetJwtTtl();
            return ttl?.TotalMinutes > 0 ? (int)ttl.Value.TotalMinutes : 0;
        }

        public static int? GetJwtTtlSeconds(this HttpRequest request)
        {
            var ttl = request.GetJwtTtl();
            return ttl?.TotalSeconds > 0 ? (int)ttl.Value.TotalSeconds : 0;
        }

        public static bool HasRole(this HttpRequest request, string role)
        {
            return request.GetRoles().Contains(role, StringComparer.OrdinalIgnoreCase);
        }

        public static bool HasPermission(this HttpRequest request, string permission)
        {
            return request.GetPermissions().Contains(permission, StringComparer.OrdinalIgnoreCase);
        }

        public static bool HasAnyRole(this HttpRequest request, params string[] roles)
        {
            return roles.Any(role => request.HasRole(role));
        }

        public static bool HasAnyPermission(this HttpRequest request, params string[] permissions)
        {
            return permissions.Any(permission => request.HasPermission(permission));
        }
    }
}
