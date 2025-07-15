using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietDonate.Domain.Common;

namespace VietDonate.Application.Common.Mediator
{
    public interface IPublisher
    {
        Task Publish<TNotification>(TNotification @event, CancellationToken cancellationToken = default)
            where TNotification : IDomainEvent;
    }
}
