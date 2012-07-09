using ProtoBuf;

namespace Domain
{
    [ProtoContract]
    public class UserProperty
    {
        [ProtoMember(1)]public string Name { get; set; }
        [ProtoMember(2)]public string Value { get; set; }
        [ProtoMember(3)]public string Type { get; set; }
        [ProtoMember(4)]public string Format { get; set; }

        public UserProperty() { }

        public UserProperty(string name, string value, string type, string format)
        {
            Name = name;
            Value = value;
            Type = type;
            Format = format;
        }
    }
}
