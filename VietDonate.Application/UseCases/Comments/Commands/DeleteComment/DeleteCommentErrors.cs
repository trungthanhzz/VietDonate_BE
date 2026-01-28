using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Comments.Commands.DeleteComment
{
    public static class DeleteCommentErrors
    {
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to delete this comment");
        public static readonly Error Forbidden = new(ErrorType.Forbidden, "You are not allowed to delete this comment");
        public static readonly Error CommentNotFound = new(ErrorType.NotFound, "Comment not found");
        public static readonly Error PostNotFound = new(ErrorType.NotFound, "Post not found");
    }
}

