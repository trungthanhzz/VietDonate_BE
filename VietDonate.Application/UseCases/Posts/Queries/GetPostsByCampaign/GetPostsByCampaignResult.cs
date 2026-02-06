namespace VietDonate.Application.UseCases.Posts.Queries.GetPostsByCampaign
{
    public record GetPostsByCampaignResult(
        List<PostItem> Posts
    );

    public record PostItem(
        Guid Id,
        string Title,
        string Content,
        string PostType,
        string? Status,
        Guid UserId,
        Guid? CampaignId,
        int ViewCount,
        int LikeCount,
        int CommentCount,
        string? ProofType,
        DateTime? ProofDate,
        DateTime CreateTime,
        DateTime? UpdateTime,
        string? UserName,
        string? UserFullName
    );
}
