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

            using (var blobS = blob.OpenRead())
            {
                return Serializer.Deserialize<T>(blobS) as IReadModel;
            }
        }

        public IReadModel GetOrCreate<T>()
        {
            CloudBlob blob = container.GetBlobReference(GetName<T>());
            if (!blob.Exists())
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
            //blob.DeleteIfExists();
            
            using (var blobS = blob.OpenWrite())
            {
                Serializer.Serialize(blobS,readModel);
            }
        }

        private static string GetName(string fullName)
        {
            return string.Format("{0}.ardmdl", fullName.ToLower());
        }

        private static string GetName<T>()
        {
            return string.Format("{0}.ardmdl", typeof(T).FullName.ToLower());
        }
    }

    public static class BlobExtensions
    {
        public static bool Exists(this CloudBlob blob)
        {
            try
            {
                blob.FetchAttributes();
                return true;
            }
            catch (StorageClientException e)
            {
                if (e.ErrorCode == StorageErrorCode.ResourceNotFound)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
