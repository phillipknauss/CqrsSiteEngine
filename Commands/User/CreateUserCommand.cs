using Ncqrs.Commanding;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Commands
{
    [MapsToAggregateRootConstructor("Domain.User, Domain")]
    public class CreateUserCommand : CommandBase
    {
        public string Username { get; set; }
    }
}
