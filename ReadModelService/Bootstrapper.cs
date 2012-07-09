using Ncqrs;

namespace ReadModelService
{
    public static class Bootstrapper
    {
        public static void BootUp()
        {
            NcqrsEnvironment.SetDefault<ReadModel.IReadModelStore>(InitializeReadModelStore());
        }

        private static ReadModel.IReadModelStore InitializeReadModelStore()
        {
            return new ReadModel.ReadModelStore("D:\\store\\read_model");
        }

    }
}