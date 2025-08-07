using VietDonate.Application.Common.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace VietDonate.Infrastructure.Security.PasswordHasher
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BC.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BC.Verify(password, hashedPassword);
        }
    }
} 