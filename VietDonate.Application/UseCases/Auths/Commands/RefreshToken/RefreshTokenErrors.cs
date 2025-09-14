using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Auths.Commands.RefreshToken
{
    public static class RefreshTokenErrors
    {
        public static readonly Error RefreshTokenNotFound = new(ErrorType.NotFound, "Invalid refresh token");
        public static readonly Error RefreshTokenRevoked = new(ErrorType.Validation, "Refresh token has been revoked");
        public static readonly Error RefreshTokenExpired = new(ErrorType.Validation, "Refresh token has expired");
        public static readonly Error UserNotFound = new(ErrorType.NotFound, "User not found");
        public static readonly Error UserInactive = new(ErrorType.Validation, "User account is deactivated");
        public static readonly Error RevokeFailed = new(ErrorType.InternalServerError, "Failed to revoke old refresh token");
        public static readonly Error CreateTokenFailed = new(ErrorType.InternalServerError, "Failed to create new refresh token");
    }
}
