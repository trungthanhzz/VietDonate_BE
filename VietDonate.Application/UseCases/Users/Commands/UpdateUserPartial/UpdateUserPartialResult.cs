namespace VietDonate.Application.UseCases.Users.Commands.UpdateUserPartial
{
    public record UpdateUserPartialResult(
        Guid UserId,
        string Message
    );
}

