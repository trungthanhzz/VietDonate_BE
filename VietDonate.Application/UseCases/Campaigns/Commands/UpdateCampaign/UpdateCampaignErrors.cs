using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Commands.UpdateCampaign
{
    public static class UpdateCampaignErrors
    {
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to update this campaign");
        public static readonly Error CampaignNotFound = new(ErrorType.NotFound, "Campaign not found");
        public static readonly Error Forbidden = new(ErrorType.Forbidden, "You do not have permission to update this campaign");
        public static readonly Error InvalidDateRange = new(ErrorType.Validation, "End time must be after start time");
    }
}
