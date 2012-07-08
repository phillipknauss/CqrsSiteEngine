using System;
using System.ServiceModel;
using ReadModel;
using System.Collections.Generic;

namespace ReadModelService
{
    [ServiceContract]
    public interface ISimpleTwitterReadModelService
    {
        [OperationContract]
        IEnumerable<TweetIndexItem> GetTweets();

        [OperationContract]
        IEnumerable<ChannelIndexItem> GetChannels();

        [OperationContract]
        IEnumerable<UserIndexItem> GetUsers();
    }
}
