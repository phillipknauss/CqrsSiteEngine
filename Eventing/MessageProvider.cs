using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ncqrs.Eventing.Sourcing;

namespace Eventing
{
    public static class MessagesProvider
    {
        public static Type[] GetKnownEventTypes()
        {
            var types = Assembly.GetAssembly(typeof(Events.TweetPostedEvent))
                .GetTypes()
                .Where(t => typeof(ISourcedEvent) == t && t.IsAbstract == false)
                .Union(new[] { typeof(MessageContract) })
                .ToArray();

            types = new List<Type>
                        {
                            typeof (Events.TweetPostedEvent),
                            typeof (Events.TweetDeletedEvent),
                            typeof (Events.ChannelCreatedEvent),
                            typeof (Events.TweetAddedToChannelEvent),
                            typeof (Events.UserCreatedEvent),
                            typeof (Events.UserDeletedEvent),
                            typeof (Events.UserPropertySetEvent),
                            typeof (Events.UserPasswordSetEvent),
                            typeof(Events.UserValidatedEvent),
                            typeof(Events.UserInvalidatedEvent),
                            typeof(Events.UserAddedToRoleEvent),
                            typeof(Events.UserRemovedFromRoleEvent),
                            typeof (MessageContract)
                        }.ToArray();

            return types;
        }
    }
}
