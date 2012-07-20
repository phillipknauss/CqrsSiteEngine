using System.Collections.Generic;
using System.Linq;
using ReadModel;
using System;

namespace ReadModelServiceLibrary
{
    public class SimpleTwitterReadModelService : ISimpleTwitterReadModelService
    {
        public IEnumerable<TweetIndexItem> GetTweets()
        {
            var store = Ncqrs.NcqrsEnvironment.Get<ReadModel.IReadModelStore>();
            var readModel = store.GetOrCreate<ReadModel.FileTweetIndexReadModel>();
            var query = readModel.Get("items");

            var sorted = (query as IEnumerable<TweetIndexItem>).OrderByDescending(n => n.TimeStamp);

            return sorted;
        }

        public IEnumerable<ChannelIndexItem> GetChannels()
        {
            var store = Ncqrs.NcqrsEnvironment.Get<ReadModel.IReadModelStore>();
            var readModel = store.GetOrCreate<ReadModel.ChannelIndexReadModel>();
            var query = readModel.Get("items");

            var sorted = (query as IEnumerable<ChannelIndexItem>).OrderBy(n => n.Name);

            return sorted;
        }

        public IEnumerable<UserIndexItem> GetUsers()
        {
            var store = Ncqrs.NcqrsEnvironment.Get<ReadModel.IReadModelStore>();
            var readModel = store.GetOrCreate<ReadModel.UserIndexReadModel>();
            var query = readModel.Get("items");

            var sorted = (query as IEnumerable<UserIndexItem>).OrderBy(n => n.Username);

            return sorted;
        }

        public bool UserValidated(Guid UserID)
        {
            var store = Ncqrs.NcqrsEnvironment.Get<ReadModel.IReadModelStore>();
            var readModel = store.GetOrCreate<ReadModel.UserIndexReadModel>();
            var query = readModel.Get("items");

            var enumerable = query as IEnumerable<UserIndexItem>;
            var user = enumerable.Where(n => n.Id == UserID).SingleOrDefault();

            if (user == null)
            {
                return false;
            }

            return user.Authenticated;
            
        }

        static SimpleTwitterReadModelService()
        {
            Bootstrapper.BootUp();
        }
    }
}