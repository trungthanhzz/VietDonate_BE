using ErrorOr;
using VietDonate.Application.Common.Mediator;

namespace VietDonate.Application.UseCases.Tokens.Queries.Generate;

public record GenerateTokenQuery(
    Guid? Id,
    string FirstName,
    string LastName,
    string Email,
    List<string> Permissions,
    string Role) : IQuery<ErrorOr<GenerateTokenResult>>;