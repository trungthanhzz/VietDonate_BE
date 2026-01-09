using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Queries.GetCampaign
{
    public static class GetCampaignErrors
    {
        public static readonly Error CampaignNotFound = new(ErrorType.NotFound, "Campaign not found");
    }
}
