using System.Collections.Generic;
using System.Linq;
using ReadModel;

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

        static SimpleTwitterReadModelService()
        {
            Bootstrapper.BootUp();
        }
    }
}