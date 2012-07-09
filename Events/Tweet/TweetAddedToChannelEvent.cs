using System;
using ProtoBuf;
using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    [ProtoContract]
    public class TweetAddedToChannelEvent : SourcedEvent, IIdentifiable
    {
        [ProtoMember(1)]public Guid Channel { get; set; }
        [ProtoMember(2)]public DateTime TimeStamp { get; set; }
        [ProtoMember(3)]
        public long Sequence
        {
            get
            {
                return 1;
            }
            set
            {
                return;
            }
        }

        public TweetAddedToChannelEvent() : base(Guid.NewGuid(), Guid.Empty, 1, DateTime.UtcNow)
        {
        }

        public Guid GetID()
        {
            return EventSourceId;
        }
    }
}
