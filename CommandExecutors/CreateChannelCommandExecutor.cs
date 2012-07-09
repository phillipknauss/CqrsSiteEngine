using System;
using Commands;
using Domain;
using Ncqrs.Commanding.CommandExecution;
using Ncqrs.Domain;

namespace CommandExecutors
{
    public class CreateChannelCommandExecutor : CommandExecutorBase<CreateNewChannelCommand>
    {
        protected override void ExecuteInContext(IUnitOfWorkContext context, CreateNewChannelCommand command)
        {
            var newChannel = new Channel(Guid.NewGuid(), command.Name);
            context.Accept();
        }
    }
}
