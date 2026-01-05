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
            await context.UserIdentities.AddAsync(userIdentity, cancellationToken);
            await context.UserInformations.AddAsync(userIdentity.UserInformation, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(UserIdentity userIdentity, CancellationToken cancellationToken)
        {
            context.UserIdentities.Update(userIdentity);
            if (userIdentity.UserInformation != null)
            {
                var entry = context.Entry(userIdentity.UserInformation);
                entry.State = EntityState.Modified;
            }
            
            await context.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateUserInformationPropertiesAsync(
            UserInformation userInformation,
            string? fullName,
            string? phone,
            string? email,
            string? address,
            string? avtUrl,
            CancellationToken cancellationToken)
        {
            var entry = context.Entry(userInformation);
            entry.State = EntityState.Modified;

            // Update get-only properties from primary constructor using EF Core Entry API
            if (fullName != null)
                entry.Property(nameof(UserInformation.FullName)).CurrentValue = fullName;

            if (phone != null)
                entry.Property(nameof(UserInformation.Phone)).CurrentValue = phone;

            if (email != null)
                entry.Property(nameof(UserInformation.Email)).CurrentValue = email;

            if (address != null)
                entry.Property(nameof(UserInformation.Address)).CurrentValue = address;

            if (avtUrl != null)
                entry.Property(nameof(UserInformation.AvtUrl)).CurrentValue = avtUrl;

            // Don't save here - let UpdateAsync handle the save
            return Task.CompletedTask;
        }

        public async Task UpdatePasswordAsync(UserIdentity userIdentity, string newPasswordHash, CancellationToken cancellationToken)
        {
            var entry = context.Entry(userIdentity);
            entry.State = EntityState.Modified;
            
            // Update PasswordHash property using EF Core Entry API
            entry.Property(nameof(UserIdentity.PasswordHash)).CurrentValue = newPasswordHash;
            
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