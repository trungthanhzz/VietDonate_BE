using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Queries.GetMedia
{
    public static class GetMediaErrors
    {
        public static readonly Error MediaNotFound = new(ErrorType.NotFound, "Media not found");
    }
}
