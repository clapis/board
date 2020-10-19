using System;
using System.Threading.Tasks;
using Board.Core.Abstractions;
using Board.Core.Events;
using MediatR;

namespace Board.Application
{
    public class Notification<T> : INotification where T : DomainEvent
    {
        public T Event { get; }
        public Notification(T @event) => Event = @event;
    }

    public class EventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public EventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task PublishAsync<T>(T @event) where T : DomainEvent
        {
            var type = typeof(Notification<>).MakeGenericType(@event.GetType());
            var notification = (INotification)Activator.CreateInstance(type, @event);

            return _mediator.Publish(notification);
        }
    }
}
