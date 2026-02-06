namespace VietDonate.Application.UseCases.Comments.Queries.GetCommentsByPost
{
    public record CommentItem(
        Guid Id,
        string Content,
        Guid UserId,
        string? UserName,
        string? UserFullName,
        DateTime CreateTime
    );

    public record GetCommentsByPostResult(
        Guid PostId,
        List<CommentItem> Comments
    );
}

