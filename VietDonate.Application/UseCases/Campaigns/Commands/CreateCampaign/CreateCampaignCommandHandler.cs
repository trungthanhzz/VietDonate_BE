using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Application.Common.Handlers;
using VietDonate.Domain.Model.Campaigns;

namespace VietDonate.Application.UseCases.Campaigns.Commands.CreateCampaign
{
    public class CreateCampaignCommandHandler(
        ICampaignRepository campaignRepository,
        IRequestContextService requestContextService,
        IUnitOfWork unitOfWork)
        : BaseCommandHandler(unitOfWork),
            ICommandHandler<CreateCampaignCommand, Result<CreateCampaignResult>>
    {
        public async Task<Result<CreateCampaignResult>> Handle(
            CreateCampaignCommand command,
            CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<CreateCampaignResult>.ValidationFailure(CreateCampaignErrors.Unauthorized);
            }

            var validationResult = ValidateCreateData(command);
            if (validationResult.IsFailure)
            {
                return Result<CreateCampaignResult>.ValidationFailure(validationResult.Error);
            }

            return await ExecuteInTransactionAsync(async () =>
            {
                var campaignId = Guid.NewGuid();
                var campaignCode = GenerateCampaignCode();
                var campaign = new Campaign(
                    id: campaignId,
                    name: command.Name,
                    code: campaignCode,
                    createdDate: DateTime.UtcNow
                );

                campaign.ShortDescription = command.ShortDescription;
                campaign.FullStory = command.FullStory;
                campaign.TargetAmount = command.TargetAmount;
                campaign.CurrentAmount = 0;
                campaign.Type = command.Type;
                campaign.UrgencyLevel = command.UrgencyLevel;
                campaign.Status = "Pending";
                campaign.AllowComment = command.AllowComment;
                campaign.AllowDonate = command.AllowDonate;
                campaign.TargetItems = command.TargetItems;
                campaign.StartTime = command.StartTime;
                campaign.EndTime = command.EndTime;
                campaign.OwnerId = userId.Value;
                campaign.CreateTime = DateTime.UtcNow;

                await campaignRepository.AddAsync(campaign, cancellationToken);

                return Result.Success(new CreateCampaignResult(
                    CampaignId: campaignId,
                    Code: campaignCode,
                    Name: command.Name,
                    Message: SuccessMessages.Campaign.CreatedSuccessfully
                ));
            });
        }

        private Result ValidateCreateData(CreateCampaignCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                return Result.Failure(CreateCampaignErrors.NameRequired);
            }

            if (string.IsNullOrWhiteSpace(command.ShortDescription))
            {
                return Result.Failure(CreateCampaignErrors.ShortDescriptionRequired);
            }

            if (string.IsNullOrWhiteSpace(command.Type))
            {
                return Result.Failure(CreateCampaignErrors.TypeRequired);
            }

            if (string.IsNullOrWhiteSpace(command.UrgencyLevel))
            {
                return Result.Failure(CreateCampaignErrors.UrgencyLevelRequired);
            }

            if (command.StartTime.HasValue && command.EndTime.HasValue && 
                command.EndTime.Value <= command.StartTime.Value)
            {
                return Result.Failure(CreateCampaignErrors.InvalidDateRange);
            }

            return Result.Success();
        }

        private string GenerateCampaignCode()
        {
            return $"CAM-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";
        }
    }
}
