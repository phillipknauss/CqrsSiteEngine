using System;
using ProtoBuf;
using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    [ProtoContract]
    public class TweetDeletedEvent : SourcedEvent, IIdentifiable
    {
        [ProtoMember(1)]public Guid TweetID { get; set; }
        [ProtoMember(2)]public string Who { get; set; }
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
                ClaimEvent(TweetID, value);
            }
        }

        public TweetDeletedEvent() : base() { }

        public TweetDeletedEvent(Guid eventIdentifier, Guid aggregateRootId, long eventSequence, DateTime eventTimeStamp) 
            : base(eventIdentifier, aggregateRootId, eventSequence, eventTimeStamp) { }

        public Guid GetID()
        {
            return TweetID;
        }
    }
}
