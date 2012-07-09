using Ncqrs.Eventing.Sourcing;

namespace Eventing
{
    public interface IEventStreamer
    {
        byte[] SerializeEvent(ISourcedEvent e);
        ISourcedEvent DeserializeEvent(byte[] buffer);
    }
}
