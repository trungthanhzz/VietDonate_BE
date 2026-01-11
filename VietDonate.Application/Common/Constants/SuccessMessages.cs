namespace VietDonate.Application.Common.Constants
{
    public static class SuccessMessages
    {
        public static class Auth
        {
            public const string LogoutSuccessful = "Logout successful";
            public const string UserContextValid = "User context valid";
            public const string RefreshTokenValid = "Refresh token valid";
            public const string JwtTokenBlacklisted = "JWT token blacklisted";
            public const string RefreshTokenRevoked = "Refresh token revoked";
            public const string NoJwtToBlacklist = "No JWT to blacklist";
            public const string LoginSuccessful = "Login successful";
            public const string RefreshTokenSuccessful = "Token refreshed successfully";
            public const string PasswordChangeSuccessful = "Password changed successfully";
        }

        public static class User
        {
            public const string RegistrationSuccessful = "User registered successfully";
            public const string UserCreated = "User created successfully";
            public const string UpdateSuccessful = "User updated successfully";
            public const string RoleUpdateSuccessful = "User role updated successfully";
        }

        public static class Campaign
        {
            public const string CreatedSuccessfully = "Campaign created successfully";
            public const string UpdatedSuccessfully = "Campaign updated successfully";
            public const string DeletedSuccessfully = "Campaign deleted successfully";
        }
    }
}
