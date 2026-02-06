namespace VietDonate.Infrastructure.ModelInfrastructure.Comments.Contracts
{
    public record CreateCommentRequest(
        Guid PostId,
        string Content,
        Guid? ParentId
    );
}

