using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Posts.Commands.CreatePost
{
    public record CreatePostCommand(
        string Title,
        string Content,
        string PostType,
        Guid? CampaignId,
        string? Status,
        string? ProofType,
        DateTime? ProofDate
    ) : ICommand<Result<CreatePostResult>>;
}
