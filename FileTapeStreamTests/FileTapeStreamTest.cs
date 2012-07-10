using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TapeStream;

namespace FileTapeStreamTests
{
    /// <summary>
    ///This is a test class for FileTapeStreamTest and is intended
    ///to contain all FileTapeStreamTest Unit Tests
    ///</summary>
    [TestClass]
    public class FileTapeStreamTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        public class FakeTapeStreamSerializer : ITapeStreamSerializer
        {
            public void WriteRecord(Stream stream, byte[] data, long versionToWrite)
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(data);
                }
            }

            public TapeRecord ReadRecord(Stream file)
            {
                byte[] result = new byte[file.Length];
                using (BinaryReader br = new BinaryReader(file))
                {
                    br.Read(result, 0, (int)file.Length);
                }
                return new TapeRecord(1L, result);
            }
        }

        /// <summary>
        ///A test for Append and ReadRecords
        ///</summary>
        [TestMethod]
        public void AppendAndReadRecordsTest()
        {
            var target = new FileTapeStream.FileTapeStream("test", new FakeTapeStreamSerializer());

            const string expected = "Some test data";

            target.Append(Encoding.ASCII.GetBytes(expected));
            
            IEnumerable<TapeRecord> records = target.ReadRecords();
            
            var items = new List<TapeRecord>(records);

            var actual = Encoding.ASCII.GetString(items[0].Data);

            Assert.AreEqual(expected, actual);
        }
    }
}
