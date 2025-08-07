using VietDonate.Domain.Common;

namespace VietDonate.Domain.Model.User
{
    public class UserInformation(
        Guid id,
        string fullName,
        string phone,
        string email,
        string address,
        string avtUrl)
        : Entity(id)
    {
        public string FullName { get; } = fullName;
        public string Phone { get; } = phone;
        public string Email { get; } = email;
        public string Address { get; } = address;
        public string AvtUrl { get; } = avtUrl;
        public DateTime CreateTime { get; } = DateTime.UtcNow;

        public UserIdentity UserIdentity { get; set; }
    }
}
