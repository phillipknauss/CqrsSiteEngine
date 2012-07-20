using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;
using System;

namespace Commands
{
    [MapsToAggregateRootConstructor("Domain.User, Domain")]
    public class ValidateUserCommand : CommandBase
    {
        public Guid UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
