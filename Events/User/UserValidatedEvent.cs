using System;
using ProtoBuf;
using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    [ProtoContract]
    public class UserValidatedEvent : SourcedEvent, IIdentifiable
    {
        [ProtoMember(1)]public Guid UserID { get; set; }
        [ProtoMember(2)]public DateTime TimeStamp { get; set; }
        [ProtoMember(3)]
        public long Sequence
        {
            get
            {
                return EventSequence;
            }
            set
            {
                ClaimEvent(UserID, value);
            }
        }

        public UserValidatedEvent() : base() { }

        public UserValidatedEvent(Guid eventIdentifier, Guid aggregateRootId, long eventSequence, DateTime eventTimeStamp) 
            : base(eventIdentifier, aggregateRootId, eventSequence, eventTimeStamp) { }

        public Guid GetID()
        {
            return UserID;
        }
    }
}
