using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ncqrs.Eventing.Storage;
using Ncqrs.Eventing;
using ProtoBuf;
using ProtoBuf.Meta;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Globalization;
using Events;
using Ncqrs.Eventing.Sourcing;

namespace Eventing
{
    public class FileSystemEventStore : IEventStore
    {
        public string BasePath { get; protected set; }

        public IEventStreamer Streamer { get; private set; }
        
        public FileSystemEventStore(string basePath)
        {
            BasePath = basePath;

            Streamer = new EventStreamer(new EventSerializer(MessagesProvider.GetKnownEventTypes()));
            
        }

        #region EventStore

        public CommittedEventStream ReadFrom(Guid id, long minVersion, long maxVersion)
        {
            List<CommittedEvent> events = new List<CommittedEvent>();

            var loadedEvents = GetAllEventsSinceVersion(id, minVersion);

            loadedEvents = loadedEvents.Where(n => n.EventSequence <= maxVersion).ToList();

            foreach (var evt in loadedEvents)
            {
                events.Add(new CommittedEvent(Guid.NewGuid(), evt.EventIdentifier, id, evt.EventSequence, evt.EventTimeStamp, evt as object, evt.EventVersion));
            }

            var stream = new CommittedEventStream(id, events);

            return stream;
        }

        public void Store(UncommittedEventStream eventStream)
        {
            var sourcePath = BasePath + Path.DirectorySeparatorChar + eventStream.SourceId.ToString();

            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }

            var tapeStream = new FileTapeStream.FileTapeStream(sourcePath);

            foreach (var evt in eventStream)
            {
                var record = Streamer.SerializeEvent(evt.Payload as ISourcedEvent);
                tapeStream.Append(record);
            }
        }

        public void StoreEmptyEventSource(Guid id)
        {
            var sourcePath = BasePath + Path.DirectorySeparatorChar + id.ToString();
            if (File.Exists(sourcePath))
            {
                return;
            }

            using (FileStream fs = new FileStream(sourcePath, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
            {
                fs.Flush();
            }
        }

        public void RemoveEmptyEventSource(Guid id)
        {
            var sourcePath = BasePath + Path.DirectorySeparatorChar + id.ToString();
            if (!File.Exists(sourcePath))
            {
                return;
            }

            var events = GetAllEvents(id);

            if (events.Count() == 0)
            {
                File.Delete(sourcePath);
            }
        }

        public IEnumerable<ISourcedEvent> GetAllEvents(Guid id)
        {
        
            var sourcePath = BasePath + Path.DirectorySeparatorChar + id.ToString();

            List<ISourcedEvent> events = new List<ISourcedEvent>();
            
            var tapeStream = new FileTapeStream.FileTapeStream(sourcePath);
            var records = tapeStream.ReadRecords();

            foreach (var rec in records)
            {
                var evt = Streamer.DeserializeEvent(rec.Data);
                events.Add(evt);
            }

            // todo: Add caching

            return events.OrderBy(n => n.EventTimeStamp);
        }

        public IEnumerable<ISourcedEvent> GetAllEventsSinceVersion(Guid id, long version)
        {
            var all = GetAllEvents(id);

            return all.Where(n => n.EventSequence >= version);

            // todo: optimize this
        }

        #endregion

        public IEnumerable<Guid> GetEventSourceIndex()
        {
            var sourceIds = Directory.GetFiles((this as FileSystemEventStore).BasePath);

            var fileInfos = new List<FileInfo>();
            foreach (var id in sourceIds)
            {
                fileInfos.Add(new FileInfo(id));
            }

            fileInfos = fileInfos.OrderBy(n => n.LastWriteTimeUtc).ToList();

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

        protected string GetSourcePath(IEventSource source)
        {
            return BasePath + Path.DirectorySeparatorChar + source.EventSourceId.ToString();
        }
    }

}
