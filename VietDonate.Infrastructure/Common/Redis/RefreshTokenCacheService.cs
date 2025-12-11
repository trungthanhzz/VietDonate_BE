using VietDonate.Application.Common.Interfaces;

namespace VietDonate.Infrastructure.Common.Redis
{
    public class RefreshTokenCacheService : IRefreshTokenCacheService
    {
        private readonly IRedisService _redisService;
        private const string RefreshTokenPrefix = "refresh_token";

        public RefreshTokenCacheService(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<string?> GetRefreshTokenAsync(string userId)
        {
            var key = $"{RefreshTokenPrefix}:{userId}";
            return await _redisService.GetAsync<string>(key);
        }

        public async Task<bool> SetRefreshTokenAsync(string userId, string refreshToken, TimeSpan expiry)
        {
            var key = $"{RefreshTokenPrefix}:{userId}";
            return await _redisService.SetAsync(key, refreshToken, expiry);
        }

        public async Task<bool> RemoveRefreshTokenAsync(string userId)
        {
            var key = $"{RefreshTokenPrefix}:{userId}";
            return await _redisService.RemoveAsync(key);
        }

        public async Task<bool> IsRefreshTokenValidAsync(string userId, string refreshToken)
        {
            var cachedToken = await GetRefreshTokenAsync(userId);
            return !string.IsNullOrEmpty(cachedToken) && cachedToken == refreshToken;
        }

        public async Task<bool> RevokeAllUserTokensAsync(string userId)
        {
            return await RemoveRefreshTokenAsync(userId);
        }
    }
}

