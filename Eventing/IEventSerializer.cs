using System;
namespace Eventing
{
    interface IEventSerializer
    {
        object Deserialize(System.IO.Stream sourceStream, Type type);
        Type GetContentType(string contractName);
        void Serialize(object instance, Type type, System.IO.Stream destinationStream);
    }
}
