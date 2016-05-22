using System;
using System.Security.Cryptography;
using EP_HSRlearnIT.BusinessLayer.AesGcmLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EP_HSRlearnIT.BusinessLayerTests.CryptoToolsTest
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
            string key = "0000000000000000000000000000000000000000000000000000000000000000";
            string plaintext = "00000000000000000000000000000000";
            string iv = "000000000000000000000000";
            string aad = "";
            Tuple<string, string> returnValue = Library.Encrypt(key, plaintext, iv, aad);

            string resultingTag = returnValue.Item1;
            string resultingCiphertext = returnValue.Item2;

            string expectedTag = "d0d1c8a799996bf0265b98b5d48ab919";
            string expectedCiphertext = "cea7403d4d606b6e074ec5d3baf39d18";

            Assert.AreEqual(expectedTag, resultingTag);
            Assert.AreEqual(expectedCiphertext, resultingCiphertext);
        }

        [TestMethod, ExpectedException(typeof(CryptographicException))]
        public void EncryptWrongKeySizeTest()
        {
            string key = "00";
            string plaintext = "00000000000000000000000000000000";
            string iv = "000000000000000000000000";
            string aad = "";
            Library.Encrypt(key, plaintext, iv, aad);
        }

        [TestMethod]
        public void EncryptOptionalIvTest()
        {
            string key = "0000000000000000000000000000000000000000000000000000000000000000";
            string plaintext = "00000000000000000000000000000000";
            string iv = "";
            string aad = "";
            Tuple<string, string> returnValue = Library.Encrypt(key, plaintext, iv, aad);

            string resultingTag = returnValue.Item1;
            string resultingCiphertext = returnValue.Item2;

            string expectedTag = "d0d1c8a799996bf0265b98b5d48ab919";
            string expectedCiphertext = "cea7403d4d606b6e074ec5d3baf39d18";

            Assert.AreEqual(expectedTag, resultingTag);
            Assert.AreEqual(expectedCiphertext, resultingCiphertext);
        }

        [TestMethod]
        public void DecryptTestCase14Test()
        {
            string key = "0000000000000000000000000000000000000000000000000000000000000000";
            string cyphertext = "cea7403d4d606b6e074ec5d3baf39d18";
            string iv = "000000000000000000000000";
            string aad = "";
            string tag = "d0d1c8a799996bf0265b98b5d48ab919";

            string returnValue = Library.Decrypt(key,cyphertext, iv, aad, tag);

            string expectedPlaintext = "00000000000000000000000000000000";

            Assert.AreEqual(expectedPlaintext, returnValue);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void DecryptWrongTagTest()
        {
            string key = "0000000000000000000000000000000000000000000000000000000000000000";
            string cyphertext = "cea7403d4d606b6e074ec5d3baf39d18";
            string iv = "000000000000000000000000";
            string aad = "";
            string tag = "aa";

            Library.Decrypt(key, cyphertext, iv, aad, tag);
        }

        [TestMethod]
        public void DecryptOptionalIvTest()
        {
            string key = "0000000000000000000000000000000000000000000000000000000000000000";
            string cyphertext = "cea7403d4d606b6e074ec5d3baf39d18";
            string iv = "";
            string aad = "";
            string tag = "d0d1c8a799996bf0265b98b5d48ab919";

            string returnValue = Library.Decrypt(key, cyphertext, iv, aad, tag);

            string expectedPlaintext = "00000000000000000000000000000000";

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
