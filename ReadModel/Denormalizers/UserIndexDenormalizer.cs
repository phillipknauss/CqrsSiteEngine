using System.Collections.Generic;
using System.Linq;
using Events;
using Ncqrs.Eventing.ServiceModel.Bus;

namespace ReadModel.Denormalizers
{
    public class UserIndexDenormalizer : IEventHandler<UserCreatedEvent>
    {
        public void Handle(IPublishedEvent<UserCreatedEvent> evnt)
        {
            Handle(evnt.Payload);
        }

        public void Handle(UserCreatedEvent evnt)
        {
            var store = Ncqrs.NcqrsEnvironment.Get<IReadModelStore>();

            var model = store.GetOrCreate<UserIndexReadModel>();

            var items = model.Get("items") as List<UserIndexItem>;

            items.Add(new UserIndexItem
                          {
                              Id = evnt.EventSourceId,
                              Username = evnt.Name,
                              TimeStamp = evnt.TimeStamp
                          });

            store.Save(model);
        }

        public void DenormalizeEvent(UserCreatedEvent evnt)
        {
            Handle(evnt);
        }
    }

    public class UserIndexDeleteDenormalizer : IEventHandler<UserDeletedEvent>
    {
        public void Handle(IPublishedEvent<UserDeletedEvent> evnt)
        {
            Handle(evnt.Payload);
        }
        public void Handle(UserDeletedEvent evnt)
        {
            var store = Ncqrs.NcqrsEnvironment.Get<IReadModelStore>();

            var model = store.GetReadModel<UserIndexReadModel>();

            var items = model.Get("items") as List<UserIndexItem>;

            var toDelete = items.Where(n => n.Id == evnt.UserID).ToList();
            
            foreach (var item in toDelete)
            {
                items.Remove(item);
            }

            store.Save(model);
        }

        public void DenormalizeEvent(UserDeletedEvent evnt)
        {
            Handle(evnt);
        }
    }

    public class UserIndexPasswordSetDenormalizer : IEventHandler<UserPasswordSetEvent>
    {
        public void Handle(IPublishedEvent<UserPasswordSetEvent> evnt)
        {
            Handle(evnt.Payload);
        }
        public void Handle(UserPasswordSetEvent evnt)
        {
            // No need to do anything, the view doesn't need to see when password is changed

            //var store = Ncqrs.NcqrsEnvironment.Get<IReadModelStore>();

            //var model = store.GetReadModel<UserIndexReadModel>();

            //var items = model.Get("items") as List<UserIndexItem>;

            //var item = items.Where(n => n.Id == evnt.UserID).SingleOrDefault();

            //if (item == null) { return; }
            
            //store.Save(model);
        }

        public void DenormalizeEvent(UserPasswordSetEvent evnt)
        {
            Handle(evnt);
        }
    }

    public class UserIndexPropertySetDenormalizer : IEventHandler<UserPropertySetEvent>
    {
        public void Handle(IPublishedEvent<UserPropertySetEvent> evnt)
        {
            var store = Ncqrs.NcqrsEnvironment.Get<IReadModelStore>();

            var model = store.GetReadModel<UserIndexReadModel>();

            var items = model.Get("items") as List<UserIndexItem>;

            var item = items.SingleOrDefault(n => n.Id == evnt.Payload.UserID);

            if (item == null)
            {
                return;
            }

            if (!item.Properties.ContainsKey(evnt.Payload.Name))
            {
                item.Properties.Add(evnt.Payload.Name,
                                    new UserProperty
                                        {
                                            Name = evnt.Payload.Name,
                                            Value = evnt.Payload.Value,
                                            Type = evnt.Payload.Type,
                                            Format = evnt.Payload.Format
                                        });
            }

            item.Properties[evnt.Payload.Name] =
                new UserProperty
                    {
                        Name = evnt.Payload.Name,
                        Value = evnt.Payload.Value,
                        Type = evnt.Payload.Type,
                        Format = evnt.Payload.Format
                    };

            store.Save(model);
        }
    }
}
