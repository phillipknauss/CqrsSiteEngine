using System;
using Ncqrs.Commanding.CommandExecution;
using Commands;
using Ncqrs.Domain;
using Domain;

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
