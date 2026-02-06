using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Posts.Commands.UpdatePost
{
    public record UpdatePostCommand(
        Guid PostId,
        string? Title,
        string? Content,
        string? PostType,
        Guid? CampaignId,
        string? Status,
        string? ProofType,
        DateTime? ProofDate
    ) : ICommand<Result<UpdatePostResult>>;
}
