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
    [TestClass]
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
        [TestMethod]
        [DeploymentItem("TapeStream.dll")]
        public void ReadAndVerifySignatureTest()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(Encoding.ASCII.GetBytes("SIGNATURE"));

                var expected = Encoding.ASCII.GetBytes("SIGNATURE");

                ms.Seek(0, SeekOrigin.Begin);

                TapeStreamSerializer_Accessor.ReadAndVerifySignature(ms, expected, "SIGNATURE");

                // No assert - ReadAndVerifySignature should throw if it fails, 
                // otherwise test passes.
            }
        }

        /// <summary>
        ///A test for ReadAndVerifySignature
        ///</summary>
        [TestMethod]
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

                TapeStreamSerializer_Accessor.ReadAndVerifySignature(ms, expected, "SIGNATURE");

            }
        }

        /// <summary>
        ///A test for ReadAndVerifySignature
        ///</summary>
        [TestMethod]
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

                TapeStreamSerializer_Accessor.ReadAndVerifySignature(ms, expected, "SIGNATURE");
            }
        }

        /// <summary>
        ///A test for ReadReadableHash
        ///</summary>
        [TestMethod]
        [DeploymentItem("TapeStream.dll")]
        public void ReadReadableHashTest()
        {
            const byte expected = 109;

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
        [TestMethod]
        [DeploymentItem("TapeStream.dll")]
        public void ReadReadableInt64Test()
        {
            const long expected = 74L;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                var buffer = Encoding.UTF8.GetBytes(expected.ToString("x16"));
                bw.Write(buffer);
                ms.Seek(0, SeekOrigin.Begin);

                var actual = TapeStreamSerializer_Accessor.ReadReadableInt64(ms);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for ReadRecord
        ///</summary>
        [TestMethod]
        public void ReadRecordTest()
        {
            byte[] expecteddata = Encoding.UTF8.GetBytes("TEST");
            const long versionToWrite = 3L;
            var expectedrecord = new TapeRecord(versionToWrite, expecteddata);

            using (MemoryStream ms = new MemoryStream())
            {
                new TapeStreamSerializer().WriteRecord(ms, expecteddata, versionToWrite);
                ms.Seek(0, SeekOrigin.Begin);

                var actual = new TapeStreamSerializer().ReadRecord(ms);

                Assert.AreEqual(expectedrecord.Version, actual.Version);
                Assert.AreEqual(expectedrecord.Data[0], actual.Data[0]);
            }
        }

        /// <summary>
        ///A test for WriteReadableHash
        ///</summary>
        [TestMethod]
        [DeploymentItem("TapeStream.dll")]
        public void WriteReadableHashTest()
        {
            const byte expected = 109;

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
        [TestMethod]
        [DeploymentItem("TapeStream.dll")]
        public void WriteReadableInt64Test()
        {
            const long expected = 74L;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                TapeStreamSerializer_Accessor.WriteReadableInt64(bw, expected);
                ms.Seek(0, SeekOrigin.Begin);

                var actual = TapeStreamSerializer_Accessor.ReadReadableInt64(ms);

                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///A test for WriteRecord
        ///</summary>
        [TestMethod]
        public void WriteRecordTest()
        {
            byte[] expecteddata = Encoding.UTF8.GetBytes("TEST");
            const long expectedversion = 3L;
            var expectedrecord = new TapeRecord(expectedversion, expecteddata);

            using (MemoryStream ms = new MemoryStream())
            {
                new TapeStreamSerializer().WriteRecord(ms, expecteddata, expectedversion);
                ms.Seek(0, SeekOrigin.Begin);

                var actual = new TapeStreamSerializer().ReadRecord(ms);

                Assert.AreEqual(expectedrecord.Version, actual.Version);
                Assert.AreEqual(expectedrecord.Data[0], actual.Data[0]);
            }
        }
    }
}
