using System;
using Events;
using System.Collections.Generic;
using ProtoBuf;
using System.IO;
using System.Linq;
using Ncqrs.Eventing.ServiceModel.Bus;

namespace ReadModel.Denormalizers
{
    public class FileTweetIndexItemDenormalizer : IEventHandler<TweetPostedEvent>
    {
        public void Handle(IPublishedEvent<TweetPostedEvent> evnt)
        {
            Handle(evnt.Payload);
        }

        public void Handle(TweetPostedEvent evnt)
        {
            var store = Ncqrs.NcqrsEnvironment.Get<IReadModelStore>();

            var model = store.GetOrCreate<FileTweetIndexReadModel>();

            var items = model.Get("items") as List<TweetIndexItem>;

            items.Add(new TweetIndexItem()
            {
                Id = evnt.EventSourceId,
                Message = evnt.Message,
                Who = evnt.Who,
                TimeStamp = evnt.TimeStamp,
                Channel = evnt.Channel
            });

            store.Save(model);
        }

        public void DenormalizeEvent(TweetPostedEvent evnt)
        {
            Handle(evnt);
        }
    }

    public class FileTweetIndexItemDeleteDenormalizer : IEventHandler<TweetDeletedEvent>
    {
        public void Handle(IPublishedEvent<TweetDeletedEvent> evnt)
        {
            Handle(evnt.Payload);
        }

        public void Handle(TweetDeletedEvent evnt)
        {
            var store = Ncqrs.NcqrsEnvironment.Get<IReadModelStore>();

            var model = store.GetReadModel<FileTweetIndexReadModel>();

            var items = model.Get("items") as List<TweetIndexItem>;

            var toDelete = items.Where(n => n.Id == evnt.TweetID).ToList();


            foreach (var item in toDelete)
            {
                items.Remove(item);
            }

            store.Save(model);
        }

        public void DenormalizeEvent(TweetDeletedEvent evnt)
        {
            Handle(evnt);
        }
    }

}
