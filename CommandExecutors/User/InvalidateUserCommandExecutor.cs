using Commands;
using Domain;
using Ncqrs.Commanding.CommandExecution;
using Ncqrs.Domain;

namespace CommandExecutors
{
    public class InvalidateUserCommandExecutor : CommandExecutorBase<InvalidateUserCommand>
    {
        protected override void ExecuteInContext(IUnitOfWorkContext context, InvalidateUserCommand command)
        {
            var aggregate = context.GetById<User>(command.UserID) as User;
            aggregate.Invalidate(command.UserID);
            context.Accept();
        }
    }
}
