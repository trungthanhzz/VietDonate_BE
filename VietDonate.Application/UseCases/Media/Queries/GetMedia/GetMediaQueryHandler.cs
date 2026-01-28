using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Queries.GetMedia
{
    public class GetMediaQueryHandler(
        IMediaRepository mediaRepository,
        IStorageService storageService)
        : IQueryHandler<GetMediaQuery, Result<GetMediaResult>>
    {
        public async Task<Result<GetMediaResult>> Handle(
            GetMediaQuery query,
            CancellationToken cancellationToken)
        {
            var media = await mediaRepository.GetByIdAsync(query.MediaId, cancellationToken);

            if (media == null)
            {
                return Result.Failure<GetMediaResult>(GetMediaErrors.MediaNotFound);
            }

            var url = await storageService.GetUrlAsync(media.Path);

            var result = new GetMediaResult(
                Id: media.Id,
                Type: media.Type,
                Status: media.Status,
                Path: media.Path,
                Url: url,
                DisplayOrder: media.DisplayOrder,
                CreateTime: media.CreateTime,
                UpdateTime: media.UpdateTime,
                CampaignId: media.CampaignId,
                UserId: media.UserId,
                PostId: media.PostId
            );

            return Result.Success(result);
        }
    }
}
