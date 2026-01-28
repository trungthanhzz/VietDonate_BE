using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Model.Campaigns;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetAllCampaigns
{
    public class GetAllCampaignsQueryHandler(
        ICampaignRepository campaignRepository,
        IMediaRepository mediaRepository,
        IStorageService storageService)
        : IQueryHandler<GetAllCampaignsQuery, Result<GetAllCampaignsResult>>
    {
        public async Task<Result<GetAllCampaignsResult>> Handle(
            GetAllCampaignsQuery query,
            CancellationToken cancellationToken)
        {
            var campaigns = await campaignRepository.GetAllAsync(cancellationToken);

            var campaignItems = new List<CampaignItem>();

            foreach (var campaign in campaigns)
            {
                var mediaList = await mediaRepository.GetByCampaignIdAsync(
                    campaign.Id,
                    cancellationToken);

                var imageUrl = string.Empty;
                if (mediaList.Count > 0)
                {
                    var firstMedia = mediaList.OrderBy(m => m.DisplayOrder)
                        .ThenBy(m => m.CreateTime)
                        .First();
                    imageUrl = await storageService.GetUrlAsync(firstMedia.Path);
                }

                var campaignItem = new CampaignItem(
                    Id: campaign.Id,
                    Code: campaign.Code,
                    Name: campaign.Name,
                    CreatedDate: campaign.CreatedDate,
                    ShortDescription: campaign.ShortDescription,
                    TargetAmount: campaign.TargetAmount,
                    CurrentAmount: campaign.CurrentAmount,
                    Type: campaign.Type,
                    UrgencyLevel: campaign.UrgencyLevel,
                    Status: campaign.Status,
                    ViewCount: campaign.ViewCount,
                    DonorCount: campaign.DonorCount,
                    OwnerId: campaign.OwnerId,
                    OwnerName: campaign.OwnerUser?.UserInformation?.FullName
                        ?? campaign.OwnerUser?.UserName
                        ?? string.Empty,
                    ImageUrl: imageUrl
                );

                campaignItems.Add(campaignItem);
            }

            return Result.Success(new GetAllCampaignsResult(Campaigns: campaignItems));
        }
    }
}
