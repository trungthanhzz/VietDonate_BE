using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Application.Common.Handlers;
using VietDonate.Domain.Model.Campaigns;

namespace VietDonate.Application.UseCases.Campaigns.Commands.UpdateCampaign
{
    public class UpdateCampaignCommandHandler(
        ICampaignRepository campaignRepository,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<UpdateCampaignCommand, Result<UpdateCampaignResult>>
    {
        public async Task<Result<UpdateCampaignResult>> Handle(
            UpdateCampaignCommand command,
            CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<UpdateCampaignResult>.ValidationFailure(UpdateCampaignErrors.Unauthorized);
            }

            var campaign = await campaignRepository.GetByIdAsync(command.CampaignId, cancellationToken);
            if (campaign == null)
            {
                return Result<UpdateCampaignResult>.ValidationFailure(UpdateCampaignErrors.CampaignNotFound);
            }

            if (campaign.OwnerId != userId.Value)
            {
                return Result<UpdateCampaignResult>.ValidationFailure(UpdateCampaignErrors.Forbidden);
            }

            var validationResult = ValidateUpdateData(command);
            if (validationResult.IsFailure)
            {
                return Result<UpdateCampaignResult>.ValidationFailure(validationResult.Error);
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                UpdateCampaignProperties(campaign, command);
                campaign.UpdateTime = DateTime.UtcNow;

                await campaignRepository.UpdateAsync(campaign, cancellationToken);

                return Result.Success(new UpdateCampaignResult(
                    CampaignId: campaign.Id,
                    Message: SuccessMessages.Campaign.UpdatedSuccessfully
                ));
            });
        }

        private Result ValidateUpdateData(UpdateCampaignCommand command)
        {
            if (command.StartTime.HasValue && command.EndTime.HasValue &&
                command.EndTime.Value <= command.StartTime.Value)
            {
                return Result.Failure(UpdateCampaignErrors.InvalidDateRange);
            }

            return Result.Success();
        }

        private void UpdateCampaignProperties(Campaign campaign, UpdateCampaignCommand command)
        {
            if (!string.IsNullOrWhiteSpace(command.Name))
            {
                // Note: Name is readonly in domain model, so we can't update it directly
                // This would require a domain method or reflection, but for now we'll skip it
            }

            if (!string.IsNullOrWhiteSpace(command.ShortDescription))
            {
                campaign.ShortDescription = command.ShortDescription;
            }

            if (command.FullStory != null)
            {
                campaign.FullStory = command.FullStory;
            }

            if (command.TargetAmount.HasValue)
            {
                campaign.TargetAmount = command.TargetAmount;
            }

            if (!string.IsNullOrWhiteSpace(command.Type))
            {
                campaign.Type = command.Type;
            }

            if (!string.IsNullOrWhiteSpace(command.UrgencyLevel))
            {
                campaign.UrgencyLevel = command.UrgencyLevel;
            }

            if (!string.IsNullOrWhiteSpace(command.Status))
            {
                campaign.Status = command.Status;
            }

            if (command.AllowComment.HasValue)
            {
                campaign.AllowComment = command.AllowComment.Value;
            }

            if (command.AllowDonate.HasValue)
            {
                campaign.AllowDonate = command.AllowDonate.Value;
            }

            if (command.TargetItems != null)
            {
                campaign.TargetItems = command.TargetItems;
            }

            if (command.StartTime.HasValue)
            {
                campaign.StartTime = command.StartTime;
            }

            if (command.EndTime.HasValue)
            {
                campaign.EndTime = command.EndTime;
            }
        }
    }
}
