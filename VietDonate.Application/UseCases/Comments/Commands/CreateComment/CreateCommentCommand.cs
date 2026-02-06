using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Comments.Commands.CreateComment
{
    public record CreateCommentCommand(
        Guid PostId,
        string Content,
        Guid? ParentId
    ) : ICommand<Result<CreateCommentResult>>;
}

