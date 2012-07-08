using System;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
    [MapsToAggregateRootMethod("Domain.Tweet", "Delete")]
    public class DeleteTweetCommand : CommandBase
    {
        [AggregateRootId]
        public Guid TweetID { get; set; }
        public string Who { get; set; }
    }
}
