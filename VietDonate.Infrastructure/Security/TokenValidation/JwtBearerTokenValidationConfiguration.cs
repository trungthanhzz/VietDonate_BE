using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VietDonate.Infrastructure.Security.TokenGenerator;
using VietDonate.Infrastructure.Configurations;

namespace VietDonate.Infrastructure.Security.TokenValidation;

public sealed class JwtBearerTokenValidationConfiguration(
    IOptions<JwtSettings> jwtSettings,
    IOptions<CookieConfig> cookieConfig)
    : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly CookieConfig _cookieConfig = cookieConfig.Value;

    public void Configure(string name, JwtBearerOptions options) => Configure(options);

    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
        };

        // Đọc token từ cookie nếu không có trong Authorization header
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Nếu không có token trong Authorization header, thử đọc từ cookie
                if (string.IsNullOrEmpty(context.Token))
                {
                    context.Token = context.Request.Cookies[_cookieConfig.AccessTokenCookieName];
                }
                return System.Threading.Tasks.Task.CompletedTask;
            }
        };
    }
}
