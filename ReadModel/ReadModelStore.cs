using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ProtoBuf;

namespace ReadModel
{
    public interface IReadModelStore
    {
        IReadModel GetReadModel<T>();
        IReadModel GetOrCreate<T>();
        void Save(IReadModel readModel);
    }

    public class ReadModelStore : IReadModelStore
    {
        public string BasePath { get; private set; }

        public ReadModelStore(string basePath)
        {
            BasePath = basePath;
        }

        public IReadModel GetReadModel<T>()
        {
            var sourcePath = BasePath + Path.DirectorySeparatorChar + typeof(T).FullName + ".rdmdl";

            using (FileStream fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                return Serializer.Deserialize<T>(fs) as IReadModel;
            }
        }

        public IReadModel GetOrCreate<T>()
        {
            var sourcePath = BasePath + Path.DirectorySeparatorChar + typeof(T).FullName + ".rdmdl";

            if (!File.Exists(sourcePath))
            {
                var model = Activator.CreateInstance(typeof(T));
                Save(model as IReadModel);
                return model as IReadModel;
            }

            return GetReadModel<T>();
        }

        public void Save(IReadModel readModel)
        {
            var sourcePath = BasePath + Path.DirectorySeparatorChar + readModel.GetType().FullName + ".rdmdl";

            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }

            var fileMode = FileMode.Truncate;

            if (!File.Exists(sourcePath))
            {
                fileMode = FileMode.CreateNew;
            }

            using (FileStream fs = new FileStream(sourcePath,fileMode, FileAccess.Write, FileShare.Read))
            {
                Serializer.Serialize<IReadModel>(fs, readModel);
            }
        }
    }
}
