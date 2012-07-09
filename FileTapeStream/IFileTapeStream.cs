namespace FileTapeStream
{
    interface IFileTapeStream
    {
        void Append(byte[] buffer);
        System.Collections.Generic.IEnumerable<TapeRecord> ReadRecords();
    }
}
