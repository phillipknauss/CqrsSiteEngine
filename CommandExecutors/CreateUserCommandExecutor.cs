using System;
using Ncqrs.Commanding.CommandExecution;
using Commands;
using Ncqrs.Domain;
using Domain;
using System.Security.Cryptography;
using System.Text;

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
