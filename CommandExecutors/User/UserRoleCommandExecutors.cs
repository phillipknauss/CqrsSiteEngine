using Commands;
using Domain;
using Ncqrs.Commanding.CommandExecution;
using Ncqrs.Domain;

namespace CommandExecutors
{
    public class AddUserToRoleCommandExecutor : CommandExecutorBase<AddUserToRoleCommand>
    {
        protected override void ExecuteInContext(IUnitOfWorkContext context, AddUserToRoleCommand command)
        {
            var aggregate = context.GetById<User>(command.UserID) as User;

            aggregate.AddToRole(command.UserID, command.Role);

            context.Accept();
        }
    }

    public class RemoveUserFromRoleCommandExecutor : CommandExecutorBase<RemoveUserFromRoleCommand>
    {
        protected override void ExecuteInContext(IUnitOfWorkContext context, RemoveUserFromRoleCommand command)
        {
            var aggregate = context.GetById<User>(command.UserID) as User;

            aggregate.RemoveFromRole(command.UserID, command.Role);

            context.Accept();
        }
    }
}
