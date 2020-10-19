using System;
namespace Board.Core.Events
{
    public abstract class DomainEvent
    {
        public DateTime RaisedOn { get; protected set; } = DateTime.UtcNow;
    }
}
