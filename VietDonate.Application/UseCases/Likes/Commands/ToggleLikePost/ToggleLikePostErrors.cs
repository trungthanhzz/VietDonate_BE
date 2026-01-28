using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Likes.Commands.ToggleLikePost
{
    public static class ToggleLikePostErrors
    {
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to like this post");
        public static readonly Error PostNotFound = new(ErrorType.NotFound, "Post not found");
    }
}

