using ErrorOr;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Tokens.Queries.Generate;

namespace CleanArchitecture.Application.Tokens.Queries.Generate;

public class GenerateTokenQueryHandler(
    IJwtTokenGenerator _jwtTokenGenerator)
        : IQueryHandler<GenerateTokenQuery, ErrorOr<GenerateTokenResult>>
{
    public Task<ErrorOr<GenerateTokenResult>> Handle(GenerateTokenQuery query, CancellationToken cancellationToken)
    {
        var id = query.Id ?? Guid.NewGuid();

        var token = _jwtTokenGenerator.GenerateToken(
            id,
            query.FirstName,
            query.LastName,
            query.Email,
            query.Permissions,
            query.Roles);

        var authResult = new GenerateTokenResult(
            id,
            query.FirstName,
            query.LastName,
            query.Email,
            token);

        return Task.FromResult(ErrorOrFactory.From(authResult));
    }
}