using System;
using Ncqrs.Domain;
using ProtoBuf;
using Ncqrs.Eventing;
using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    [ProtoContract]
    public class UserPropertySetEvent : SourcedEvent, IIdentifiable
    {

        [ProtoMember(1)]
        public Guid UserID { get; set; }
        [ProtoMember(2)]public string Name { get; set; }
        [ProtoMember(3)]public string Value { get; set; }
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
                this.ClaimEvent(UserID, value);
            }
        }

        [ProtoMember(6)]
        public string Type { get; set; }
        [ProtoMember(7)]
        public string Format { get; set; }
        
        
        public UserPropertySetEvent() : base(Guid.NewGuid(), Guid.Empty, 1, DateTime.UtcNow)
        {
        }

        public UserPropertySetEvent(Guid guid, Guid guid_2, long p, DateTime dateTime) : base(guid, guid_2, p, dateTime)
        {
        }

        public Guid GetID()
        {
            return UserID;
        }
    }
}
