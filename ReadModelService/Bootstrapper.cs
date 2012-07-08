using System;
using Ncqrs.Commanding.ServiceModel;
using Ncqrs;
using Ncqrs.Eventing.Storage;
using Ncqrs.Eventing.Storage.SQL;
using Ncqrs.Eventing.ServiceModel.Bus;
using ReadModel.Denormalizers;

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