using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Users.Queries.GetUserProfile
{
    public static class GetUserProfileErrors
    {
        public static readonly Error UserNotFound = new(ErrorType.NotFound, "User not found");
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "User is not authenticated");
    }
}

