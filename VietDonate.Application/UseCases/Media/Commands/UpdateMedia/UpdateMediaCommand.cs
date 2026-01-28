using VietDonate.Application.Common.Mediator;
using VietDonate.Application.Common.Result;

namespace VietDonate.Application.UseCases.Media.Commands.UpdateMedia
{
    public record UpdateMediaCommand(
        Guid MediaId,
        int? DisplayOrder,
        string? Status
    ) : ICommand<Result<UpdateMediaResult>>;
}
