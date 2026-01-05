using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace VietDonate.API.Utils.ExceptionHandler
{
    public class CustomExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var statusCode = exception switch
            {
                ArgumentException or ArgumentNullException or InvalidOperationException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            httpContext.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = statusCode == StatusCodes.Status500InternalServerError 
                    ? "An internal server error occurred" 
                    : "An error occurred",
                Detail = statusCode == StatusCodes.Status500InternalServerError 
                    ? "An unexpected error occurred. Please try again later." 
                    : exception.Message,
                Status = statusCode
            };

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails
            });
        }
    }
}
