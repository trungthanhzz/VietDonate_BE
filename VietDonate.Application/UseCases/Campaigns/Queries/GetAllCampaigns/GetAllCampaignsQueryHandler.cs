using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Model.Campaigns;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetAllCampaigns
{
    public class GetAllCampaignsQueryHandler(
        ICampaignRepository campaignRepository)
        : IQueryHandler<GetAllCampaignsQuery, Result<GetAllCampaignsResult>>
    {
        public async Task<Result<GetAllCampaignsResult>> Handle(
            GetAllCampaignsQuery query,
            CancellationToken cancellationToken)
        {
            var campaigns = await campaignRepository.GetAllAsync(cancellationToken);

            var campaignItems = campaigns.Select(c => new CampaignItem(
                Id: c.Id,
                Code: c.Code,
                Name: c.Name,
                CreatedDate: c.CreatedDate,
                ShortDescription: c.ShortDescription,
                TargetAmount: c.TargetAmount,
                CurrentAmount: c.CurrentAmount,
                Type: c.Type,
                UrgencyLevel: c.UrgencyLevel,
                Status: c.Status,
                ViewCount: c.ViewCount,
                DonorCount: c.DonorCount,
                OwnerId: c.OwnerId
            )).ToList();

            return Result.Success(new GetAllCampaignsResult(Campaigns: campaignItems));
        }
    }
}
