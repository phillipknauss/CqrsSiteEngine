using AzureReadModel;
using Ncqrs;
using ReadModelServiceLibrary.Properties;

namespace ReadModelServiceLibrary
{
    public static class Bootstrapper
    {
        public static void BootUp()
        {
            NcqrsEnvironment.SetDefault<ReadModel.IReadModelStore>(InitializeReadModelStore());
        }

        private static ReadModel.IReadModelStore InitializeReadModelStore()
        {
            return new AzureReadModelStore(Settings.Default.AzureConnectionString, Settings.Default.AzureContainerName);
        }
    }
}