using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProtoBuf.Meta;

namespace Eventing
{
    public class EventSerializer : Eventing.IEventSerializer
    {
        readonly IDictionary<Type, Formatter> _type2Contract = new Dictionary<Type, Formatter>();
        readonly IDictionary<string, Type> _contractName2Type = new Dictionary<string, Type>();


        protected sealed class Formatter
        {
            public Action<object, Stream> SerializeDelegate;
            public Func<Stream, object> DeserializeDelegate;
            public string ContractName;

            public Formatter(string name, Func<Stream, object> deserializerDelegate, Action<object, Stream> serializeDelegate)
            {
                ContractName = name;
                DeserializeDelegate = deserializerDelegate;
                SerializeDelegate = serializeDelegate;
            }
        }

        public EventSerializer(IEnumerable<Type> knownEventTypes)
        {
            _type2Contract = knownEventTypes
                .ToDictionary(
                    t => t,
                    t =>
                    {
                        var formatter = RuntimeTypeModel.Default.CreateFormatter(t);
                        return new Formatter(
                            t.GetContractName(), formatter.Deserialize,
                            (o, stream) => formatter.Serialize(stream, o));
                    });
            _contractName2Type = knownEventTypes
                .ToDictionary(
                    t => t.GetContractName(),
                    t => t);


        }

        public void Serialize(object instance, Type type, Stream destinationStream)
        {
            Formatter formatter;
            if (!_type2Contract.TryGetValue(type, out formatter))
            {
                var s =
                    string.Format("Cant' find serializer for unknown type '{0}'.", instance.GetType());
                throw new InvalidOperationException(s);
            }
            formatter.SerializeDelegate(instance, destinationStream);
        }

        public Type GetContentType(string contractName)
        {
            return _contractName2Type[contractName];
        }

        public object Deserialize(Stream sourceStream, Type type)
        {
            Formatter value;
            if (!_type2Contract.TryGetValue(type, out value))
            {
                var s =
                    string.Format("Can't find formatter for unknown object type '{0}'.", type);
                throw new InvalidOperationException(s);
            }
            return value.DeserializeDelegate(sourceStream);
        }
    }
}
