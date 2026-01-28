using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Queries.GetTempMedia
{
    public class GetTempMediaQueryHandler(
        ITempMediaService tempMediaService,
        IRequestContextService requestContextService)
        : IQueryHandler<GetTempMediaQuery, Result<GetTempMediaResult>>
    {
        public async Task<Result<GetTempMediaResult>> Handle(
            GetTempMediaQuery query,
            CancellationToken cancellationToken)
        {
            var userId = requestContextService.GetUserIdAsGuid();
            if (userId == null)
            {
                return Result<GetTempMediaResult>.ValidationFailure(GetTempMediaErrors.Unauthorized);
            }

            var tempMediaList = await tempMediaService.GetUserTempMediaAsync(userId.Value, cancellationToken);

            var tempMediaItems = tempMediaList.Select(tm => new TempMediaItem(
                MediaId: tm.MediaId,
                FileName: tm.FileName,
                ContentType: tm.ContentType,
                Url: tm.Url,
                FileSize: tm.FileSize,
                UploadTime: tm.UploadTime
            )).ToList();

            var result = new GetTempMediaResult(TempMediaItems: tempMediaItems);
            return Result.Success(result);
        }
    }
}
