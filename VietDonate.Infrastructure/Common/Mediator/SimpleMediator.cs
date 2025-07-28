using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using VietDonate.Application.Common.Mediator;
using VietDonate.Domain.Common;

namespace VietDonate.Infrastructure.Common.Mediator
{
    public class SimpleMediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public SimpleMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            dynamic handler = _serviceProvider.GetService(handlerType);
            if (handler == null)
            {
                throw new InvalidOperationException($"No handler registered for {handlerType.Name}");
            }
            return handler.Handle((dynamic)command, cancellationToken);
        }

        public Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _serviceProvider.GetService(handlerType);
            if (handler == null)
            {
                throw new InvalidOperationException($"No handler registered for {handlerType.Name}");
            }

            return handler.Handle((dynamic)query, cancellationToken);
        }

        public async Task Publish<TNotification>(TNotification @event, CancellationToken cancellationToken = default)
            where TNotification : IDomainEvent
        {
            var handlerType = typeof(INotificationHandler<>).MakeGenericType(typeof(TNotification));
            var handlers = _serviceProvider.GetServices(handlerType);
            if (handlers?.Any() == false)
            {
                throw new InvalidOperationException($"No handlers registered for {handlerType.Name}");
            }

            foreach (dynamic handler in handlers)
            {
                await handler.Handle((dynamic)@event, cancellationToken);
            }
        }
    }
}
