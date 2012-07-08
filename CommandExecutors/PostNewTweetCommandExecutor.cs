using System;
using Ncqrs.Commanding.CommandExecution;
using Commands;
using Ncqrs.Domain;
using Domain;

namespace CommandExecutors
{
    public class PostNewTweetCommandExecutor : CommandExecutorBase<PostNewTweetCommand>
    {
        protected override void ExecuteInContext(IUnitOfWorkContext context, PostNewTweetCommand command)
        {
            var newTweet = new Tweet(command.Message, Guid.Parse(command.Channel), command.Who);

            context.Accept();
        }
    }
}
