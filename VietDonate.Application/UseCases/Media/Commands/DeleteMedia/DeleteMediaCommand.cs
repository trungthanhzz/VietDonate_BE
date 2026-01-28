using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Commands.DeleteMedia
{
    public record DeleteMediaCommand(Guid MediaId) : ICommand<Result<DeleteMediaResult>>;
}
