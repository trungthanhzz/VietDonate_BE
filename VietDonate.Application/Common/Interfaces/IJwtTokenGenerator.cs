namespace VietDonate.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(
        Guid id,
        Guid jti,
        List<string> permissions,
        List<string> roles);

    int GetAccessTokenExpirationInMinutes();
}