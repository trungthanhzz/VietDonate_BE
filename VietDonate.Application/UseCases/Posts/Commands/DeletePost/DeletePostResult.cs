namespace VietDonate.Application.UseCases.Posts.Commands.DeletePost
{
    public record DeletePostResult(
        Guid PostId,
        string Message
    );
}
