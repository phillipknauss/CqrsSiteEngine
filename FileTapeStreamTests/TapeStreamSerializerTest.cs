using FileTapeStream;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using TapeStream;

namespace FileTapeStreamTests
{
    
    
    /// <summary>
    ///This is a test class for TapeStreamSerializerTest and is intended
    ///to contain all TapeStreamSerializerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TapeStreamSerializerTest
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


        /// <summary>
        ///A test for ReadAndVerifySignature
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TapeStream.dll")]
        public void ReadAndVerifySignatureTest()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(Encoding.ASCII.GetBytes("SIGNATURE"));

                var expected = Encoding.ASCII.GetBytes("SIGNATURE");

                ms.Seek(0, SeekOrigin.Begin);

                TapeStreamSerializer_Accessor.ReadAndVerifySignature(ms, Encoding.ASCII.GetBytes("SIGNATURE"), "SIGNATURE");

                // No assert - ReadAndVerifySignature should throw if it fails, 
                // otherwise test passes.
            }
        }

        /// <summary>
        ///A test for ReadAndVerifySignature
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TapeStream.dll")]
        [ExpectedException(typeof(SignatureVerificationException))]
        public void ReadAndVerifySignatureFailsOnBadTest_TooShort()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(Encoding.ASCII.GetBytes("SIGNATUR"));

                var expected = Encoding.ASCII.GetBytes("SIGNATURE");

                ms.Seek(0, SeekOrigin.Begin);

                TapeStreamSerializer_Accessor.ReadAndVerifySignature(ms, Encoding.ASCII.GetBytes("SIGNATURE"), "SIGNATURE");

            }
        }

        /// <summary>
        ///A test for ReadAndVerifySignature
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TapeStream.dll")]
        [ExpectedException(typeof(SignatureVerificationException))]
        public void ReadAndVerifySignatureFailsOnBadTest_BadCharacter()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(Encoding.ASCII.GetBytes("SIGNATARE"));

                var expected = Encoding.ASCII.GetBytes("SIGNATURE");

                ms.Seek(0, SeekOrigin.Begin);

                TapeStreamSerializer_Accessor.ReadAndVerifySignature(ms, Encoding.ASCII.GetBytes("SIGNATURE"), "SIGNATURE");

            }
        }

        /// <summary>
        ///A test for ReadReadableHash
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TapeStream.dll")]
        public void ReadReadableHashTest()
        {
            byte expected = 109;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            using (var managed = new SHA1Managed())
            {
                var buffer = Encoding.UTF8.GetBytes(Convert.ToBase64String(managed.ComputeHash(Encoding.UTF8.GetBytes("TEST"))));

                TapeStreamSerializer_Accessor.WriteReadableHash(bw, buffer);

                ms.Seek(0, SeekOrigin.Begin);

                var actual = TapeStreamSerializer_Accessor.ReadReadableHash(ms);

                Assert.AreEqual(expected,new List<Byte>(actual)[0]);
            }
        }

        /// <summary>
        ///A test for ReadReadableInt64
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TapeStream.dll")]
        public void ReadReadableInt64Test()
        {
            var expected = 74L;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                var buffer = Encoding.UTF8.GetBytes(74L.ToString("x16"));
                bw.Write(buffer);
                ms.Seek(0, SeekOrigin.Begin);

                var actual = TapeStreamSerializer_Accessor.ReadReadableInt64(ms);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for ReadRecord
        ///</summary>
        [TestMethod()]
        public void ReadRecordTest()
        {
            var expected = new TapeRecord(3L, Encoding.UTF8.GetBytes("TEST"));

            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(ms))
            {
                new TapeStreamSerializer().WriteRecord(ms, Encoding.UTF8.GetBytes("TEST"), 3L);
                ms.Seek(0, SeekOrigin.Begin);

                var actual = new TapeStreamSerializer().ReadRecord(ms);

                Assert.AreEqual(expected.Version, actual.Version);
                Assert.AreEqual(expected.Data[0], actual.Data[0]);
            }
        }

        /// <summary>
        ///A test for WriteReadableHash
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TapeStream.dll")]
        public void WriteReadableHashTest()
        {
            byte expected = 109;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            using (var managed = new SHA1Managed())
            {
                var buffer = Encoding.UTF8.GetBytes(Convert.ToBase64String(managed.ComputeHash(Encoding.UTF8.GetBytes("TEST"))));

                TapeStreamSerializer_Accessor.WriteReadableHash(bw, buffer);

                ms.Seek(0, SeekOrigin.Begin);

                var actual = TapeStreamSerializer_Accessor.ReadReadableHash(ms);

                Assert.AreEqual(expected, new List<Byte>(actual)[0]);
            }
        }

        /// <summary>
        ///A test for WriteReadableInt64
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TapeStream.dll")]
        public void WriteReadableInt64Test()
        {
            var expected = 74L;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                TapeStreamSerializer_Accessor.WriteReadableInt64(bw, 74L);
                ms.Seek(0, SeekOrigin.Begin);

                var actual = TapeStreamSerializer_Accessor.ReadReadableInt64(ms);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for WriteRecord
        ///</summary>
        [TestMethod()]
        public void WriteRecordTest()
        {
            var expected = new TapeRecord(3L, Encoding.UTF8.GetBytes("TEST"));

            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(ms))
            {
                new TapeStreamSerializer().WriteRecord(ms, Encoding.UTF8.GetBytes("TEST"), 3L);
                ms.Seek(0, SeekOrigin.Begin);

                var actual = new TapeStreamSerializer().ReadRecord(ms);

                Assert.AreEqual(expected.Version, actual.Version);
                Assert.AreEqual(expected.Data[0], actual.Data[0]);
            }
        }

        /// <summary>
        ///A test for TapeStreamSerializer Constructor
        ///</summary>
        [TestMethod()]
        public void TapeStreamSerializerConstructorTest()
        {
            TapeStreamSerializer target = new TapeStreamSerializer();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ReadAndVerifySignature
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TapeStream.dll")]
        public void ReadAndVerifySignatureTest1()
        {
            Stream source = null; // TODO: Initialize to an appropriate value
            IList<byte> signature = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            TapeStreamSerializer_Accessor.ReadAndVerifySignature(source, signature, name);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ReadReadableHash
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TapeStream.dll")]
        public void ReadReadableHashTest1()
        {
            Stream stream = null; // TODO: Initialize to an appropriate value
            IEnumerable<byte> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<byte> actual;
            actual = TapeStreamSerializer_Accessor.ReadReadableHash(stream);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ReadReadableInt64
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TapeStream.dll")]
        public void ReadReadableInt64Test1()
        {
            Stream stream = null; // TODO: Initialize to an appropriate value
            long expected = 0; // TODO: Initialize to an appropriate value
            long actual;
            actual = TapeStreamSerializer_Accessor.ReadReadableInt64(stream);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ReadRecord
        ///</summary>
        [TestMethod()]
        public void ReadRecordTest1()
        {
            TapeStreamSerializer target = new TapeStreamSerializer(); // TODO: Initialize to an appropriate value
            Stream file = null; // TODO: Initialize to an appropriate value
            TapeRecord expected = null; // TODO: Initialize to an appropriate value
            TapeRecord actual;
            actual = target.ReadRecord(file);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for WriteReadableHash
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TapeStream.dll")]
        public void WriteReadableHashTest1()
        {
            BinaryWriter writer = null; // TODO: Initialize to an appropriate value
            byte[] hash = null; // TODO: Initialize to an appropriate value
            TapeStreamSerializer_Accessor.WriteReadableHash(writer, hash);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for WriteReadableInt64
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TapeStream.dll")]
        public void WriteReadableInt64Test1()
        {
            BinaryWriter writer = null; // TODO: Initialize to an appropriate value
            long value = 0; // TODO: Initialize to an appropriate value
            TapeStreamSerializer_Accessor.WriteReadableInt64(writer, value);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for WriteRecord
        ///</summary>
        [TestMethod()]
        public void WriteRecordTest1()
        {
            TapeStreamSerializer target = new TapeStreamSerializer(); // TODO: Initialize to an appropriate value
            Stream stream = null; // TODO: Initialize to an appropriate value
            byte[] data = null; // TODO: Initialize to an appropriate value
            long versionToWrite = 0; // TODO: Initialize to an appropriate value
            target.WriteRecord(stream, data, versionToWrite);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
