using System.IO;
using Ncqrs.Eventing.Sourcing;

namespace Eventing
{
    public class EventStreamer : IEventStreamer
    {
        private readonly EventSerializer serializer;

        public EventStreamer(EventSerializer serializer)
        {
            this.serializer = serializer;
        }

        public byte[] SerializeEvent(ISourcedEvent e)
        {
            byte[] content;
            using (var ms = new MemoryStream())
            {
                serializer.Serialize(e, e.GetType(), ms);
                content = ms.ToArray();
            }

            byte[] messageContractBuffer;
            using (var ms = new MemoryStream())
            {
                var name = e.GetType().GetContractName();
                var messageContract = new MessageContract(name, content.Length, 0, e.EventIdentifier, e.EventSequence);
                serializer.Serialize(messageContract, typeof(MessageContract), ms);
                messageContractBuffer = ms.ToArray();
            }

            using (var ms = new MemoryStream())
            {
                var headerContract = new MessageHeaderContract(messageContractBuffer.Length);
                headerContract.WriteHeader(ms);
                ms.Write(messageContractBuffer, 0, messageContractBuffer.Length);
                ms.Write(content, 0, content.Length);
                return ms.ToArray();
            }
        }

        public ISourcedEvent DeserializeEvent(byte[] buffer)
        {
            using (var ms = new MemoryStream(buffer))
            {
                var header = MessageHeaderContract.ReadHeader(buffer);
                ms.Seek(MessageHeaderContract.FixedSize, SeekOrigin.Begin);

                var headerBuffer = new byte[header.HeaderBytes];
                ms.Read(headerBuffer, 0, (int)header.HeaderBytes);
                var contract = (MessageContract)serializer.Deserialize(
                    new MemoryStream(headerBuffer), typeof(MessageContract));

                var contentBuffer = new byte[contract.ContentSize];
                ms.Read(contentBuffer, 0, (int)contract.ContentSize);
                var contentType = serializer.GetContentType(contract.ContractName);
                var @event = (ISourcedEvent)serializer.Deserialize(new MemoryStream(contentBuffer), contentType);

                return @event;
            }
        }
    }
}
