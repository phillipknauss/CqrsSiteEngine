using System;
using System.Collections.Generic;
using ProtoBuf;

namespace ReadModel
{
    [ProtoContract]
    public class TweetIndexItem 
    {
        [ProtoMember(1)]
        public Guid Id { get; set; }
        [ProtoMember(2)]
        public string Message { get; set; }
        [ProtoMember(3)]
        public string Who { get; set; }
        [ProtoMember(4)]
        public DateTime TimeStamp { get; set; }
        [ProtoMember(5)]
        public Guid Channel { get; set; }
    }

    [ProtoContract]
    public class FileTweetIndexReadModel : IReadModel
    {
        [ProtoMember(1)]public List<TweetIndexItem> Items { get; set; }

        public FileTweetIndexReadModel()
        {
            Items = new List<TweetIndexItem>();
        }

        public object Get(string identifier)
        {
            return identifier != "items" ? null : Items;
        }
    }
}
