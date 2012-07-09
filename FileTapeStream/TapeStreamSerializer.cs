using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;

namespace FileTapeStream
{
    public class TapeStreamSerializer : ITapeStreamSerializer
    {
        static readonly byte[] ReadableHeaderStart = Encoding.UTF8.GetBytes("Start");
        static readonly byte[] ReadableHeaderEnd = Encoding.UTF8.GetBytes("Header-End");
        static readonly byte[] ReadableFooterStart = Encoding.UTF8.GetBytes("Footer-Start");
        static readonly byte[] ReadableFooterEnd = Encoding.UTF8.GetBytes("End");

        static void WriteReadableInt64(BinaryWriter writer, long value)
        {
            var buffer = Encoding.UTF8.GetBytes(value.ToString("x16"));
            writer.Write(buffer);
        }

        static void WriteReadableHash(BinaryWriter writer, byte[] hash)
        {
            var buffer = Encoding.UTF8.GetBytes(Convert.ToBase64String(hash));
            writer.Write(buffer);
        }

        public void WriteRecord(Stream stream, byte[] data, long versionToWrite)
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
                WriteReadableHash(writer, managed.ComputeHash(data));
                writer.Write(ReadableFooterEnd);

                ms.Seek(0, SeekOrigin.Begin);
                ms.CopyTo(stream);
            }
        }

        public TapeRecord ReadRecord(Stream file)
        {
            ReadAndVerifySignature(file, ReadableHeaderStart, "Start");
            var dataLength = ReadReadableInt64(file);
            ReadAndVerifySignature(file, ReadableHeaderEnd, "Header-End");

            var data = new byte[dataLength];
            file.Read(data, 0, (int)dataLength);

            ReadAndVerifySignature(file, ReadableFooterStart, "Footer-Start");

            ReadReadableInt64(file);
            var recVersion = ReadReadableInt64(file);
            var hash = ReadReadableHash(file);
            using (var managed = new SHA1Managed())
            {
                var computed = managed.ComputeHash(data);
                if (!computed.SequenceEqual(hash))
                    throw new InvalidOperationException("Hash corrupted");
            }

            ReadAndVerifySignature(file, ReadableFooterEnd, "End");

            return new TapeRecord(recVersion, data);
        }

        static long ReadReadableInt64(Stream stream)
        {
            var buffer = new byte[16];
            stream.Read(buffer, 0, 16);
            var s = Encoding.UTF8.GetString(buffer);
            return Int64.Parse(s, NumberStyles.HexNumber);
        }

        static IEnumerable<byte> ReadReadableHash(Stream stream)
        {
            var buffer = new byte[28];
            stream.Read(buffer, 0, buffer.Length);
            var hash = Convert.FromBase64String(Encoding.UTF8.GetString(buffer));
            return hash;
        }

        static void ReadAndVerifySignature(Stream source, IList<byte> signature, string name)
        {
            for (var i = 0; i < signature.Count; i++)
            {
                var readByte = source.ReadByte();
                if (readByte == -1)
                {
                    throw new SignatureVerificationException(String.Format(
                        "Expected byte[{0}] of signature '{1}', but found EOL", i, name));
                }
                if (readByte != signature[i])
                {
                    throw new SignatureVerificationException("Signature failed: " + name);
                }
            }
        }
    }

    public class SignatureVerificationException : InvalidOperationException
    {
        public SignatureVerificationException(string message) : base(message) { }
    }
}
