using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;
using System;

namespace Commands
{
    [MapsToAggregateRootConstructor("Domain.User, Domain")]
    public class InvalidateUserCommand : CommandBase
    {
        public Guid UserID { get; set; }
    }
}
