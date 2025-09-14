using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Auths.Queries.Login
{
    public static class LoginErrors
    {
        public static readonly Error UserNotFound = new(ErrorType.NotFound, "User not found");
        public static readonly Error UserInactive = new(ErrorType.Validation, "User account is deactivated");
        public static readonly Error InvalidPassword = new(ErrorType.Validation, "Invalid password");
        public static readonly Error CreateRefreshTokenFailed = new(ErrorType.InternalServerError, "Failed to create refresh token");
    }
}
