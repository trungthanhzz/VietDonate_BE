using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Commands.UpdateCampaign
{
    public record UpdateCampaignCommand(
        Guid CampaignId,
        string? Name,
        string? ShortDescription,
        string? FullStory,
        decimal? TargetAmount,
        string? Type,
        string? UrgencyLevel,
        string? Status,
        bool? AllowComment,
        bool? AllowDonate,
        string? TargetItems,
        DateTime? StartTime,
        DateTime? EndTime
    ) : ICommand<Result<UpdateCampaignResult>>;
}
