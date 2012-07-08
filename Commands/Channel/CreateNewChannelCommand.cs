using System;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
    [MapsToAggregateRootConstructor("Domain.Channel, Domain")]
    public class CreateNewChannelCommand : CommandBase
    {
        public string Name { get; set; }
    }
}
