namespace VietDonate.Application.UseCases.Comments.Commands.DeleteComment
{
    public record DeleteCommentResult(
        Guid CommentId,
        string Message
    );
}

