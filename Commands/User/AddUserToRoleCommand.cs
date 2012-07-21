using System;
using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
    public class AddUserToRoleCommand : CommandBase
    {
        [AggregateRootId]
        public Guid UserID { get; set; }
        public string Role { get; set; }
    }
}
