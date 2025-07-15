using Asp.Versioning;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VietDonate.Application.Common.SwaggerExtension
{
    public class AddApiVersionExampleValueOperationFilter : IOperationFilter
    {
        private const string ApiVersionQueryParameter = "v";
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiVersionParameter = operation.Parameters.SingleOrDefault(p => p.Name == ApiVersionQueryParameter);
            if (apiVersionParameter == null)
            {
                // maybe we should warn the user if they are using this filter without applying the QueryStringApiVersionReader as the ApiVersionReader
                return;
            }
            // get the [ApiVersion("VV")] attribute
            var attribute = context?.MethodInfo?.DeclaringType?
              .GetCustomAttributes(typeof(ApiVersionAttribute), false)
              .Cast<ApiVersionAttribute>()
              .SingleOrDefault();
            // extract the value of the api version
            var version = attribute?.Versions?.SingleOrDefault()?.ToString();
            // may be we should warn if we find un-versioned ApiControllers/ operations?
            if (version != null)
            {
                apiVersionParameter.Example = new OpenApiString(version);
                apiVersionParameter.Schema.Example = new OpenApiString(version);
            }
        }
    }
}
