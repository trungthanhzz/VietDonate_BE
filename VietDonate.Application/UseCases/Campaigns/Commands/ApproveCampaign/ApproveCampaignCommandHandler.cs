using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Application.Common.Handlers;
using VietDonate.Domain.Common;
using VietDonate.Domain.Model.Campaigns;

namespace VietDonate.Application.UseCases.Campaigns.Commands.ApproveCampaign
{
    public class ApproveCampaignCommandHandler(
        ICampaignRepository campaignRepository,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<ApproveCampaignCommand, Result<ApproveCampaignResult>>
    {
        public async Task<Result<ApproveCampaignResult>> Handle(
            ApproveCampaignCommand command,
            CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<ApproveCampaignResult>.ValidationFailure(ApproveCampaignErrors.Unauthorized);
            }

            // Validate status
            var normalizedStatus = command.Status.Trim();
            if (!normalizedStatus.Equals("Approved", StringComparison.OrdinalIgnoreCase) &&
                !normalizedStatus.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
            {
                return Result<ApproveCampaignResult>.ValidationFailure(ApproveCampaignErrors.InvalidStatus);
            }

            // Rejection reason is required when rejecting
            if (normalizedStatus.Equals("Rejected", StringComparison.OrdinalIgnoreCase) &&
                string.IsNullOrWhiteSpace(command.RejectionReason))
            {
                return Result<ApproveCampaignResult>.ValidationFailure(ApproveCampaignErrors.RejectionReasonRequired);
            }

            var campaign = await campaignRepository.GetByIdAsync(command.CampaignId, cancellationToken);
            if (campaign == null)
            {
                return Result<ApproveCampaignResult>.ValidationFailure(ApproveCampaignErrors.CampaignNotFound);
            }

            // Check if status is already the same
            if (campaign.Status.Equals(normalizedStatus, StringComparison.OrdinalIgnoreCase))
            {
                return Result<ApproveCampaignResult>.ValidationFailure(ApproveCampaignErrors.StatusNotChanged);
            }

            // Check if campaign status is valid for update (should be Pending)
            if (!campaign.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                return Result<ApproveCampaignResult>.ValidationFailure(ApproveCampaignErrors.InvalidCurrentStatus);
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                campaign.Status = normalizedStatus;
                campaign.UpdateTime = DateTime.UtcNow;

                if (normalizedStatus.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                {
                    campaign.ApprovedTime = DateTime.UtcNow;
                    campaign.ApprovedId = userId.Value;
                    campaign.RejectionReason = null; // Clear rejection reason if approving
                }
                else if (normalizedStatus.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                {
                    campaign.RejectionReason = command.RejectionReason;
                    campaign.ApprovedTime = null; // Clear approval time if rejecting
                    campaign.ApprovedId = null; // Clear approved ID if rejecting
                }

                await campaignRepository.UpdateAsync(campaign, cancellationToken);

                var message = normalizedStatus.Equals("Approved", StringComparison.OrdinalIgnoreCase)
                    ? SuccessMessages.Campaign.ApprovedSuccessfully
                    : SuccessMessages.Campaign.RejectedSuccessfully;

                return Result.Success(new ApproveCampaignResult(
                    CampaignId: campaign.Id,
                    Message: message
                ));
            });
        }
    }
}
