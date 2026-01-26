using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Commands.AssignMediaToCampaign
{
    public record AssignMediaToCampaignCommand(
        Guid CampaignId,
        List<Guid> TempMediaIds,
        List<Guid>? ExistingMediaIds = null
    ) : ICommand<Result<AssignMediaToCampaignResult>>;
}
