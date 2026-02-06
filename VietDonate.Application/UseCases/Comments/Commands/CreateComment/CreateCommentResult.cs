namespace VietDonate.Application.UseCases.Comments.Commands.CreateComment
{
    public record CreateCommentResult(
        Guid CommentId,
        string Message
    );
}

