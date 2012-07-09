using System;
using ProtoBuf;
using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    [ProtoContract]
    public class TweetPostedEvent : SourcedEvent, IIdentifiable
    {
        [ProtoMember(1)]public string Message { get; set; }
        [ProtoMember(2)]public string Who { get; set; }
        [ProtoMember(3)]public DateTime TimeStamp { get; set; }
        [ProtoMember(4)]
        public Guid Channel { get; set; }
        [ProtoMember(5)]
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

        public TweetPostedEvent() : base(Guid.NewGuid(), Guid.Empty, 1, DateTime.UtcNow)
        {
        }

        public Guid GetID()
        {
            return EventSourceId;
        }
    }
}
