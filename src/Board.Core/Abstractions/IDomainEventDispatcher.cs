using System.Threading.Tasks;
using Board.Core.Events;

namespace Board.Core.Abstractions
{
    public interface IDomainEventDispatcher
    {
        Task PublishAsync<T>(T @event) where T : DomainEvent;
    }
}
