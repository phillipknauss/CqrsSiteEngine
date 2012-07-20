namespace TapeStream
{
    public interface ITapeStreamSerializer
    {
        /// <summary>
        /// Read the full record from the specified file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        TapeRecord ReadRecord(System.IO.Stream file);

        /// <summary>
        /// Writes the specified data to the specified stream with the specified version
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="data"></param>
        /// <param name="versionToWrite"></param>
        /// <returns>The byte offset of the written record in the stream</returns>
        void WriteRecord(System.IO.Stream stream, byte[] data, long versionToWrite);
    }
}
