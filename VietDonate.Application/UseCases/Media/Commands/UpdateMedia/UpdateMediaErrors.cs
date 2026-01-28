using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Commands.UpdateMedia
{
    public static class UpdateMediaErrors
    {
        public static readonly Error MediaNotFound = new(ErrorType.NotFound, "Media not found");
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to update this media");
    }
}
