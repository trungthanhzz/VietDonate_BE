using VietDonate.Application.Common.Errors;

namespace VietDonate.Application.Common.Result
{
    public record Error(string Type, string? Message = null)
    {
        public static Error UnexpectedError = new(ErrorType.InternalServerError, "An unexpected error occurred.");
        public static Error NotFound = new(ErrorType.NotFound, "The requested resource was not found.");
        public static Error ValidationError = new(ErrorType.Validation, "One or more validation errors occurred.");
        public static Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to perform this action.");
        public static Error Forbidden = new(ErrorType.Forbidden, "You do not have permission to access this resource.");
        public static Error Conflict = new(ErrorType.Conflict, "A conflict occurred with the current state of the resource.");
        public static Error BadRequest = new(ErrorType.Validation, "The request was invalid or cannot be served.");
        public static Error ServiceUnavailable = new(ErrorType.InternalServerError, "The service is currently unavailable. Please try again later.");
        public static Error None = new(ErrorType.None, null);
    }
}
