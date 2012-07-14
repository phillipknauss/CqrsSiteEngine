using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using TapeStream;
using System.IO;

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
            byte[] orig = null;

            if (_blob.Exists())
            {
                using (var fileRead = _blob.OpenRead())
                {
                    orig = _blob.DownloadByteArray();
                }
            }

            using (var fileWrite = _blob.OpenWrite())
            {
                var versionToWrite = 1;
                if (orig != null)
                {
                    fileWrite.Write(orig, 0, orig.Length);
                }
                    _serializer.WriteRecord(fileWrite, buffer, versionToWrite);
                
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

    /// <summary>
    /// Extension methods for Cloud Blobs
    /// </summary>
    public static class BlobExtensions
    {
        /// <summary>
        /// Check if the specified blob exists
        /// </summary>
        /// <param name="blob"></param>
        /// <returns></returns>
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
