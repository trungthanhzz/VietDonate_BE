using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Commands.ApproveCampaign
{
    public record ApproveCampaignCommand(
        Guid CampaignId,
        string Status,
        string? RejectionReason = null) : ICommand<Result<ApproveCampaignResult>>;
}
