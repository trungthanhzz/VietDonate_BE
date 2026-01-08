using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Campaigns.Commands.CreateCampaign
{
    public static class CreateCampaignErrors
    {
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to create a campaign");
        public static readonly Error NameRequired = new(ErrorType.Validation, "Campaign name is required");
        public static readonly Error ShortDescriptionRequired = new(ErrorType.Validation, "Short description is required");
        public static readonly Error TypeRequired = new(ErrorType.Validation, "Campaign type is required");
        public static readonly Error UrgencyLevelRequired = new(ErrorType.Validation, "Urgency level is required");
        public static readonly Error InvalidDateRange = new(ErrorType.Validation, "End time must be after start time");
    }
}
