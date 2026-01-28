using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Model.Campaigns;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetCampaign
{
    public class GetCampaignQueryHandler(
        ICampaignRepository campaignRepository) 
        : IQueryHandler<GetCampaignQuery, Result<GetCampaignResult>>
    {
        public async Task<Result<GetCampaignResult>> Handle(
            GetCampaignQuery query, 
            CancellationToken cancellationToken)
        {
            var campaign = await campaignRepository.GetByIdAsync(query.CampaignId, cancellationToken);
            
            if (campaign == null)
            {
                return Result.Failure<GetCampaignResult>(GetCampaignErrors.CampaignNotFound);
            }

            var result = new GetCampaignResult(
                Id: campaign.Id,
                Code: campaign.Code,
                Name: campaign.Name,
                CreatedDate: campaign.CreatedDate,
                ShortDescription: campaign.ShortDescription,
                FullStory: campaign.FullStory,
                TargetAmount: campaign.TargetAmount,
                CurrentAmount: campaign.CurrentAmount,
                Type: campaign.Type,
                UrgencyLevel: campaign.UrgencyLevel,
                Status: campaign.Status,
                AllowComment: campaign.AllowComment,
                AllowDonate: campaign.AllowDonate,
                TargetItems: campaign.TargetItems,
                CurrentItems: campaign.CurrentItems,
                StartTime: campaign.StartTime,
                EndTime: campaign.EndTime,
                ApprovedTime: campaign.ApprovedTime,
                CompletedTime: campaign.CompletedTime,
                VerificationNote: campaign.VerificationNote,
                RejectionReason: campaign.RejectionReason,
                FactCheckNote: campaign.FactCheckNote,
                ViewCount: campaign.ViewCount,
                DonorCount: campaign.DonorCount,
                CreateTime: campaign.CreateTime,
                UpdateTime: campaign.UpdateTime,
                OwnerId: campaign.OwnerId
            );

            return Result.Success(result);
        }
    }
}
