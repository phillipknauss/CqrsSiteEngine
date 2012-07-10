namespace TapeStream
{
    public interface ITapeStreamSerializer
    {
        TapeRecord ReadRecord(System.IO.Stream file);
        void WriteRecord(System.IO.Stream stream, byte[] data, long versionToWrite);
    }
}
