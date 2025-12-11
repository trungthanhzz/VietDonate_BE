namespace VietDonate.Application.Common.Interfaces
{
    public interface IRequestContextService
    {
        string? UserId { get; }
        string? Jti { get; }
        List<string> Roles { get; }
        List<string> Permissions { get; }
        bool IsAuthenticated { get; }
        string? IpAddress { get; }
        string? UserAgent { get; }
        string? RequestId { get; }
        TimeSpan? GetJwtTtl();
        int? GetJwtTtlMinutes();
        int? GetJwtTtlSeconds();
        bool HasRole(string role);
        bool HasPermission(string permission);
        bool HasAnyRole(params string[] roles);
        bool HasAnyPermission(params string[] permissions);
        Guid? GetUserIdAsGuid();
    }
}
