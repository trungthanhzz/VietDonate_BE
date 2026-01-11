using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Users.Commands.UpdateUserRole
{
    public static class UpdateUserRoleErrors
    {
        public static readonly Error UserNotFound = new(ErrorType.NotFound, "User not found");
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "Unauthorized. Only admin can update user roles");
        public static readonly Error InvalidRole = new(ErrorType.Validation, "Invalid role. Cannot change to Guest role or Admin role");
        public static readonly Error SameRole = new(ErrorType.Validation, "User already has this role");
        public static readonly Error CannotChangeOwnRole = new(ErrorType.Validation, "Cannot change your own role");
    }
}
