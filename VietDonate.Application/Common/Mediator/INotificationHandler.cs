using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietDonate.Domain.Common;

namespace VietDonate.Application.Common.Mediator
{
    public interface INotificationHandler<TNotification>
        where TNotification : IDomainEvent
    {
        Task Handle(TNotification notification, CancellationToken cancellationToken);
    }
}
