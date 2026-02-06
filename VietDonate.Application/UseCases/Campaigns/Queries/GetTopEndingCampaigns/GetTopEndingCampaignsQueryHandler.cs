using System;
using System.Linq;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetTopEndingCampaigns
{
    public class GetTopEndingCampaignsQueryHandler(
        ICampaignRepository campaignRepository,
        IMediaRepository mediaRepository,
        IStorageService storageService)
        : IQueryHandler<GetTopEndingCampaignsQuery, Result<GetTopEndingCampaignsResult>>
    {
        private const int MaxCount = 50;
        private const int DefaultCount = 10;

        public async Task<Result<GetTopEndingCampaignsResult>> Handle(
            GetTopEndingCampaignsQuery query,
            CancellationToken cancellationToken)
        {
            var normalizedCount = query.Count;

            if (normalizedCount < 1)
            {
                normalizedCount = DefaultCount;
            }

            if (normalizedCount > MaxCount)
            {
                normalizedCount = MaxCount;
            }

            var campaigns = await campaignRepository.GetTopByEndTimeAsync(
                normalizedCount,
                cancellationToken);

            var items = new List<DashboardCampaignItem>();

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
    
                var percentAchieved = (campaign.TargetAmount ?? 0) > 0
                    ? Math.Round(
                        (campaign.CurrentAmount ?? 0) / (campaign.TargetAmount ?? 0) * 100,
                        2)
                    : 0;

                var creatorName = campaign.OwnerUser?.UserInformation?.FullName
                    ?? campaign.OwnerUser?.UserName
                    ?? string.Empty;

                var creatorAvatarUrl = campaign.OwnerUser?.UserInformation?.AvtUrl
                    ?? string.Empty;

                var item = new DashboardCampaignItem(
                    Id: campaign.Id,
                    Code: campaign.Code,
                    Name: campaign.Name,
                    ImageUrl: imageUrl,
                    StartTime: campaign.StartTime,
                    EndTime: campaign.EndTime,
                    TargetAmount: campaign.TargetAmount,
                    CurrentAmount: campaign.CurrentAmount,
                    PercentAchieved: percentAchieved,
                    DonorCount: campaign.DonorCount,
                    Creator: new DashboardCampaignCreator(
                        Id: campaign.OwnerUser?.Id ?? Guid.Empty,
                        Name: creatorName,
                        AvatarUrl: creatorAvatarUrl));

                items.Add(item);
            }

            var result = new GetTopEndingCampaignsResult(items);

            return Result.Success(result);
        }
    }
}