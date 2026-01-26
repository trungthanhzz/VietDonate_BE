using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Commands.UploadMedia
{
    public record UploadMediaCommand(
        Stream FileStream,
        string FileName,
        string ContentType,
        long FileSize
    ) : ICommand<Result<UploadMediaResult>>;

}
