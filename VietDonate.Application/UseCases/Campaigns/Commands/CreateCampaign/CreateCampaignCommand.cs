using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Commands.CreateCampaign
{
    public record CreateCampaignCommand(
        string Name,
        string ShortDescription,
        string? FullStory,
        decimal? TargetAmount,
        string Type,
        string UrgencyLevel,
        bool AllowComment,
        bool AllowDonate,
        string? TargetItems,
        DateTime? StartTime,
        DateTime? EndTime
    ) : ICommand<Result<CreateCampaignResult>>;
}
