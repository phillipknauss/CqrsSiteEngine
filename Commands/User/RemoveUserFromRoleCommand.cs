using System;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
    public class RemoveUserFromRoleCommand : CommandBase
    {
        [AggregateRootId]
        public Guid UserID { get; set; }
        public string Role { get; set; }
    }
}
