using VietDonate.Application.Common.Errors;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Commands.AssignMediaToCampaign
{
    public static class AssignMediaToCampaignErrors
    {
        public static readonly Error Unauthorized = new(ErrorType.Unauthorized, "You are not authorized to assign media to this campaign");
        public static readonly Error CampaignNotFound = new(ErrorType.NotFound, "Campaign not found");
        public static readonly Error TempMediaNotFound = new(ErrorType.NotFound, "One or more temp media items not found");
        public static readonly Error MediaNotFound = new(ErrorType.NotFound, "One or more media items not found");
        public static readonly Error NoMediaToAssign = new(ErrorType.Validation, "No media to assign");
    }
}
