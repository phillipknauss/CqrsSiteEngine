using System;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using ProtoBuf;
using ReadModel;

namespace AzureReadModel
{
    public class AzureReadModelStore : IReadModelStore
    {
        private readonly CloudBlobClient blobClient;
        private readonly CloudBlobContainer container;

        public AzureReadModelStore(string connectionString, string containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExist();
        }

        public IReadModel GetReadModel<T>()
        {
            CloudBlob blob = container.GetBlobReference(GetName<T>());

            using (MemoryStream ms = new MemoryStream())
            {
                blob.DownloadToStream(ms);
                return Serializer.Deserialize<T>(ms) as IReadModel;
            }
        }

        public IReadModel GetOrCreate<T>()
        {
            if (!File.Exists(GetName<T>()))
            {
                var model = Activator.CreateInstance(typeof(T));
                Save(model as IReadModel);
                return model as IReadModel;
            }

            return GetReadModel<T>();
        }

        public void Save(IReadModel readModel)
        {
            CloudBlob blob = container.GetBlobReference(GetName(readModel.GetType().FullName));

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, readModel);
                blob.DeleteIfExists();
                blob.UploadFromStream(ms);
                ms.Close();
            }
        }

        private static string GetName(string fullName)
        {
            return string.Format("{0}.rdmdl", fullName.ToLower());
        }

        private static string GetName<T>()
        {
            return string.Format("{0}.rdmdl", typeof(T).FullName.ToLower());
        }
    }
}
