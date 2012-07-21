using Commands;
using Ncqrs;
using Ncqrs.Commanding.ServiceModel;

namespace CommandServiceLibrary
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

        public void ValidateUser(ValidateUserCommand command)
        {
            var service = NcqrsEnvironment.Get<ICommandService>();
            service.Execute(command);
        }

        public void InvalidateUser(InvalidateUserCommand command)
        {
            var service = NcqrsEnvironment.Get<ICommandService>();
            service.Execute(command);
        }

        public void AddUserToRole(AddUserToRoleCommand command)
        {
            var service = NcqrsEnvironment.Get<ICommandService>();
            service.Execute(command);
        }

        public void RemoveUserFromRole(RemoveUserFromRoleCommand command)
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