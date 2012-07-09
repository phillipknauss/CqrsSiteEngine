using System;
using Commands;
using Domain;
using Ncqrs.Commanding.CommandExecution;
using Ncqrs.Domain;

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
