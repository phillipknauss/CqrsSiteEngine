using System;
using System.IO;
using ProtoBuf;
using ReadModel;

namespace FileReadModel
{
    public class DirectoryReadModelStore : IReadModelStore
    {
        private const string _rdmdlExtension = ".rdmdl";

        public string BasePath { get; private set; }

        public DirectoryReadModelStore(string basePath)
        {
            BasePath = basePath;
        }

        public IReadModel GetReadModel<T>()
        {
            var sourcePath = string.Format("{0}{1}{2}{3}", BasePath, Path.DirectorySeparatorChar, typeof(T).FullName, _rdmdlExtension);

            using (FileStream fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                return Serializer.Deserialize<T>(fs) as IReadModel;
            }
        }

        public IReadModel GetOrCreate<T>()
        {
            var sourcePath = string.Format("{0}{1}{2}{3}", BasePath, Path.DirectorySeparatorChar, typeof(T).FullName, _rdmdlExtension);

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
            var sourcePath = string.Format("{0}{1}{2}{3}", BasePath, Path.DirectorySeparatorChar, readModel.GetType().FullName, _rdmdlExtension);

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
                Serializer.Serialize(fs, readModel);
            }
        }
    }
}
