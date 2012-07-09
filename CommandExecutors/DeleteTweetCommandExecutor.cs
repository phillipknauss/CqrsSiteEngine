using Commands;
using Domain;
using Ncqrs.Commanding.CommandExecution;
using Ncqrs.Domain;

namespace CommandExecutors
{
    public class DeleteTweetCommandExecutor : CommandExecutorBase<DeleteTweetCommand>
    {
        protected override void ExecuteInContext(IUnitOfWorkContext context, DeleteTweetCommand command)
        {
            var aggregate = context.GetById<Tweet>(command.TweetID) as Tweet;

            aggregate.Delete(command.TweetID);

            context.Accept();
        }
    }
}
