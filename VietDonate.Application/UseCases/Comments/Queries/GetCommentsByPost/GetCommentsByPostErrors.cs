using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Comments.Queries.GetCommentsByPost
{
    public static class GetCommentsByPostErrors
    {
        public static readonly Error PostNotFound = new(ErrorType.NotFound, "Post not found");
    }
}

