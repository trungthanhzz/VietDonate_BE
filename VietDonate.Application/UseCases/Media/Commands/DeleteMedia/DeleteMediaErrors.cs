using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Commands.DeleteMedia
{
    public static class DeleteMediaErrors
    {
        public static readonly Error MediaNotFound = new(ErrorType.NotFound, "Media not found");
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to delete this media");
        public static readonly Error DeleteFailed = new(ErrorType.InternalServerError, "Failed to delete media");
    }
}
