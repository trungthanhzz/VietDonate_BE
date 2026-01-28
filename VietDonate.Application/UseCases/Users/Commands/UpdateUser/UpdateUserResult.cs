namespace VietDonate.Application.UseCases.Users.Commands.UpdateUser
{
    public record UpdateUserResult(
        Guid UserId,
        string Message
    );
}

