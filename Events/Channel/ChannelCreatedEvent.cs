using System;
using Ncqrs.Domain;
using ProtoBuf;
using Ncqrs.Eventing;
using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    [ProtoContract]
    public class ChannelCreatedEvent : SourcedEvent, IIdentifiable
    {
        [ProtoMember(1)]public Guid ID { get; set; }
        [ProtoMember(2)]public string Name { get; set; }
        [ProtoMember(3)]public DateTime TimeStamp { get; set; }

        public ChannelCreatedEvent()
            : base(Guid.NewGuid(), Guid.Empty, 1, DateTime.UtcNow)
        {
        }

        public Guid GetID()
        {
            return EventSourceId;
        }
    }
}
