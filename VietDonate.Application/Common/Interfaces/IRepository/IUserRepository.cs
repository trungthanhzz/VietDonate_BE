using VietDonate.Domain.Model.User;

namespace VietDonate.Application.Common.Interfaces.IRepository
{
    public interface IUserRepository
    {
        Task<UserIdentity> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<UserIdentity?> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
        Task<UserIdentity?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<UserIdentity?> GetByPhoneAsync(string phone, CancellationToken cancellationToken);
        Task AddAsync(UserIdentity userIdentity, CancellationToken cancellationToken);
        Task UpdateAsync(UserIdentity userIdentity, CancellationToken cancellationToken);
        Task UpdateUserInformationPropertiesAsync(
            UserInformation userInformation,
            string? fullName,
            string? phone,
            string? email,
            string? address,
            string? avtUrl,
            CancellationToken cancellationToken);
        Task UpdatePasswordAsync(UserIdentity userIdentity, string newPasswordHash, CancellationToken cancellationToken);
        Task<bool> UserNameExistsAsync(string userName, CancellationToken cancellationToken);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
        Task<bool> PhoneExistsAsync(string phone, CancellationToken cancellationToken);
    }
} 