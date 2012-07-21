using System;
using ProtoBuf;
using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    [ProtoContract]
    public class UserAddedToRoleEvent : SourcedEvent, IIdentifiable
    {
        [ProtoMember(1)]public Guid UserID { get; set; }
        [ProtoMember(2)]
        public string Role { get; set; }
        [ProtoMember(3)]public DateTime TimeStamp { get; set; }
        [ProtoMember(4)]
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

        public UserAddedToRoleEvent() : base() { }

        public UserAddedToRoleEvent(Guid eventIdentifier, Guid aggregateRootId, long eventSequence, DateTime eventTimeStamp) 
            : base(eventIdentifier, aggregateRootId, eventSequence, eventTimeStamp) { }

        public Guid GetID()
        {
            return UserID;
        }
    }

    [Serializable]
    [ProtoContract]
    public class UserRemovedFromRoleEvent : SourcedEvent, IIdentifiable
    {
        [ProtoMember(1)]
        public Guid UserID { get; set; }
        [ProtoMember(2)]
        public string Role { get; set; }
        [ProtoMember(3)]
        public DateTime TimeStamp { get; set; }
        [ProtoMember(4)]
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

        public UserRemovedFromRoleEvent() : base() { }

        public UserRemovedFromRoleEvent(Guid eventIdentifier, Guid aggregateRootId, long eventSequence, DateTime eventTimeStamp)
            : base(eventIdentifier, aggregateRootId, eventSequence, eventTimeStamp) { }

        public Guid GetID()
        {
            return UserID;
        }
    }
}
