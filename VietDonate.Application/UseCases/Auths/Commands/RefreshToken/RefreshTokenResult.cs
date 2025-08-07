namespace VietDonate.Application.UseCases.Auths.Commands.RefreshToken
{
    public record RefreshTokenResult(
        string AccessToken,
        string RefreshToken,
        int ExpireDate);
} 
