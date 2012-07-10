using System;
using System.Collections.Generic;
using Ncqrs.Eventing.Sourcing;
using Ncqrs.Eventing.Storage;

namespace Eventing
{
    public interface IExplorableEventStore:IEventStore
    {
        IEnumerable<Guid> GetEventSourceIndex();
        void RemoveEmptyEventSource(Guid guid);
        IEnumerable<ISourcedEvent> GetAllEvents(Guid source);
        void StoreEmptyEventSource(Guid source);
    }
}
