using System.Security.Claims;

namespace VietDonate.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(
        Guid id,
        string firstName,
        string lastName,
        string email,
        List<string> permissions,
        string roles);
    
    ClaimsPrincipal GetPrincipalFromToken(string token);
}