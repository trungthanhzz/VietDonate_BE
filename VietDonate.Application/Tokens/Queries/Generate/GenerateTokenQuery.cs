using ErrorOr;
using VietDonate.Application.Common.Mediator;


namespace VietDonate.Application.Tokens.Queries.Generate;

public record GenerateTokenQuery(
    Guid? Id,
    string FirstName,
    string LastName,
    string Email,
    List<string> Permissions,
    List<string> Roles) : IQuery<ErrorOr<GenerateTokenResult>>;