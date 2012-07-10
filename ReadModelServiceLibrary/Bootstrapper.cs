using FileReadModel;
using Ncqrs;

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
            return new DirectoryReadModelStore("D:\\store\\read_model");
        }

    }
}