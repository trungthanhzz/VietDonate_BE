using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Comments.Commands.DeleteComment
{
    public record DeleteCommentCommand(Guid CommentId) : ICommand<Result<DeleteCommentResult>>;
}

