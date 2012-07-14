using System.IO;
using System.Text;
using AzureTapeStream;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using TapeStream;
using System.Collections.Generic;

namespace AzureTapeStreamTests
{


    /// <summary>
    ///This is a test class for AzureTapeStreamTest and is intended
    ///to contain all AzureTapeStreamTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AzureTapeStreamTest
    {
        private const string DevConnectionString = "UseDevelopmentStorage=true";
        private const string TestContainerName = "testtapecontainer";
        
        private TestContext testContextInstance;
        private static CloudBlobClient blobClient;
        private static CloudBlobContainer container;

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

        //You can use the following additional attributes as you write your tests:
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(DevConnectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference(TestContainerName);
            container.CreateIfNotExist();
        }
        
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            container.Delete();
        }
        
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
        }
        
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {

        }

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
            var target = new AzureTapeStream.AzureTapeStream("test", DevConnectionString, TestContainerName, new FakeTapeStreamSerializer());

            const string expected = "Some test data";

            target.Append(Encoding.ASCII.GetBytes(expected));

            IEnumerable<TapeRecord> records = target.ReadRecords();

            var items = new List<TapeRecord>(records);

            var actual = Encoding.ASCII.GetString(items[0].Data);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for repeated Append and ReadRecords
        ///</summary>
        [TestMethod]
        public void RepeatedAppendAndReadRecordsTest()
        {
            var target = new AzureTapeStream.AzureTapeStream(DateTime.Now.ToString("yyyyMMsshhmmss"), DevConnectionString, TestContainerName, new FakeTapeStreamSerializer());

            const string expected = "Some test dataSome more test data";
            
            target.Append(Encoding.ASCII.GetBytes("Some test data"));

            target.Append(Encoding.ASCII.GetBytes("Some more test data"));

            IEnumerable<TapeRecord> records = target.ReadRecords();

            var items = new List<TapeRecord>(records);

            var actual = Encoding.ASCII.GetString(items[0].Data);
            Assert.AreEqual(expected, actual);
        }
    }
}
