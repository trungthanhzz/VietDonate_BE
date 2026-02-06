using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;
using System.Diagnostics;

namespace VietDonate.API.Common;

[ApiController]
[Authorize]
public class ApiController : ControllerBase
{
    protected ActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors[0]);
    }

    protected IActionResult Problem(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot create problem result from successful result");
        }

        return Problem(result.Error);
    }

    protected IActionResult Problem<TValue>(Result<TValue> result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot create problem result from successful result");
        }

        return Problem(result.Error);
    }

    private ObjectResult Problem(Error error)
    {
        var statusCode = GetStatusCode(error);
        var type = GetErrorTypeUrl(error.Type);
        var traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        var problemDetails = new ProblemDetails
        {
            Type = type,
            Title = error.Message ?? GetDefaultTitle(error.Type),
            Status = statusCode,
            Instance = HttpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = traceId;

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }

    private int GetStatusCode(Error error)
    {
        if (error.Type == ErrorType.NotFound)
            return StatusCodes.Status404NotFound;

        if (error.Type == ErrorType.Validation)
            return StatusCodes.Status400BadRequest;

        if (error.Type == ErrorType.Conflict)
            return StatusCodes.Status409Conflict;

        if (error.Type == ErrorType.Unauthorized)
            return StatusCodes.Status401Unauthorized;

        if (error.Type == ErrorType.Forbidden)
            return StatusCodes.Status403Forbidden;

        if (error.Type == ErrorType.BadRequest)
            return StatusCodes.Status400BadRequest;

        if (error.Type == ErrorType.ServiceUnavailable)
            return StatusCodes.Status503ServiceUnavailable;

        return StatusCodes.Status500InternalServerError;
    }

    private ActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();
        var traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        var groupedErrors = errors.GroupBy(e => e.Type)
            .ToDictionary(g => g.Key, g => g.Select(e => e.Message ?? "").ToArray());

        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
            Instance = HttpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = traceId;

        foreach (var errorGroup in groupedErrors)
        {
            problemDetails.Errors[errorGroup.Key] = errorGroup.Value;
        }

        return new BadRequestObjectResult(problemDetails);
    }

    private string GetErrorTypeUrl(string errorType)
    {
        return errorType switch
        {
            "NotFound" => "https://tools.ietf.org/html/rfc9110#section-15.5.4",
            "Validation" => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            "Conflict" => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
            "Unauthorized" => "https://tools.ietf.org/html/rfc9110#section-15.5.2",
            "Forbidden" => "https://tools.ietf.org/html/rfc9110#section-15.5.3",
            "BadRequest" => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            "ServiceUnavailable" => "https://tools.ietf.org/html/rfc9110#section-15.5.4",
            _ => "https://tools.ietf.org/html/rfc9110#section-15.5.1"
        };
    }

    private string GetDefaultTitle(string errorType)
    {
        return errorType switch
        {
            "NotFound" => "The requested resource was not found.",
            "Validation" => "One or more validation errors occurred.",
            "Conflict" => "A conflict occurred with the current state of the resource.",
            "Unauthorized" => "You are not authorized to perform this action.",
            "Forbidden" => "You do not have permission to access this resource.",
            "BadRequest" => "The request was invalid or cannot be served.",
            "ServiceUnavailable" => "The service is currently unavailable. Please try again later.",
            _ => "An unexpected error occurred."
        };
    }
}