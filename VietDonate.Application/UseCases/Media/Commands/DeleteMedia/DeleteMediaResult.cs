namespace VietDonate.Application.UseCases.Media.Commands.DeleteMedia
{
    public record DeleteMediaResult(
        Guid MediaId,
        string Message
    );
}
