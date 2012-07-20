using System.ServiceModel;
using Commands;

namespace CommandServiceLibrary
{
    [ServiceContract]
    public interface ISimpleTwitterCommandService
    {

        #region Tweets

        [OperationContract]
        void Execute(PostNewTweetCommand command); // todo: Should be authenticated-only

        [OperationContract]
        void Delete(DeleteTweetCommand command); // todo: Should be owner-or-admin only

        #endregion

        #region Channels

        [OperationContract]
        void CreateNewChannel(CreateNewChannelCommand command); // todo: Should be authenticated-only

        #endregion

        #region Users

        [OperationContract]
        void CreateUser(CreateUserCommand command);

        [OperationContract]
        void DeleteUser(DeleteUserCommand command);

        [OperationContract]
        void SetUserProperty(SetUserPropertyCommand command);

        [OperationContract]
        void SetUserPassword(SetUserPasswordCommand command);

        [OperationContract]
        void ValidateUser(ValidateUserCommand command);

        [OperationContract]
        void InvalidateUser(InvalidateUserCommand command);

        #endregion
    }
}
