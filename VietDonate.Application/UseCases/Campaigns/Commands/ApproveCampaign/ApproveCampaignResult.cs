namespace VietDonate.Application.UseCases.Campaigns.Commands.ApproveCampaign
{
    public record ApproveCampaignResult(
        Guid CampaignId,
        string Message
    );
}
