using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ncqrs.Eventing.Sourcing;

namespace Eventing
{
    public interface IEventStreamer
    {
        byte[] SerializeEvent(ISourcedEvent e);
        ISourcedEvent DeserializeEvent(byte[] buffer);
    }
}
