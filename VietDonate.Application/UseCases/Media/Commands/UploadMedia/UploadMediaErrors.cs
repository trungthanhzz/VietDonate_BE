using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Commands.UploadMedia
{
    public static class UploadMediaErrors
    {
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to upload media");
        public static readonly Error FileRequired = new(ErrorType.Validation, "File is required");
        public static readonly Error FileNameRequired = new(ErrorType.Validation, "File name is required");
        public static readonly Error ContentTypeRequired = new(ErrorType.Validation, "Content type is required");
        public static readonly Error FileSizeExceeded = new(ErrorType.Validation, "File size exceeds the maximum allowed size");
        public static readonly Error InvalidFileType = new(ErrorType.Validation, "File type is not allowed");
        public static readonly Error UploadFailed = new(ErrorType.InternalServerError, "Failed to upload file to storage");
    }
}
