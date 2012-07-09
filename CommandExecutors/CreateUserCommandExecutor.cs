using Commands;
using Domain;
using Ncqrs.Commanding.CommandExecution;
using Ncqrs.Domain;

namespace CommandExecutors
{
    public class CreateUserCommandExecutor : CommandExecutorBase<CreateUserCommand>
    {
        protected override void ExecuteInContext(IUnitOfWorkContext context, CreateUserCommand command)
        {
            var newUser = new User(command.Username);

            context.Accept();
        }
    }
}
