using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Posts.Queries.GetPostsByCampaign
{
    public static class GetPostsByCampaignErrors
    {
        public static readonly Error CampaignNotFound = new(ErrorType.NotFound, "Campaign not found");
    }
}
