namespace VietDonate.Application.UseCases.Campaigns.Commands.UpdateCampaign
{
    public record UpdateCampaignResult(
        Guid CampaignId,
        string Message
    );
}
