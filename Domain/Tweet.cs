using System;
using Ncqrs.Domain;
using Events;

namespace Domain
{
    public class Tweet : AggregateRootMappedByConvention 
    {
        private string _message;
        private Guid _channel;
        private string _who;
        private DateTime _timestamp;
        private TweetState _tweetState;

        public Tweet() { }

        public Tweet(string message, Guid channel, string who)
        {
            var e = new TweetPostedEvent
            {
                Message = message,
                Who = who,
                TimeStamp = DateTime.UtcNow,
                Channel = channel
            };

            ApplyEvent(e);
        }

        public void Delete(Guid tweetID)
        {
            var e = new TweetDeletedEvent(Guid.NewGuid(), Guid.Empty, this.InitialVersion+1, DateTime.UtcNow)
            {
                TweetID = tweetID,
                TimeStamp = DateTime.UtcNow
            };

            ApplyEvent(e);
        }

        protected void OnTweetPosted(TweetPostedEvent e)
        {
            _message = e.Message;
            _who = e.Who;
            _timestamp = e.TimeStamp;
            _channel = e.Channel;
        }

        protected void OnTweetDeleted(TweetDeletedEvent e)
        {
            _timestamp = e.TimeStamp;
            _tweetState = TweetState.Deleted;
        }

        protected void OnTweetAddedToChannel(TweetAddedToChannelEvent e)
        {
            
        }
    }

    public enum TweetState
    {
        Normal=0,
        Deleted=1
    }
}
