using System;
using Ncqrs.Commanding.CommandExecution;
using Commands;
using Ncqrs.Domain;
using Domain;
using System.Security.Cryptography;
using System.Text;

namespace CommandExecutors
{
    public class SetUserPasswordCommandExecutor : CommandExecutorBase<SetUserPasswordCommand>
    {
        protected override void ExecuteInContext(IUnitOfWorkContext context, SetUserPasswordCommand command)
        {
            var aggregate = context.GetById<User>(command.UserID) as User;

            aggregate.SetPassword(command.UserID, EncodePassword(command.Password));

            context.Accept();
        }

        string EncodePassword(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }
    }
}
