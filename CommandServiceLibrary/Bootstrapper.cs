using CommandExecutors;
using Ncqrs;
using Ncqrs.Commanding.ServiceModel;
using Ncqrs.Eventing.ServiceModel.Bus;
using Ncqrs.Eventing.Storage;
using ReadModel.Denormalizers;

namespace CommandServiceLibrary
{
    public static class Bootstrapper
    {
        public static void BootUp()
        {
            NcqrsEnvironment.SetDefault<ICommandService>(InitializeCommandService());
            NcqrsEnvironment.SetDefault<IEventStore>(InitializeEventStore());
            NcqrsEnvironment.SetDefault<ReadModel.IReadModelStore>(InitializeReadModelStore());
            NcqrsEnvironment.SetDefault<IEventBus>(InitializeEventBus());
        }

        private static ICommandService InitializeCommandService()
        {
            var service = new Ncqrs.Commanding.ServiceModel.CommandService();
            service.RegisterExecutor(new PostNewTweetCommandExecutor());
            service.RegisterExecutor(new DeleteTweetCommandExecutor());
            service.RegisterExecutor(new CreateChannelCommandExecutor());
            service.RegisterExecutor(new CreateUserCommandExecutor());
            service.RegisterExecutor(new DeleteUserCommandExecutor());
            service.RegisterExecutor(new SetUserPropertyCommandExecutor());
            service.RegisterExecutor(new SetUserPasswordCommandExecutor());

            return service;
        }

        private static IEventStore InitializeEventStore()
        {
            //return new SimpleMicrosoftSqlServerEventStore("Data Source=.\\SQLEXPRESS;Integrated Security=SSPI;User Instance=True;AttachDbFilename=|DataDirectory|\\EventStore.mdf;");
            return new Eventing.FileSystemEventStore("D:\\store\\event_store");
        }

        private static ReadModel.IReadModelStore InitializeReadModelStore()
        {
            return new ReadModel.DirectoryReadModelStore("D:\\store\\read_model");
        }

        private static IEventBus InitializeEventBus()
        {
            var bus = new InProcessEventBus();
            //bus.RegisterHandler(new SqlTweetIndexItemDenormalizer());
            bus.RegisterHandler(new FileTweetIndexItemDenormalizer());
            bus.RegisterHandler(new FileTweetIndexItemDeleteDenormalizer());
            bus.RegisterHandler(new ChannelIndexItemDenormalizer());
            bus.RegisterHandler(new UserIndexDenormalizer());
            bus.RegisterHandler(new UserIndexDeleteDenormalizer());
            bus.RegisterHandler(new UserIndexPropertySetDenormalizer());
            bus.RegisterHandler(new UserIndexPasswordSetDenormalizer());

            return bus;
        }
    }
}