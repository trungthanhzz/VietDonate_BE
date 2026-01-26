namespace VietDonate.Application.UseCases.Media.Queries.GetMedia
{
    public record GetMediaResult(
        Guid Id,
        string Type,
        string? Status,
        string Path,
        string Url,
        int DisplayOrder,
        DateTime CreateTime,
        DateTime? UpdateTime,
        Guid? CampaignId,
        Guid? UserId,
        Guid? PostId
    );
}
