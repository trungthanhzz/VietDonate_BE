using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Users.Commands.Register
{
    public static class RegisterUserErrors
    {
        public static readonly Error ContactMethodRequired = new(ErrorType.Validation, "At least one contact method (phone or email) is required");
        public static readonly Error UsernameExists = new(ErrorType.Conflict, "Username already exists");
        public static readonly Error EmailExists = new(ErrorType.Conflict, "Email already exists");
        public static readonly Error PhoneExists = new(ErrorType.Conflict, "Phone number already exists");
    }
}
