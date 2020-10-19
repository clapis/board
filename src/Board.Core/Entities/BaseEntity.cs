using System;
using System.Collections.Generic;
using Board.Core.Events;

namespace Board.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<DomainEvent> Events { get; set; } = new List<DomainEvent>();

    }
}
