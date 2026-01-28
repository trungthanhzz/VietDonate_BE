namespace VietDonate.Application.UseCases.Media.Commands.AssignMediaToCampaign
{
    public record AssignMediaToCampaignResult(
        Guid CampaignId,
        int AssignedCount,
        List<Guid> AssignedMediaIds,
        string Message
    );
}
