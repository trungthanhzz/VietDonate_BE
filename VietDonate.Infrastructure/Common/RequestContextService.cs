using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using VietDonate.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace VietDonate.Infrastructure.Common
{
    public class RequestContextService(IHttpContextAccessor httpContextAccessor) : IRequestContextService
    {
        public string? UserId => httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
        
        public string? Jti => httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        
        public List<string> Roles => httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList() ?? new List<string>();
        
        public List<string> Permissions => httpContextAccessor.HttpContext?.User?.FindAll("permissions")
            .Select(c => c.Value)
            .ToList() ?? new List<string>();
        
        public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        
        public string? IpAddress => httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        
        public string? UserAgent => httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();
        
        public string? RequestId => httpContextAccessor.HttpContext?.TraceIdentifier;

        private DateTime? JwtExpiresAt
        {
            get
            {
                var expClaim = httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
                if (long.TryParse(expClaim, out var exp))
                {
                    return DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
                }
                return null;
            }
        }

        public TimeSpan? GetJwtTtl()
        {
            var expiresAt = JwtExpiresAt;
            if (expiresAt.HasValue)
            {
                var ttl = expiresAt.Value - DateTime.UtcNow;
                return ttl > TimeSpan.Zero ? ttl : TimeSpan.Zero;
            }
            return null;
        }

        public int? GetJwtTtlMinutes()
        {
            var ttl = GetJwtTtl();
            return ttl?.TotalMinutes > 0 ? (int)ttl.Value.TotalMinutes : 0;
        }

        public int? GetJwtTtlSeconds()
        {
            var ttl = GetJwtTtl();
            return ttl?.TotalSeconds > 0 ? (int)ttl.Value.TotalSeconds : 0;
        }

        public bool HasRole(string role)
        {
            return Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
        }

        public bool HasPermission(string permission)
        {
            return Permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
        }

        public bool HasAnyRole(params string[] roles)
        {
            return roles.Any(role => HasRole(role));
        }

        public bool HasAnyPermission(params string[] permissions)
        {
            return permissions.Any(permission => HasPermission(permission));
        }

        public Guid? GetUserIdAsGuid()
        {
            if (string.IsNullOrEmpty(UserId) || !Guid.TryParse(UserId, out var userId))
                return null;
            
            return userId;
        }
    }
}
