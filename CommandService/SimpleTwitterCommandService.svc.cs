using System;
using Ncqrs;
using Ncqrs.Commanding.ServiceModel;
using Commands;
using Ncqrs.Eventing.Storage;
using Eventing;
using ReadModel;
using Ncqrs.Commanding;

namespace CommandService
{
    public class SimpleTwitterCommandService : ISimpleTwitterCommandService
    {

        #region Tweets

        public void Execute(PostNewTweetCommand command)
        {
            var service = NcqrsEnvironment.Get<ICommandService>();
            service.Execute(command);
        }

        public void Delete(DeleteTweetCommand command)
        {
            var service = NcqrsEnvironment.Get<ICommandService>();
            service.Execute(command);
        }

        #endregion

        #region Channels

        public void CreateNewChannel(CreateNewChannelCommand command)
        {
            var service = NcqrsEnvironment.Get<ICommandService>();
            service.Execute(command);
        }

        #endregion

        #region Users

        public void CreateUser(CreateUserCommand command)
        {
            var service = NcqrsEnvironment.Get<ICommandService>();
            service.Execute(command);
        }

        public void DeleteUser(DeleteUserCommand command)
        {
            var service = NcqrsEnvironment.Get<ICommandService>();
            service.Execute(command);
        }

        public void SetUserProperty(SetUserPropertyCommand command)
        {
            var service = NcqrsEnvironment.Get<ICommandService>();
            service.Execute(command);
        }

        public void SetUserPassword(SetUserPasswordCommand command)
        {
            var service = NcqrsEnvironment.Get<ICommandService>();
            service.Execute(command);
        }

        #endregion

        static SimpleTwitterCommandService()
        {
            Bootstrapper.BootUp();
        }
    }
}
