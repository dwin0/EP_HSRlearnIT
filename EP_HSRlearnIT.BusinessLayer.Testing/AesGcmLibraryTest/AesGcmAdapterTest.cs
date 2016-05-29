using System;
using System.Security.Cryptography;
using EP_HSRlearnIT.BusinessLayer.AesGcmLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EP_HSRlearnIT.BusinessLayer.Testing.AesGcmLibraryTest
{
    [TestClass]
    public class AesGcmAdapterTest
    {
        protected AesGcmAdapter Library;
        
        [TestInitialize]
        public void Initialize()
        {
            Library = new AesGcmAdapter();
        }

        [TestMethod]
        public void EncryptTestCase14Test()
        {
            const string key = "0000000000000000000000000000000000000000000000000000000000000000";
            const string plaintext = "00000000000000000000000000000000";
            const string iv = "000000000000000000000000";
            const string aad = "";
            Tuple<string, string> returnValue = Library.Encrypt(key, plaintext, iv, aad);

            string resultingTag = returnValue.Item1;
            string resultingCiphertext = returnValue.Item2;

            const string expectedTag = "d0d1c8a799996bf0265b98b5d48ab919";
            const string expectedCiphertext = "cea7403d4d606b6e074ec5d3baf39d18";

            Assert.AreEqual(expectedTag, resultingTag);
            Assert.AreEqual(expectedCiphertext, resultingCiphertext);
        }

        [TestMethod, ExpectedException(typeof(CryptographicException))]
        public void EncryptWrongKeySizeTest()
        {
            const string key = "00";
            const string plaintext = "00000000000000000000000000000000";
            const string iv = "000000000000000000000000";
            const string aad = "";
            Library.Encrypt(key, plaintext, iv, aad);
        }

        [TestMethod]
        public void EncryptOptionalIvTest()
        {
            const string key = "0000000000000000000000000000000000000000000000000000000000000000";
            const string plaintext = "00000000000000000000000000000000";
            const string iv = "";
            const string aad = "";
            Tuple<string, string> returnValue = Library.Encrypt(key, plaintext, iv, aad);

            string resultingTag = returnValue.Item1;
            string resultingCiphertext = returnValue.Item2;

            const string expectedTag = "d0d1c8a799996bf0265b98b5d48ab919";
            const string expectedCiphertext = "cea7403d4d606b6e074ec5d3baf39d18";

            Assert.AreEqual(expectedTag, resultingTag);
            Assert.AreEqual(expectedCiphertext, resultingCiphertext);
        }

        [TestMethod]
        public void DecryptTestCase14Test()
        {
            const string key = "0000000000000000000000000000000000000000000000000000000000000000";
            const string cyphertext = "cea7403d4d606b6e074ec5d3baf39d18";
            const string iv = "000000000000000000000000";
            const string aad = "";
            const string tag = "d0d1c8a799996bf0265b98b5d48ab919";

            string returnValue = Library.Decrypt(key,cyphertext, iv, aad, tag);

            const string expectedPlaintext = "00000000000000000000000000000000";

            Assert.AreEqual(expectedPlaintext, returnValue);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void DecryptWrongTagTest()
        {
            const string key = "0000000000000000000000000000000000000000000000000000000000000000";
            const string cyphertext = "cea7403d4d606b6e074ec5d3baf39d18";
            const string iv = "000000000000000000000000";
            const string aad = "";
            const string tag = "aa";

            Library.Decrypt(key, cyphertext, iv, aad, tag);
        }

        [TestMethod]
        public void DecryptOptionalIvTest()
        {
            const string key = "0000000000000000000000000000000000000000000000000000000000000000";
            const string cyphertext = "cea7403d4d606b6e074ec5d3baf39d18";
            const string iv = "";
            const string aad = "";
            const string tag = "d0d1c8a799996bf0265b98b5d48ab919";

            string returnValue = Library.Decrypt(key, cyphertext, iv, aad, tag);

            const string expectedPlaintext = "00000000000000000000000000000000";

            Assert.AreEqual(expectedPlaintext, returnValue);
        }

        [TestMethod]
        public void GenerateHexKeyWithLessInputTest()
        {
            string resultingKey = Library.GenerateKey("TestTest");
            Assert.AreEqual("TestTestTestTestTestTestTestTest", resultingKey);
            Assert.AreEqual(32, resultingKey.Length);
        }

        [TestMethod]
        public void GenerateHexKeyWithToMuchInputTest()
        {
            string resultingKey = Library.GenerateKey("TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest");
            Assert.AreEqual("TestTestTestTestTestTestTestTest", resultingKey);
            Assert.AreEqual(32, resultingKey.Length);
        }

        [TestMethod]
        public void GenerateHexKeyWithRightInputTest()
        {
            string resultingKey = Library.GenerateKey("TestTestTestTestTestTestTestTest");
            Assert.AreEqual("TestTestTestTestTestTestTestTest", resultingKey);
            Assert.AreEqual(32, resultingKey.Length);
        }

        [TestMethod]
        public void HexStringToDecimalByteArrayTest()
        {
            byte[] resultingHex = Library.HexStringToDecimalByteArray("54657374");
            byte[] expectedHex = { 84, 101, 115, 116 };
            CollectionAssert.AreEqual(expectedHex, resultingHex);
        }

        [TestMethod]
        public void BytesToStringTest()
        {
            byte[] startBytes = { 84, 101, 115, 116 };
            string result = Library.BytesToString(startBytes);
            Assert.AreEqual("Test", result);
        }

        [TestMethod]
        public void StringToBytesTest()
        {
            byte[] result = Library.StringToBytes("Test");
            byte[] expectedBytes = { 84, 101, 115, 116 };
            CollectionAssert.AreEqual(expectedBytes, result);
        }

        [TestMethod]
        public void ConvertToHexStringTest()
        {
            string result = Library.ConvertToHexString("Test");
            Assert.AreEqual("54657374", result);
        }

    }
}
