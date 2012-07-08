using System;
using Ncqrs.Domain;
using ProtoBuf;
using Ncqrs.Eventing;
using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    [ProtoContract]
    public class UserDeletedEvent : SourcedEvent, IIdentifiable
    {
        [ProtoMember(1)]public Guid UserID { get; set; }
        [ProtoMember(2)]public DateTime TimeStamp { get; set; }
        [ProtoMember(3)]
        public long Sequence
        {
            get
            {
                return this.EventSequence;
            }
            set
            {
                this.ClaimEvent(UserID, value);
            }
        }

        public UserDeletedEvent() : base() { }

        public UserDeletedEvent(Guid eventIdentifier, Guid aggregateRootId, long eventSequence, DateTime eventTimeStamp) 
            : base(eventIdentifier, aggregateRootId, eventSequence, eventTimeStamp) { }

        public Guid GetID()
        {
            return UserID;
        }
    }
}
