namespace VietDonate.Application.UseCases.Posts.Commands.CreatePost
{
    public record CreatePostResult(
        Guid PostId,
        string Message
    );
}
