namespace VietDonate.Application.Tokens.Queries.Generate;

public record GenerateTokenResult(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Token);