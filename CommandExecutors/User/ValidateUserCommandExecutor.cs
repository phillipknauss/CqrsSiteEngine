using Commands;
using Domain;
using Ncqrs.Commanding.CommandExecution;
using Ncqrs.Domain;

namespace CommandExecutors
{
    public class ValidateUserCommandExecutor : CommandExecutorBase<ValidateUserCommand>
    {
        protected override void ExecuteInContext(IUnitOfWorkContext context, ValidateUserCommand command)
        {
            var aggregate = context.GetById<User>(command.UserID) as User;

            bool validated = aggregate.Validate(command.UserID, command.Username, command.Password);

            if (validated)
            {
                aggregate.Validated(command.UserID);
                context.Accept();
            }
        }
    }
}
