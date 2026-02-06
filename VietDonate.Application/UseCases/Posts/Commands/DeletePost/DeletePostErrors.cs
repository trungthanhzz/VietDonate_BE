using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Posts.Commands.DeletePost
{
    public static class DeletePostErrors
    {
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to delete this post");
        public static readonly Error PostNotFound = new(ErrorType.NotFound, "Post not found");
        public static readonly Error Forbidden = new(ErrorType.Forbidden, "You do not have permission to delete this post");
    }
}
