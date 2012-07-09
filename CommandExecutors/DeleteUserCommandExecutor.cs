using Commands;
using Domain;
using Ncqrs.Commanding.CommandExecution;
using Ncqrs.Domain;

namespace CommandExecutors
{
    public class DeleteUserCommandExecutor : CommandExecutorBase<DeleteUserCommand>
    {
        protected override void ExecuteInContext(IUnitOfWorkContext context, DeleteUserCommand command)
        {
            var aggregate = context.GetById<User>(command.UserID) as User;

            aggregate.Delete(command.UserID);

            context.Accept();
        }
    }
}
