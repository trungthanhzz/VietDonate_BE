using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Users.Commands.UpdateUserPartial
{
    public static class UpdateUserPartialErrors
    {
        public static readonly Error UserNotFound = new(ErrorType.NotFound, "User not found");
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "Unauthorized");
        public static readonly Error ContactMethodRequired = new(ErrorType.Validation, "At least one contact method (phone or email) is required");
        public static readonly Error EmailExists = new(ErrorType.Conflict, "Email already exists");
        public static readonly Error PhoneExists = new(ErrorType.Conflict, "Phone number already exists");
        public static readonly Error NoFieldsToUpdate = new(ErrorType.Validation, "No fields provided to update");
    }
}

