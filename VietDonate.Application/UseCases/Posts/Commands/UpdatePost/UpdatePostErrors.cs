using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Posts.Commands.UpdatePost
{
    public static class UpdatePostErrors
    {
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to update this post");
        public static readonly Error PostNotFound = new(ErrorType.NotFound, "Post not found");
        public static readonly Error Forbidden = new(ErrorType.Forbidden, "You do not have permission to update this post");
        public static readonly Error InvalidPostType = new(ErrorType.Validation, "Invalid post type");
    }
}
