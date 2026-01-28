using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Comments.Commands.CreateComment
{
    public static class CreateCommentErrors
    {
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to comment on this post");
        public static readonly Error PostNotFound = new(ErrorType.NotFound, "Post not found");
        public static readonly Error ContentRequired = new(ErrorType.Validation, "Comment content is required");
        public static readonly Error ParentCommentNotFound = new(ErrorType.NotFound, "Parent comment not found");
    }
}

