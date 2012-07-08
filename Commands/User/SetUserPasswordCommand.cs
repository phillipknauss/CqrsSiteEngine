using System;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
    [MapsToAggregateRootMethod("Domain.User", "SetPassword")]
    public class SetUserPasswordCommand : CommandBase
    {
        [AggregateRootId]
        public Guid UserID { get; set; }
        public string Password { get; set; }
    }
}
