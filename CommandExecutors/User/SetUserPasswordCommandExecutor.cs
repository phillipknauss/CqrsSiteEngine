using System;
using System.Security.Cryptography;
using System.Text;
using Commands;
using Domain;
using Ncqrs.Commanding.CommandExecution;
using Ncqrs.Domain;

namespace CommandExecutors
{
    public class SetUserPasswordCommandExecutor : CommandExecutorBase<SetUserPasswordCommand>
    {
        protected override void ExecuteInContext(IUnitOfWorkContext context, SetUserPasswordCommand command)
        {
            var aggregate = context.GetById<User>(command.UserID) as User;

            aggregate.SetPassword(command.UserID, command.Password);

            context.Accept();
        }
    }
}
