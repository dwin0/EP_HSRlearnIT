using System;
using System.Security.Cryptography;
using EP_HSRlearnIT.BusinessLayer.CryptoTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EP_HSRlearnIT.BusinessLayerTests.CryptoToolsTest
{
    [TestClass]
    public class AesGcmCryptoLibraryTest
    {
        [TestMethod]
        public void EncryptTestCase14Test()
        {
            AesGcmAdapter library = new AesGcmAdapter();
            string key = "0000000000000000000000000000000000000000000000000000000000000000";
            string plaintext = "00000000000000000000000000000000";
            string iv = "000000000000000000000000";
            string aad = "";
            Tuple<string, string> returnValue = library.Encrypt(key, plaintext, iv, aad);

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
            AesGcmAdapter library = new AesGcmAdapter();
            string key = "00";
            string plaintext = "00000000000000000000000000000000";
            string iv = "000000000000000000000000";
            string aad = "";
            library.Encrypt(key, plaintext, iv, aad);
        }

        [TestMethod]
        public void EncryptOptionalIvTest()
        {
            AesGcmAdapter library = new AesGcmAdapter();
            string key = "0000000000000000000000000000000000000000000000000000000000000000";
            string plaintext = "00000000000000000000000000000000";
            string iv = "";
            string aad = "";
            Tuple<string, string> returnValue = library.Encrypt(key, plaintext, iv, aad);

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
            AesGcmAdapter library = new AesGcmAdapter();
            string key = "0000000000000000000000000000000000000000000000000000000000000000";
            string cyphertext = "cea7403d4d606b6e074ec5d3baf39d18";
            string iv = "000000000000000000000000";
            string aad = "";
            string tag = "d0d1c8a799996bf0265b98b5d48ab919";

            string returnValue = library.Decrypt(key,cyphertext, iv, aad, tag);

            string expectedPlaintext = "00000000000000000000000000000000";

            Assert.AreEqual(expectedPlaintext, returnValue);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void DecryptWrongTagTest()
        {
            AesGcmAdapter library = new AesGcmAdapter();
            string key = "0000000000000000000000000000000000000000000000000000000000000000";
            string cyphertext = "cea7403d4d606b6e074ec5d3baf39d18";
            string iv = "000000000000000000000000";
            string aad = "";
            string tag = "aa";

            library.Decrypt(key, cyphertext, iv, aad, tag);
        }

        [TestMethod]
        public void DecryptOptionalIvTest()
        {
            AesGcmAdapter library = new AesGcmAdapter();
            string key = "0000000000000000000000000000000000000000000000000000000000000000";
            string cyphertext = "cea7403d4d606b6e074ec5d3baf39d18";
            string iv = "";
            string aad = "";
            string tag = "d0d1c8a799996bf0265b98b5d48ab919";

            string returnValue = library.Decrypt(key, cyphertext, iv, aad, tag);

            string expectedPlaintext = "00000000000000000000000000000000";

            Assert.AreEqual(expectedPlaintext, returnValue);
        }

        [TestMethod]
        public void GenerateHexKeyWithLessInputTest()
        {
            AesGcmAdapter library = new AesGcmAdapter();
            string resultingKey = library.GenerateKey("TestTest");
            Assert.AreEqual("TestTestTestTestTestTestTestTest", resultingKey);
            Assert.AreEqual(32, resultingKey.Length);
        }

        [TestMethod]
        public void GenerateHexKeyWithToMuchInputTest()
        {
            AesGcmAdapter library = new AesGcmAdapter();
            string resultingKey = library.GenerateKey("TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest");
            Assert.AreEqual("TestTestTestTestTestTestTestTest", resultingKey);
            Assert.AreEqual(32, resultingKey.Length);
        }

        [TestMethod]
        public void GenerateHexKeyWithRightInputTest()
        {
            AesGcmAdapter library = new AesGcmAdapter();
            string resultingKey = library.GenerateKey("TestTestTestTestTestTestTestTest");
            Assert.AreEqual("TestTestTestTestTestTestTestTest", resultingKey);
            Assert.AreEqual(32, resultingKey.Length);
        }

        [TestMethod]
        public void HexStringToDecimalByteArrayTest()
        {
            AesGcmAdapter library = new AesGcmAdapter();
            byte[] resultingHex = library.HexStringToDecimalByteArray("54657374");
            byte[] expectedHex = { 84, 101, 115, 116 };

            CollectionAssert.AreEqual(expectedHex, resultingHex);
        }

    }
}
