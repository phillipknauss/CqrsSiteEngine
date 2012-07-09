using ReadModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.Serialization;
using System.IO;

namespace ReadModelTests
{
    
    
    /// <summary>
    ///This is a test class for ReadModelStoreTest and is intended
    ///to contain all ReadModelStoreTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ReadModelStoreTest
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

        [DataContract]
        public class FakeReadModel : IReadModel
        {
            [DataMember(Order=1)]string Data { get; set; }

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

        [TestMethod()]
        public void GetOrCreateTest()
        {
            
        }

        [TestMethod()]
        public void SaveAndGetReadModelTest()
        {
            var expected = new FakeReadModel("TEST");

            Directory.CreateDirectory("store");
            var store = new DirectoryReadModelStore("store");

            store.Save(expected);

            var actual = store.GetReadModel<FakeReadModel>();

            Assert.AreEqual(expected.Get(""), actual.Get(""));
        }

    }
}
