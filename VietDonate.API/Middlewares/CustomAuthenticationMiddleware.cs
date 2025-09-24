using VietDonate.Application.Common.Interfaces;

namespace VietDonate.API.Middlewares;

public class CustomAuthenticationMiddleware(
  RequestDelegate next,
  IJwtTokenGenerator jwtTokenGenerator
)
{
  private readonly RequestDelegate _next = next;
  
  public async Task InvokeAsync(HttpContext context)
  {
    // Custom authentication logic can be added here
    var token = context.Request.Headers["Cookie"].FirstOrDefault()?.Replace("jwt=", "");

    if (string.IsNullOrEmpty(token))
    {
      await _next(context);
      return;
    }
    var principal =  jwtTokenGenerator.GetPrincipalFromToken(token);
    context.User = principal;
    await _next(context);
  }
}