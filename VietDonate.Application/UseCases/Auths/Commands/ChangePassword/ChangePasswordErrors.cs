using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Auths.Commands.ChangePassword
{
    public static class ChangePasswordErrors
    {
        public static readonly Error UserIdRequired = new(ErrorType.Validation, "User ID is required for password change");
        public static readonly Error UserNotFound = new(ErrorType.NotFound, "User not found");
        public static readonly Error InvalidOldPassword = new(ErrorType.Validation, "Old password is incorrect");
        public static readonly Error PasswordUpdateFailed = new(ErrorType.InternalServerError, "Failed to update password");
        public static readonly Error NewPasswordRequired = new(ErrorType.Validation, "New password is required");
        public static readonly Error OldPasswordRequired = new(ErrorType.Validation, "Old password is required");
        public static readonly Error SamePassword = new(ErrorType.Validation, "New password must be different from old password");
    }
}
