using Microsoft.EntityFrameworkCore;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Domain.Model.User;
using VietDonate.Infrastructure.Common.Persistance;

namespace VietDonate.Infrastructure.Repositories
{
    public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
    {
        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await context.RefreshTokens
                .Include(rt => rt.UserIdentity)
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId)
        {
            return await context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ToListAsync();
        }

        public async Task<(RefreshToken Token, bool Success)> AddAsync(RefreshToken refreshToken)
        {
            try
            {
                context.RefreshTokens.Add(refreshToken);
                await context.SaveChangesAsync();
                return (refreshToken, true);
            }
            catch
            {
                return (refreshToken, false);
            }
        }

        public async Task<bool> UpdateAsync(RefreshToken refreshToken)
        {
            try
            {
                context.RefreshTokens.Update(refreshToken);
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var refreshToken = await context.RefreshTokens.FindAsync(id);
                if (refreshToken != null)
                {
                    context.RefreshTokens.Remove(refreshToken);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteExpiredTokensAsync()
        {
            try
            {
                var expiredTokens = await context.RefreshTokens
                    .Where(rt => rt.IsExpired)
                    .ToListAsync();

                if (expiredTokens.Any())
                {
                    context.RefreshTokens.RemoveRange(expiredTokens);
                    await context.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RevokeAllUserTokensAsync(Guid userId)
        {
            try
            {
                var userTokens = await context.RefreshTokens
                    .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                    .ToListAsync();

                foreach (var token in userTokens)
                {
                    token.Revoke();
                }

                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            try
            {
                var refreshToken = await context.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.Token == token);

                if (refreshToken != null)
                {
                    refreshToken.Revoke();
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}

