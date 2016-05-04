using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using Castle.Core.Internal;

namespace EP_HSRlearnIT.BusinessLayer.CryptoTools
{
    public class AesGcmCryptoLibrary
    {
        #region Public Methods
        /// <summary>
        /// To encrypt, we need to know the key, the plaintext, optional iv and optional additional authenticated data(aad).
        /// The encryption is used like it is explained here: https://blogs.msdn.microsoft.com/b/shawnfa/archive/2009/03/17/authenticated-symmetric-encryption-in-net.aspx
        /// </summary>
        /// <param name="key"></param>
        /// <param name="plaintext"></param>
        /// <param name="iv"></param>
        /// <param name="aad"></param>
        /// <returns></returns>
        public Tuple<byte[], byte[]> Encrypt(byte[] key, byte[] plaintext, byte[] iv, byte[] aad)
        {
            using (AuthenticatedAesCng aes = new AuthenticatedAesCng())
            {
                //Setting gcm mode. This should be done before anything else, since propertys like tagsize depend upon the mode.
                aes.CngMode = CngChainingMode.Gcm;
                aes.Key = key;

                //The iv must have a size of 12 Bytes.
                aes.IV = iv ?? new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //Aad is part of the generated tag. Is not part of the ciphertext, it won't be produced when decrypting.
                aes.AuthenticatedData = aad;

                //Perform the encryption.
                MemoryStream ms = new MemoryStream();
                IAuthenticatedCryptoTransform encryptor = aes.CreateAuthenticatedEncryptor();
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    //Encrypt the secret message.
                    cs.Write(plaintext, 0, plaintext.Length);

                    //Finish the encryption and get the output authentication tag and ciphertext.
                    cs.FlushFinalBlock();
                    byte[] tagEncrypt = encryptor.GetTag();
                    byte[] ciphertextEncrypt = ms.ToArray();
                    return new Tuple<byte[], byte[]>(tagEncrypt, ciphertextEncrypt);
                }
            }
        }

        /// <summary>
        /// To decrypt, we need to know the key, the ciphertext, the authentication tag. The iv and additional authenticated data (aad) are optional.
        /// The decryption is used like it is explained here: https://blogs.msdn.microsoft.com/b/shawnfa/archive/2009/03/17/authenticated-symmetric-encryption-in-net.aspx
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ciphertext"></param>
        /// <param name="iv"></param>
        /// <param name="aad"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] key, byte[] ciphertext, byte[] iv, byte[] aad, byte[] tag)
        {
            using (AuthenticatedAesCng aes = new AuthenticatedAesCng())
            {
                //Chaining modes, keys, and IVs must match between encryption and decryption.
                aes.CngMode = CngChainingMode.Gcm;
                aes.Key = key;
                aes.IV = iv ?? new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //If the aad doesn't match between encryption and decryption the tag will not match either and the decryption will fail.
                aes.AuthenticatedData = aad;

                //The tag that was generated during encryption gets set here.
                aes.Tag = tag;

                MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(ciphertext, 0, ciphertext.Length);

                    //If the authentication tag does not match, we’ll fail here with a CryptographicException, and the ciphertext will not be decrypted.
                    cs.FlushFinalBlock();

                    //returns the plaintext
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// The key needs to have a size of 32 Byte. This method generates a 32 Byte size key regardless of the input. Null value is not allowed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Returns the generated 32 Byte Key</returns>
        public string GenerateKey(string key)
        {
            byte[] keyArray = StringToBytes(key);
            int keySize = keyArray.Length;

            IEnumerable<byte> bigKey = keyArray;

            if (keySize < 32)
            {
                for (int i = 1; i <= (32 / keySize); i++)
                {
                    bigKey = bigKey.Concat(keyArray);
                }
            }

            bigKey = bigKey.Take(32);
            byte[] result = new byte[32];
            int counter = 0;

            bigKey.ForEach(i =>
            {
                byte b = i;
                result[counter] = b;
                counter++;
            });

            return BytesToString(result);
        }

        /// <summary>
        /// A hex string can be converted into a byte array with this Method.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns>Returns an Array which contains Hex Bytes</returns>
        public byte[] HexStringToDecimalByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        /// <summary>
        /// A byte array can be converted into a string.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string BytesToString(byte[] bytes)
        {
            //char[] chars = new char[bytes.Length];

            StringBuilder sb = new StringBuilder();
            //int i = 0;
            foreach (byte b in bytes)
            {
                /*
                chars[i] = Convert.ToChar(b);
                sb.Append(chars[i]);
                i++;
                */
                sb.Append(Convert.ToChar(b));
            }
            return sb.ToString();
        }

        /// <summary>
        /// A non hex string can be convertet into a byte array.
        /// </summary>
        /// <param name="toConvert"></param>
        /// <returns></returns>
        public byte[] StringToBytes(string toConvert)
        {
            byte[] bytes = new byte[toConvert.Length];

            int i = 0;
            foreach (char c in toConvert)
            {
                bytes[i] = Convert.ToByte(c);
                i++;
            }

            return bytes;
        }

        #endregion
    }
}
