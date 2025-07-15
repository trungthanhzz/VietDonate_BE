using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietDonate.Application.Common.Mediator
{
    public interface ISender
    {
        Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
        Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    }
}
