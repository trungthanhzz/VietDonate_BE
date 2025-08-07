namespace VietDonate.Application.UseCases.Users.Commands.Register
{
    public record RegisterUserResult(
        Guid UserId,
        string UserName,
        string FullName,
        string Message
    );
} 