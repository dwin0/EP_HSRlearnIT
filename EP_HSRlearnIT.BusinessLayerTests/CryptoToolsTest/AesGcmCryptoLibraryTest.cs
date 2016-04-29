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
            AesGcmCryptoLibrary library = new AesGcmCryptoLibrary();
            byte[] key = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            byte[] plaintext = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            byte[] iv = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] aad = {};
            Tuple<byte[], byte[]> returnValue = library.Encrypt(key, plaintext, iv, aad);

            byte[] resultingTag = returnValue.Item1;
            byte[] resultingCiphertext = returnValue.Item2;

            byte[] expectedTag = library.HexStringToByteArray("d0d1c8a799996bf0265b98b5d48ab919");
            byte[] expectedCiphertext = library.HexStringToByteArray("cea7403d4d606b6e074ec5d3baf39d18");

            CollectionAssert.AreEqual(expectedTag, resultingTag);
            CollectionAssert.AreEqual(expectedCiphertext, resultingCiphertext);
        }

        [TestMethod, ExpectedException(typeof(CryptographicException))]
        public void EncryptWrongKeySizeTest()
        {
            AesGcmCryptoLibrary library = new AesGcmCryptoLibrary();
            byte[] key = { 0 };
            byte[] plaintext = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] iv = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] aad = { };
            library.Encrypt(key, plaintext, iv, aad);
        }

        [TestMethod]
        public void EncryptOptionalIvTest()
        {
            AesGcmCryptoLibrary library = new AesGcmCryptoLibrary();
            byte[] key = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] plaintext = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] iv = null;
            byte[] aad = { };
            Tuple<byte[], byte[]> returnValue = library.Encrypt(key, plaintext, iv, aad);

            byte[] resultingTag = returnValue.Item1;
            byte[] resultingCiphertext = returnValue.Item2;

            byte[] expectedTag = library.HexStringToByteArray("d0d1c8a799996bf0265b98b5d48ab919");
            byte[] expectedCiphertext = library.HexStringToByteArray("cea7403d4d606b6e074ec5d3baf39d18");

            CollectionAssert.AreEqual(expectedTag, resultingTag);
            CollectionAssert.AreEqual(expectedCiphertext, resultingCiphertext);
        }

        [TestMethod]
        public void DecryptTestCAse14Test()
        {
            AesGcmCryptoLibrary library = new AesGcmCryptoLibrary();
            byte[] key = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] cyphertext = library.HexStringToByteArray("cea7403d4d606b6e074ec5d3baf39d18");
            byte[] iv = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] aad = {};
            byte[] tag = library.HexStringToByteArray("d0d1c8a799996bf0265b98b5d48ab919");

            byte[] returnValue = library.Decrypt(key,cyphertext, iv, aad, tag);

            byte[] expectedPlaintext = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            CollectionAssert.AreEqual(expectedPlaintext, returnValue);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void DecryptWrongTagTest()
        {
            AesGcmCryptoLibrary library = new AesGcmCryptoLibrary();
            byte[] key = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] cyphertext = library.HexStringToByteArray("cea7403d4d606b6e074ec5d3baf39d18");
            byte[] iv = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] aad = { };
            byte[] tag = library.HexStringToByteArray("aa");

            library.Decrypt(key, cyphertext, iv, aad, tag);
        }

        [TestMethod]
        public void DecryptOptionalIvTest()
        {
            AesGcmCryptoLibrary library = new AesGcmCryptoLibrary();
            byte[] key = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] cyphertext = library.HexStringToByteArray("cea7403d4d606b6e074ec5d3baf39d18");
            byte[] iv = null;
            byte[] aad = { };
            byte[] tag = library.HexStringToByteArray("d0d1c8a799996bf0265b98b5d48ab919");

            byte[] returnValue = library.Decrypt(key, cyphertext, iv, aad, tag);

            byte[] expectedPlaintext = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            CollectionAssert.AreEqual(expectedPlaintext, returnValue);
        }

        [TestMethod]
        public void GenerateHexKeyWithLessInputTest()
        {
            AesGcmCryptoLibrary library = new AesGcmCryptoLibrary();
            string resultingKey = library.GenerateKey("TestTest");
            Assert.AreEqual("TestTestTestTestTestTestTestTest", resultingKey);
        }

        [TestMethod]
        public void GenerateHexKeyWithToMuchInputTest()
        {
            AesGcmCryptoLibrary library = new AesGcmCryptoLibrary();
            string resultingKey = library.GenerateKey("TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest");
            Assert.AreEqual("TestTestTestTestTestTestTestTest", resultingKey);
        }


    }
}
