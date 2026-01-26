using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Queries.GetCampaignMedia
{
    public static class GetCampaignMediaErrors
    {
        public static readonly Error CampaignNotFound = new(ErrorType.NotFound, "Campaign not found");
    }
}
