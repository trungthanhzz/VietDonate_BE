using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Commands.DeleteCampaign
{
    public static class DeleteCampaignErrors
    {
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to delete this campaign");
        public static readonly Error CampaignNotFound = new(ErrorType.NotFound, "Campaign not found");
        public static readonly Error Forbidden = new(ErrorType.Forbidden, "You do not have permission to delete this campaign");
    }
}
