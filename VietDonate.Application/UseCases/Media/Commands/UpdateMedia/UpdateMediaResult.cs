namespace VietDonate.Application.UseCases.Media.Commands.UpdateMedia
{
    public record UpdateMediaResult(
        Guid MediaId,
        int DisplayOrder,
        string? Status,
        string Message
    );
}
