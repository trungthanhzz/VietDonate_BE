namespace VietDonate.Application.UseCases.Media.Queries.GetCampaignMedia
{
    public record GetCampaignMediaResult(
        List<MediaItem> MediaItems
    );

    public record MediaItem(
        Guid Id,
        string Type,
        string? Status,
        string Path,
        string Url,
        int DisplayOrder,
        DateTime CreateTime,
        DateTime? UpdateTime
    );
}
