using Commands;
using Domain;
using Ncqrs.Commanding.CommandExecution;
using Ncqrs.Domain;

namespace CommandExecutors
{
    public class SetUserPropertyCommandExecutor : CommandExecutorBase<SetUserPropertyCommand>
    {
        protected override void ExecuteInContext(IUnitOfWorkContext context, SetUserPropertyCommand command)
        {
            var aggregate = context.GetById<User>(command.UserID) as User;

            aggregate.SetProperty(command.UserID, command.Name, command.Value);

            context.Accept();
        }
    }
}
