using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using TapeStream;

namespace AzureTapeStream
{
    public class AzureTapeStream : ITapeStream
    {
        private readonly CloudBlob _blob;
        private readonly ITapeStreamSerializer _serializer;
        private readonly CloudBlobClient blobClient;
        private readonly CloudBlobContainer container;

        public AzureTapeStream(string name, string connectionString, string containerName)
            : this(name, connectionString, containerName, new TapeStreamSerializer())
        { }

        public AzureTapeStream(string name, string connectionString, string containerName, ITapeStreamSerializer serializer)
        {
            _serializer = serializer;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExist();
            
            _blob = container.GetBlobReference(name);
        }

        public void Append(byte[] buffer)
        {
            using (var file = _blob.OpenWrite())
            {
                var versionToWrite = 1;
                _serializer.WriteRecord(file, buffer, versionToWrite);
            }
        }

        public IEnumerable<TapeRecord> ReadRecords()
        {
            using (var file = _blob.OpenRead())
            {
                while (true)
                {
                    try
                    {
                        if (!file.CanRead)
                        {
                            break;
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        break;
                    }

                    if (file.Position == file.Length)
                        yield break;

                    var record = _serializer.ReadRecord(file);

                    yield return record;
                }
            }
        }
    }
}
