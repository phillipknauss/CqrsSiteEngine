using System;
using ProtoBuf;
using Ncqrs.Eventing.Sourcing;

namespace Events
{
    [Serializable]
    [ProtoContract]
    public class UserPasswordSetEvent : SourcedEvent, IIdentifiable
    {

        [ProtoMember(1)]
        public Guid UserID { get; set; }
        [ProtoMember(2)]public string Password { get; set; }
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

        
        public UserPasswordSetEvent() : base(Guid.NewGuid(), Guid.Empty, 1, DateTime.UtcNow)
        {
        }

        public UserPasswordSetEvent(Guid guid, Guid guid_2, long p, DateTime dateTime)
            : base(guid, guid_2, p, dateTime)
        {
        }

        public Guid GetID()
        {
            return UserID;
        }
    }
}
