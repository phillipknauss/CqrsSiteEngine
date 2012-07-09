using System;
using System.Collections.Generic;
using System.IO;

namespace FileTapeStream
{
    public class FileTapeStream : IFileTapeStream
    {
        private readonly FileInfo _file;

        private readonly ITapeStreamSerializer _serializer;

        public FileTapeStream(string name)
            : this(name, new TapeStreamSerializer())
        { }

        public FileTapeStream(string name, ITapeStreamSerializer serializer)
        {
            _file = new FileInfo(name);

            _serializer = serializer;
        }

        private FileStream OpenForWrite()
        {
            return _file.Open(FileMode.Append, FileAccess.Write, FileShare.Read);
        }

        public void Append(byte[] buffer)
        {
            using (var file = OpenForWrite())
            {
                var versionToWrite = 1;
                _serializer.WriteRecord(file, buffer, versionToWrite);
            }
        }

        private FileStream OpenForRead()
        {
            return _file.Open(FileMode.Open, FileAccess.Read);
        }

        public IEnumerable<TapeRecord> ReadRecords()
        {
            using (var file = OpenForRead())
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
