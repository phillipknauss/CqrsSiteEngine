using System;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
    [MapsToAggregateRootMethod("Domain.User", "SetProperty")]
    public class SetUserPropertyCommand : CommandBase
    {
        [AggregateRootId]
        public Guid UserID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
