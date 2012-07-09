using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
    [MapsToAggregateRootConstructor("Domain.Tweet, Domain")]
    public class PostNewTweetCommand : CommandBase
    {
        public string Message { get; set; }
        public string Channel { get; set; }
        public string Who { get; set; }
    }
}
