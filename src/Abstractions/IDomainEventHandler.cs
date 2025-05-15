using Clean.Solutions.Vertical.Primitives;
using MediatR;

namespace Clean.Solutions.Vertical.Abstractions
{
    public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent>
     where TEvent : IDomainEvent
    {
    }
}
