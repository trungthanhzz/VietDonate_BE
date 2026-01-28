using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Commands.ApproveCampaign
{
    public static class ApproveCampaignErrors
    {
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to update campaign status");
        public static readonly Error CampaignNotFound = new(ErrorType.NotFound, "Campaign not found");
        public static readonly Error InvalidStatus = new(ErrorType.Validation, "Invalid status. Status must be 'Approved' or 'Rejected'");
        public static readonly Error InvalidCurrentStatus = new(ErrorType.Validation, "Campaign cannot be updated in its current status");
        public static readonly Error RejectionReasonRequired = new(ErrorType.Validation, "Rejection reason is required when rejecting a campaign");
        public static readonly Error StatusNotChanged = new(ErrorType.Conflict, "Campaign is already in the requested status");
    }
}
