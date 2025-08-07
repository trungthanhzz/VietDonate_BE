using VietDonate.Domain.Model.User;

namespace VietDonate.Application.Common.Interfaces.IRepository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId);
        Task<(RefreshToken Token, bool Success)> AddAsync(RefreshToken refreshToken);
        Task<bool> UpdateAsync(RefreshToken refreshToken);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteExpiredTokensAsync();
        Task<bool> RevokeAllUserTokensAsync(Guid userId);
        Task<bool> RevokeTokenAsync(string token);
    }
}

