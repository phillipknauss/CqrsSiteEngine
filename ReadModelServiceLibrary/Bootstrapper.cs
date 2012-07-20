using AzureReadModel;
using Ncqrs;
using ReadModelServiceLibrary.Properties;

namespace ReadModelServiceLibrary
{
    public static class Bootstrapper
    {
        public static void BootUp()
        {
            // initialize the config system
            Config.Config.LoadFromXml(Config.Config.DefaultPath);

            NcqrsEnvironment.SetDefault<ReadModel.IReadModelStore>(InitializeReadModelStore());
        }

        private static ReadModel.IReadModelStore InitializeReadModelStore()
        {
            return new AzureReadModelStore( Config.Config.Get("ReadModel.AzureConnectionString"),  Config.Config.Get("ReadModel.AzureContainerName"));
        }
    }
}