using Microsoft.EntityFrameworkCore;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Domain.Model.User;
using VietDonate.Infrastructure.Common.Persistance;

namespace VietDonate.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task<UserIdentity> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await context.UserIdentities
                .Include(u => u.UserInformation)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<UserIdentity?> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            return await context.UserIdentities
                .Include(u => u.UserInformation)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        }

        public async Task<UserIdentity?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await context.UserIdentities
                .Include(u => u.UserInformation)
                .FirstOrDefaultAsync(u => u.UserInformation.Email == email, cancellationToken);
        }

        public async Task<UserIdentity?> GetByPhoneAsync(string phone, CancellationToken cancellationToken)
        {
            return await context.UserIdentities
                .Include(u => u.UserInformation)
                .FirstOrDefaultAsync(u => u.UserInformation.Phone == phone, cancellationToken);
        }

        public async Task AddAsync(UserIdentity userIdentity, CancellationToken cancellationToken)
        {
            // Add both UserIdentity and UserInformation
            await context.UserIdentities.AddAsync(userIdentity, cancellationToken);
            await context.UserInformations.AddAsync(userIdentity.UserInformation, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(UserIdentity userIdentity, CancellationToken cancellationToken)
        {
            context.UserIdentities.Update(userIdentity);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken)
        {
            return await context.UserIdentities
                .AnyAsync(u => u.UserName == userName, cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
        {
            return await context.UserInformations
                .AnyAsync(u => u.Email == email && !string.IsNullOrEmpty(u.Email), cancellationToken);
        }

        public async Task<bool> PhoneExistsAsync(string phone, CancellationToken cancellationToken)
        {
            return await context.UserInformations
                .AnyAsync(u => u.Phone == phone && !string.IsNullOrEmpty(u.Phone), cancellationToken);
        }
    }
} 