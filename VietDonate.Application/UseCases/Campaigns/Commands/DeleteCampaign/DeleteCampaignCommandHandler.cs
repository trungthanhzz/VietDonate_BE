using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Application.Common.Handlers;

namespace VietDonate.Application.UseCases.Campaigns.Commands.DeleteCampaign
{
    public class DeleteCampaignCommandHandler(
        ICampaignRepository campaignRepository,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<DeleteCampaignCommand, Result<DeleteCampaignResult>>
    {
        public async Task<Result<DeleteCampaignResult>> Handle(
            DeleteCampaignCommand command,
            CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<DeleteCampaignResult>.ValidationFailure(DeleteCampaignErrors.Unauthorized);
            }

            var campaign = await campaignRepository.GetByIdAsync(command.CampaignId, cancellationToken);
            if (campaign == null)
            {
                return Result<DeleteCampaignResult>.ValidationFailure(DeleteCampaignErrors.CampaignNotFound);
            }

            if (campaign.OwnerId != userId.Value)
            {
                return Result<DeleteCampaignResult>.ValidationFailure(DeleteCampaignErrors.Forbidden);
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                await campaignRepository.RemoveAsync(campaign, cancellationToken);

                return Result.Success(new DeleteCampaignResult(
                    CampaignId: command.CampaignId,
                    Message: SuccessMessages.Campaign.DeletedSuccessfully
                ));
            });
        }
    }
}
