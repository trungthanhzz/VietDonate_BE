using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietDonate.Application.Common.Mediator;

namespace VietDonate.Application.UseCases.Auths.Queries.Login
{
    public record LoginAccountQuery(
        string UserName,
        string Password,
        bool IsRemember = false)
        : IQuery<ErrorOr<LoginResult>>;
}
