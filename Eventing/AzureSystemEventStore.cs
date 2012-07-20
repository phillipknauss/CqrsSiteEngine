using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Eventing.Properties;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Ncqrs.Eventing;
using Ncqrs.Eventing.Sourcing;

namespace Eventing
{
    public class AzureSystemEventStore : IExplorableEventStore
    {
        private readonly IEventStreamer Streamer;
        private readonly CloudBlobClient blobClient;
        private readonly CloudBlobContainer container;

        public AzureSystemEventStore()
        {
            Streamer = new EventStreamer(new EventSerializer(MessagesProvider.GetKnownEventTypes()));
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Config.Config.Get("Eventing.AzureConnectionString"));
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference(Config.Config.Get("Eventing.AzureContainerName"));
            container.CreateIfNotExist();
        }

        public CommittedEventStream ReadFrom(Guid id, long minVersion, long maxVersion)
        {
            List<CommittedEvent> events = GetAllEventsSinceVersion(id, minVersion)
                .Where(n => n.EventSequence <= maxVersion)
                .Select(evt => new CommittedEvent(
                    Guid.NewGuid(), evt.EventIdentifier, id, evt.EventSequence,
                    evt.EventTimeStamp, evt, evt.EventVersion))
                .ToList();

            var stream = new CommittedEventStream(id, events);

            return stream;
        }

        public void Store(UncommittedEventStream eventStream)
        {
            var tapeStream = new AzureTapeStream.AzureTapeStream(eventStream.SourceId.ToString(), Config.Config.Get("Eventing.AzureConnectionString"), Config.Config.Get("Eventing.AzureContainerName"));

            foreach (var record in eventStream.Select(evt => Streamer.SerializeEvent(evt.Payload as ISourcedEvent)))
            {
                tapeStream.Append(record);
            }
        }

        public void StoreEmptyEventSource(Guid id)
        {
            var blob = blobClient.GetBlobReference(id.ToString());
            blob.UploadText(string.Empty);
        }

        public void RemoveEmptyEventSource(Guid id)
        {
            var blob = container.GetBlobReference(id.ToString());
            var events = GetAllEvents(id);

            if (!events.Any())
            {
                blob.Delete();
            }
        }

        public IEnumerable<ISourcedEvent> GetAllEvents(Guid id)
        {
            var tapeStream = new AzureTapeStream.AzureTapeStream(id.ToString(),Config.Config.Get("Eventing.AzureConnectionString"),
                                                                 Config.Config.Get("Eventing.AzureContainerName"));
            var records = tapeStream.ReadRecords();

            List<ISourcedEvent> events = records.Select(rec => Streamer.DeserializeEvent(rec.Data)).ToList();

            // todo: Add caching

            return events.OrderBy(n => n.EventTimeStamp);
        }

        public IEnumerable<ISourcedEvent> GetAllEventsSinceVersion(Guid id, long version)
        {
            var all = GetAllEvents(id);

            return all.Where(n => n.EventSequence >= version);

            // todo: optimize this
        }

        public IEnumerable<Guid> GetEventSourceIndex()
        {
            List<FileInfo> fileInfos = container.ListBlobs()
                .Select(blobItem => new FileInfo(Path.GetFileName(blobItem.Uri.LocalPath)))
                .OrderBy(n => n.LastWriteTimeUtc).ToList();
            
            var index = new List<Guid>();

            foreach (var file in fileInfos)
            {
                string id = file.Name;
                Guid guid_id = Guid.Empty;
                if (!Guid.TryParse(id, out guid_id))
                {
                    continue;
                }
                index.Add(guid_id);
            }
            return index;
        }
    }
}