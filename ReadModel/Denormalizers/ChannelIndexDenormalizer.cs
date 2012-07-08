using System;
using Events;
using System.Collections.Generic;
using ProtoBuf;
using System.IO;
using System.Linq;
using Ncqrs.Eventing.ServiceModel.Bus;

namespace ReadModel.Denormalizers
{
    public class ChannelIndexItemDenormalizer : IEventHandler<ChannelCreatedEvent>
    {
        public void Handle(IPublishedEvent<ChannelCreatedEvent> evnt)
        {
            Handle(evnt.Payload);
        }

        public void Handle(ChannelCreatedEvent evnt)
        {
            var store = Ncqrs.NcqrsEnvironment.Get<IReadModelStore>();

            var model = store.GetOrCreate<ChannelIndexReadModel>();

            var items = model.Get("items") as List<ChannelIndexItem>;

            items.Add(new ChannelIndexItem()
            {
                Id = evnt.ID,
                Name = evnt.Name,
                TimeStamp = evnt.TimeStamp
            });

            store.Save(model);
        }

        public void DenormalizeEvent(ChannelCreatedEvent evnt)
        {
            Handle(evnt);
        }
    }

}
