namespace VietDonate.Application.UseCases.Campaigns.Commands.CreateCampaign
{
    public record CreateCampaignResult(
        Guid CampaignId,
        string Code,
        string Name,
        string Message
    );
}
