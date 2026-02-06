namespace VietDonate.Application.UseCases.Posts.Commands.UpdatePost
{
    public record UpdatePostResult(
        Guid PostId,
        string Message
    );
}
