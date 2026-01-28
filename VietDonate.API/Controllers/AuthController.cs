using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VietDonate.API.Common;
using VietDonate.API.Utils.ExceptionHandler;
using VietDonate.Application.Common.Constants;
using VietDonate.Application.Common.Mediator;
using VietDonate.Infrastructure.ModelInfrastructure.Auths.Contracts;
using VietDonate.Application.UseCases.Auths.Commands.RefreshToken;
using VietDonate.Application.UseCases.Auths.Commands.Logout;
using VietDonate.Application.UseCases.Auths.Commands.ChangePassword;
using VietDonate.Application.UseCases.Auths.Queries.Login;
using VietDonate.Infrastructure.Configurations;
using VietDonate.Infrastructure.Security.TokenGenerator;

namespace VietDonate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        IMediator mediator,
        IOptions<CookieConfig> cookieConfig,
        IOptions<JwtSettings> jwtSettings) : ApiController
    {
        private readonly CookieConfig _cookieConfig = cookieConfig.Value;
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var query = new LoginAccountQuery(
                UserName: request.UserName,
                Password: request.Password,
                IsRemember: request.IsRemember);

            var result = await mediator.Send(query);
            return result.Match(
                onSuccess: loginResult =>
                {
                    SetAuthCookies(loginResult.AccessToken, loginResult.RefreshToken, request.IsRemember);
                    return Ok(loginResult);
                },
                onFailure: Problem
            );
        }

        [HttpPost]
        [Route("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var command = new RefreshTokenCommand(request.RefreshToken);
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: refreshResult =>
                {
                    SetAuthCookies(refreshResult.AccessToken, refreshResult.RefreshToken, false);
                    return Ok(refreshResult);
                },
                onFailure: Problem
            );
        }

        [HttpPost]
        [Route("logout")]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var command = new LogoutCommand(request.RefreshToken);
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: logoutResult =>
                {
                    ClearAuthCookies();
                    return Ok(logoutResult);
                },
                onFailure: Problem
            );
        }

        [HttpPost]
        [Route("change-password")]
        [Authorize(Policy = AuthorizationPolicies.RequireUser)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var command = new ChangePasswordCommand(
                OldPassword: request.OldPassword,
                NewPassword: request.NewPassword);
            
            var result = await mediator.Send(command);
            return result.Match(
                onSuccess: changePasswordResult => Ok(changePasswordResult),
                onFailure: Problem
            );
        }

        private void SetAuthCookies(string accessToken, string refreshToken, bool isRemember)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = _cookieConfig.HttpOnly,
                Secure = _cookieConfig.Secure,
                SameSite = _cookieConfig.SameSite switch
                {
                    "None" => SameSiteMode.None,
                    "Lax" => SameSiteMode.Lax,
                    "Strict" => SameSiteMode.Strict,
                    _ => SameSiteMode.None
                },
                Path = _cookieConfig.Path
            };

            if (!string.IsNullOrEmpty(_cookieConfig.Domain))
            {
                cookieOptions.Domain = _cookieConfig.Domain;
            }

            // Set expiration cho access token
            cookieOptions.Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes);

            // Set access token cookie
            Response.Cookies.Append(_cookieConfig.AccessTokenCookieName, accessToken, cookieOptions);

            // Set expiration cho refresh token (dài hơn)
            cookieOptions.Expires = isRemember
                ? DateTimeOffset.UtcNow.AddDays(30)
                : DateTimeOffset.UtcNow.AddDays(7);

            // Set refresh token cookie
            Response.Cookies.Append(_cookieConfig.RefreshTokenCookieName, refreshToken, cookieOptions);
        }

        private void ClearAuthCookies()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = _cookieConfig.HttpOnly,
                Secure = _cookieConfig.Secure,
                SameSite = _cookieConfig.SameSite switch
                {
                    "None" => SameSiteMode.None,
                    "Lax" => SameSiteMode.Lax,
                    "Strict" => SameSiteMode.Strict,
                    _ => SameSiteMode.None
                },
                Path = _cookieConfig.Path,
                Expires = DateTimeOffset.UtcNow.AddDays(-1) // Xóa cookie bằng cách set expiration trong quá khứ
            };

            if (!string.IsNullOrEmpty(_cookieConfig.Domain))
            {
                cookieOptions.Domain = _cookieConfig.Domain;
            }

            Response.Cookies.Delete(_cookieConfig.AccessTokenCookieName, cookieOptions);
            Response.Cookies.Delete(_cookieConfig.RefreshTokenCookieName, cookieOptions);
        }
    }
}
