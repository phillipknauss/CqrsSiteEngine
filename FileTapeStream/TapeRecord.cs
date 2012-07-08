using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileTapeStream
{
    public sealed class TapeRecord
    {
        public readonly long Version;
        public readonly byte[] Data;

        public TapeRecord(long version, byte[] data)
        {
            Version = version;
            Data = data;
        }
    }
}
