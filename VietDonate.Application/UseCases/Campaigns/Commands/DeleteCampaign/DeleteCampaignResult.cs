namespace VietDonate.Application.UseCases.Campaigns.Commands.DeleteCampaign
{
    public record DeleteCampaignResult(
        Guid CampaignId,
        string Message
    );
}
