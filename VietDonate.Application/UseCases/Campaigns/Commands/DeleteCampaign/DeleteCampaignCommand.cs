using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Commands.DeleteCampaign
{
    public record DeleteCampaignCommand(Guid CampaignId) : ICommand<Result<DeleteCampaignResult>>;
}
