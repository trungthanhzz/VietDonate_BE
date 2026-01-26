using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Queries.GetCampaignMedia
{
    public class GetCampaignMediaQueryHandler(
        IMediaRepository mediaRepository,
        IStorageService storageService)
        : IQueryHandler<GetCampaignMediaQuery, Result<GetCampaignMediaResult>>
    {
        public async Task<Result<GetCampaignMediaResult>> Handle(
            GetCampaignMediaQuery query,
            CancellationToken cancellationToken)
        {
            var mediaList = await mediaRepository.GetByCampaignIdAsync(query.CampaignId, cancellationToken);

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

            var result = new GetCampaignMediaResult(MediaItems: mediaItems);
            return Result.Success(result);
        }
    }
}
