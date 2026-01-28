using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;
using VietDonate.Domain.Model.Campaigns;
using VietDonate.Application.UseCases.Media.Queries.GetCampaignMedia;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetCampaign
{
    public class GetCampaignQueryHandler(
        ICampaignRepository campaignRepository,
        IMediaRepository mediaRepository,
        IStorageService storageService) 
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

            var mediaItems = await GetMediaItemsAsync(campaign, cancellationToken);

            var ownerName = campaign.OwnerUser?.UserInformation?.FullName
                ?? campaign.OwnerUser?.UserName
                ?? string.Empty;

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
                OwnerId: campaign.OwnerId,
                OwnerName: ownerName,
                MediaItems: mediaItems
            );

            return Result.Success(result);
        }

        async Task<List<MediaItem>> GetMediaItemsAsync(
            Campaign campaign,
            CancellationToken cancellationToken)
        {
            var mediaList = await mediaRepository.GetByCampaignIdAsync(campaign.Id, cancellationToken);

            var mediaItems = new List<MediaItem>();
            foreach (var media in mediaList)
            {
                var url = await storageService.GetUrlAsync(media.Path);
                mediaItems.Add(new MediaItem(
                    Id: media.Id,
                    Type: media.Type,
                    Status: media.Status,
                    Path: media.Path,
                    Url: url,
                    DisplayOrder: media.DisplayOrder,
                    CreateTime: media.CreateTime,
                    UpdateTime: media.UpdateTime
                ));
            }

            return mediaItems;
        }
    }
}
