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
    public class AesGcmAdapter
    {
        #region Public Methods
        /// <summary>
        /// To encrypt, we need to know the key, the plaintext, iv and additional authenticated data (aad).
        /// The encryption is used like it is explained here: https://blogs.msdn.microsoft.com/b/shawnfa/archive/2009/03/17/authenticated-symmetric-encryption-in-net.aspx
        /// </summary>
        /// <param name="key">Has to be 32 Byte</param>
        /// <param name="plaintext">The plaintext will be encrypted.</param>
        /// <param name="iv">Is an opitional parameter, if the iv is null the default iv with 12 zero Bytes is set. If the iv is set it has to be 12 Bytes.</param>
        /// <param name="aad">Is an optional parameter, the algorithm can be used without it. The additional authenticated data will not be encrypted but are used in the process of the generation of the tag.</param>
        /// <returns>If sucessfull:
        ///          A tuple with the following parameters:
        ///          param1: is the generated tag, this parameter has to be used in the decryption and authentication
        ///          param2: is the generated ciphertext
        ///          Else an invalid argument exception will be thrown.
        /// </returns>
        public Tuple<string, string> Encrypt(string key, string plaintext, string iv, string aad)
        {
            byte[] byteKey = HexStringToDecimalByteArray(key);
            byte[] bytePlaintext = HexStringToDecimalByteArray(plaintext);
            byte[] byteIv = HexStringToDecimalByteArray(iv);
            byte[] byteAad = HexStringToDecimalByteArray(aad);

            using (AuthenticatedAesCng aes = new AuthenticatedAesCng())
            {

                //Setting gcm mode. This should be done before anything else, since propertys like tagsize depend upon the mode.
                aes.CngMode = CngChainingMode.Gcm;
                aes.Key = byteKey;

                //The iv must have a size of 12 Bytes.
                aes.IV = byteIv ?? new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //Aad is part of the generated tag. Is not part of the ciphertext, it won't be produced when decrypting.
                aes.AuthenticatedData = byteAad;

                //Perform the encryption.
                MemoryStream ms = new MemoryStream();
                IAuthenticatedCryptoTransform encryptor = aes.CreateAuthenticatedEncryptor();
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    //Encrypt the secret message.
                    cs.Write(bytePlaintext, 0, bytePlaintext.Length);

                    //Finish the encryption and get the output authentication tag and ciphertext.
                    cs.FlushFinalBlock();
                    string tagEncrypt = ConvertToHexString(BytesToString(encryptor.GetTag()));
                    string ciphertextEncrypt = ConvertToHexString(BytesToString(ms.ToArray()));
                    return new Tuple<string, string>(tagEncrypt, ciphertextEncrypt);
                }
            }
        }

        /// <summary>
        /// To decrypt, we need to know the key, the ciphertext, the authentication tag. The iv and additional authenticated data (aad) are optional.
        /// The decryption is used like it is explained here: https://blogs.msdn.microsoft.com/b/shawnfa/archive/2009/03/17/authenticated-symmetric-encryption-in-net.aspx
        /// </summary>
        /// <param name="key">Has to be the same key which was used to encrypt the plaintext.</param>
        /// <param name="ciphertext">This part of the parameters will be decrypted.</param>
        /// <param name="iv">If the iv was set in the encryption it has to be the same in the decryption. Else the default iv with 12 zero Bytes is used.</param>
        /// <param name="aad">If the aad was set in the encryption it has to be the same in the decryption, because the aad is used to reproduce the tag.</param>
        /// <param name="tag">The tag was an outputparameter of the encryption. If the wrong tag is used an exception will be thrown.</param>
        /// <returns>If sucessfull: The decrypted plaintext.
        ///          Else an exception will be thrown.
        /// </returns>
        public string Decrypt(string key, string ciphertext, string iv, string aad, string tag)
        {
            byte[] byteKey = HexStringToDecimalByteArray(key);
            byte[] byteCiphertext = HexStringToDecimalByteArray(ciphertext);
            byte[] byteIv = HexStringToDecimalByteArray(iv);
            byte[] byteAad = HexStringToDecimalByteArray(aad);
            byte[] byteTag = HexStringToDecimalByteArray(tag);

            using (AuthenticatedAesCng aes = new AuthenticatedAesCng())
            {
                //Chaining modes, keys, and IVs must match between encryption and decryption.
                aes.CngMode = CngChainingMode.Gcm;
                aes.Key = byteKey;
                aes.IV = byteIv ?? new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //If the aad doesn't match between encryption and decryption the tag will not match either and the decryption will fail.
                aes.AuthenticatedData = byteAad;

                //The tag that was generated during encryption gets set here.
                aes.Tag = byteTag;

                MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(byteCiphertext, 0, byteCiphertext.Length);

                    //If the authentication tag does not match, we’ll fail here with a CryptographicException, and the ciphertext will not be decrypted.
                    cs.FlushFinalBlock();

                    //returns the plaintext
                    return ConvertToHexString(BytesToString(ms.ToArray()));
                }
            }
        }

        /// <summary>
        /// The key needs to have a size of 32 Byte. This method generates a 32 Byte size key regardless of the input. Null value is not allowed.
        /// </summary>
        /// <param name="key">This key will be checked if the size is 32 Byte and if it's not 32 Byte a 32 Byte key will be generated</param>
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
        /// <param name="hex">The hex string that will be converted into a byte array.</param>
        /// <returns>Returns an Array which contains Hex Bytes</returns>
        public byte[] HexStringToDecimalByteArray(string hex)
        {
            if (hex == "")
            {
                return null;
            }
            else
            {
                return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
            }
        }

        /// <summary>
        /// A byte array can be converted into a string.
        /// </summary>
        /// <param name="bytes">The byte array which will be converted into a string.</param>
        /// <returns>Returns the converted string.</returns>
        public string BytesToString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            if (bytes != null)
            {
                foreach (byte b in bytes)
                {
                    sb.Append(Convert.ToChar(b));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// A non hex string can be convertet into a byte array.
        /// </summary>
        /// <param name="toConvert">A string which contains non hex characters.</param>
        /// <returns>Returns the converted byte array.</returns>
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

        public string ConvertToHexString(string values)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char letter in values)
            {
                int value = Convert.ToInt32(letter);
                sb.AppendFormat("{0:x2}", value);
            }
            return sb.ToString();
        }

        #endregion
    }
}
