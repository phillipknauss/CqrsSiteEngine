using System;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
    [MapsToAggregateRootMethod("Domain.User", "Delete")]
    public class DeleteUserCommand : CommandBase
    {
        [AggregateRootId]
        public Guid UserID { get; set; }
        public string Who { get; set; }
    }
}
