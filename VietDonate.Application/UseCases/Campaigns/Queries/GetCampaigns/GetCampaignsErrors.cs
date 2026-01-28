using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetCampaigns
{
    public static class GetCampaignsErrors
    {
        public static readonly Error InvalidPageNumber = new(ErrorType.Validation, "Page number must be greater than 0");
        public static readonly Error InvalidPageSize = new(ErrorType.Validation, "Page size must be between 1 and 100");
    }
}
