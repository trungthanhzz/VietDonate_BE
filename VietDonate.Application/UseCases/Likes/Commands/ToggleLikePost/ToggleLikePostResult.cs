namespace VietDonate.Application.UseCases.Likes.Commands.ToggleLikePost
{
    public record ToggleLikePostResult(
        Guid PostId,
        bool IsLiked,
        int LikeCount,
        string Message
    );
}

