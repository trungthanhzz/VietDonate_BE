namespace VietDonate.Application.Common.Interfaces
{
    public interface IRefreshTokenCacheService
    {
        Task<string?> GetRefreshTokenAsync(string userId);
        Task<bool> SetRefreshTokenAsync(string userId, string refreshToken, TimeSpan expiry);
        Task<bool> RemoveRefreshTokenAsync(string userId);
        Task<bool> IsRefreshTokenValidAsync(string userId, string refreshToken);
        Task<bool> RevokeAllUserTokensAsync(string userId);
    }
}

