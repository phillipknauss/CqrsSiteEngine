using System;
using Ncqrs.Domain;
using ProtoBuf;
using Ncqrs.Eventing;
using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    [ProtoContract]
    public class UserCreatedEvent : SourcedEvent, IIdentifiable
    {
        [ProtoMember(1)]public Guid Id { get; set; }
        [ProtoMember(2)]public string Name { get; set; }
        [ProtoMember(4)]public DateTime TimeStamp { get; set; }
        [ProtoMember(5)]
        public long Sequence
        {
            get
            {
                return this.EventSequence;
            }
            set
            {
                this.ClaimEvent(Id, value);
            }
        }
        
        public UserCreatedEvent() : base(Guid.NewGuid(), Guid.Empty, 1, DateTime.UtcNow)
        {
        }

        public Guid GetID()
        {
            return EventSourceId;
        }
    }
}
