using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Auths.Commands.Logout
{
    public static class LogoutErrors
    {
        public static readonly Error UserIdRequired = new(ErrorType.Validation, "User ID is required for logout");
        public static readonly Error RefreshTokenNotFound = new(ErrorType.NotFound, "Refresh token not found");
        public static readonly Error RefreshTokenNotValid = new(ErrorType.Validation, "Refresh token is expired or revoked");
        public static readonly Error TokenMismatch = new(ErrorType.Forbidden, "Refresh token does not belong to the authenticated user");
        public static readonly Error BlacklistFailed = new(ErrorType.InternalServerError, "Failed to blacklist JWT token");
        public static readonly Error RevokeFailed = new(ErrorType.InternalServerError, "Failed to revoke refresh token");
    }
}
