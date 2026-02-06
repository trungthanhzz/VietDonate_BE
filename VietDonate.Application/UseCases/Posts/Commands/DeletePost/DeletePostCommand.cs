using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Posts.Commands.DeletePost
{
    public record DeletePostCommand(Guid PostId) : ICommand<Result<DeletePostResult>>;
}
