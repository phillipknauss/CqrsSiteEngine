using System;
using System.Collections.Generic;
using ProtoBuf;

namespace ReadModel
{
    [ProtoContract]
    public class ChannelIndexItem 
    {
        [ProtoMember(1)]
        public Guid Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public DateTime TimeStamp { get; set; }
    }

    [ProtoContract]
    public class ChannelIndexReadModel : IReadModel
    {
        [ProtoMember(1)]public List<ChannelIndexItem> Items { get; set; }

        public ChannelIndexReadModel()
        {
            Items = new List<ChannelIndexItem>();
        }

        public object Get(string identifier)
        {
            return identifier != "items" ? null : Items;
        }
    }
}
