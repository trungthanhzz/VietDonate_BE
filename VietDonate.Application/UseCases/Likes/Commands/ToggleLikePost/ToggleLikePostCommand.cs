using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Likes.Commands.ToggleLikePost
{
    public record ToggleLikePostCommand(Guid PostId) : ICommand<Result<ToggleLikePostResult>>;
}

