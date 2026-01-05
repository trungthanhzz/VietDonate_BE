using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using VietDonate.Application.Common.SwaggerExtension;
using VietDonate.Infrastructure;
using VietDonate.Application;
using VietDonate.Infrastructure.Common.Middleware;
using VietDonate.API.Utils.ExceptionHandler;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.

services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerGenConfigurationOptions>();
services.AddSwaggerGen();

services
    .AddApplication()
    .AddInfrastructure(configuration);
services.AddControllers();

services.AddExceptionHandler<CustomExceptionHandler>();
services.AddProblemDetails();

services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:3000", "http://localhost:5173", "http://localhost:3002" })
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
var app = builder.Build();

app.UseSwagger(c =>
    c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0
);
app.UseSwaggerUI(options =>
{
    using var scope = app.Services.CreateScope();
    var providerApi = scope.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in providerApi.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});



// Use exception handler (must be early in the pipeline)
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseJwtBlacklist();
app.UseAuthorization();

app.MapControllers();

app.Run();
