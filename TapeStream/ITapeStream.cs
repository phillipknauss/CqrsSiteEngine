namespace TapeStream
{
    public interface ITapeStream
    {
        void Append(byte[] buffer);
        System.Collections.Generic.IEnumerable<TapeRecord> ReadRecords();
    }
}
