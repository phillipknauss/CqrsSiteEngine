using System;
using Ncqrs.Commanding.CommandExecution;
using Commands;
using Ncqrs.Domain;
using Domain;

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
