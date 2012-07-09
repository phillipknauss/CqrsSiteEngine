using System;
using System.Runtime.Serialization;

namespace Eventing
{
    [DataContract]
    public sealed class MessageContract
    {
        [DataMember(Order = 1)]
        public readonly string ContractName;
        [DataMember(Order = 2)]
        public readonly long ContentSize;
        [DataMember(Order = 3)]
        public readonly long ContentPosition;
        [DataMember(Order = 4)]
        public Guid EventIdentifier;
        [DataMember(Order = 5)]
        public long Sequence;

        MessageContract() { }

        public MessageContract(string contractName, long contentSize, long contentPosition, Guid eventIdentifier, long sequence)
        {
            ContractName = contractName;
            ContentSize = contentSize;
            ContentPosition = contentPosition;
            EventIdentifier = eventIdentifier;
            Sequence = sequence;
        }
    }
}
