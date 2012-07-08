using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace FileStore
{
    public class FileTapeStream
    {
        private readonly FileInfo _file;

        public FileTapeStream(string name)
        {
            _file = new FileInfo(name);
        }

        private FileStream OpenForWrite()
        {
            return _file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
        }

        public void Append(byte[] buffer, long version=1)
        {
            using (var file = OpenForWrite())
            {
                TapeStreamSerializer.WriteRecord(file, buffer, version);
            }
        }
    }

    public static class TapeStreamSerializer
    {
        public static void WriteRecord(Stream stream, byte[] data, long versionToWrite)
        {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            using (var managed = new SHA1Managed())
            {
                writer.Write(ReadableHeaderStart);
                WriteReadableInt64(writer, data.Length);
                writer.Write(ReadableHeaderEnd);

                writer.Write(data);
                writer.Write(ReadableFooterStart);
                WriteReadableInt64(writer, data.Length);
                WriteReadableInt64(writer, versionToWrite);
                WriteReadableHash(writer, ref managed.ComputeHash(data));
                writer.Write(ReadableFooterEnd);

                ms.Seek(0, SeekOrigin.Begin);
                ms.CopyTo(stream);
            }
        }
    }
}
