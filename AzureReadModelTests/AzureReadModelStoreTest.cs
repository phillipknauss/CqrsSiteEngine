using System.Runtime.Serialization;
using AzureReadModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using ReadModel;

namespace AzureReadModelTests
{
    /// <summary>
    ///This is a test class for AzureReadModelStoreTest and is intended
    ///to contain all AzureReadModelStoreTest Unit Tests
    ///</summary>
    [TestClass]
    public class AzureReadModelStoreTest
    {
        private const string DevConnectionString = "UseDevelopmentStorage=true";
        private const string TestContainerName = "testreadmodelcontainer";
        private static CloudBlobClient blobClient;
        private static CloudBlobContainer container;

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

        //You can use the following additional attributes as you write your tests:
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(DevConnectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference(TestContainerName);
            
            container.CreateIfNotExist();
        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup]
        public static void MyClassCleanup()
        {
            container.Delete();
        }

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void MyTestInitialize()
        {
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void MyTestCleanup()
        {

        }

        [DataContract]
        public class FakeReadModel : IReadModel
        {
            [DataMember(Order = 1)]
            string Data { get; set; }

            public object Get(string identifier)
            {
                return Data;
            }

            public FakeReadModel() { }
            public FakeReadModel(string data)
            {
                Data = data;
            }
        }

        [TestMethod]
        public void GetOrCreateTest()
        { }

        [TestMethod]
        public void SaveAndGetReadModelTest()
        {
            var expected = new FakeReadModel("TEST");

            var store = new AzureReadModelStore(DevConnectionString, TestContainerName);

            store.Save(expected);

            var actual = store.GetReadModel<FakeReadModel>();

            Assert.AreEqual(expected.Get(""), actual.Get(""));
        }
    }
}
