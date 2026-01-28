using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Queries.GetTempMedia
{
    public static class GetTempMediaErrors
    {
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to access temp media");
    }
}
