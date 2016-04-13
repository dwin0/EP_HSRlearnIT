using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Security.Cryptography;
using System.Security.Cryptography;


namespace EP_HSRlearnIT.BusinessLayer.CryptoTools
{
    public class AesGcmCryptoLibrary
    {
        #region Private Members

        private byte[] _nonce;
        private byte[] _tag;
        private byte[] _ciphertext;

        #endregion

        #region Public Methods

        public byte[] Encrypt(string key, string plaintext, string nonce)
        {
            //byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] keyByte = StringToBytes(key);
            byte[] plaintextByte = Encoding.UTF8.GetBytes(plaintext);
            byte[] nonceByte = null;
            if (nonce != "")
            {
                nonceByte = Encoding.UTF8.GetBytes(nonce);
            }
            return _Encrypt(keyByte, plaintextByte, nonceByte);
        }

        private byte[] StringToBytes(string toConvert)
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


        public String Decrypt(string key, byte[] ciphertext)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] decryptedTextByte = _Decrypt(keyByte, ciphertext);
            String decryptedTextString = Encoding.UTF8.GetString(decryptedTextByte);
            return decryptedTextString;
        }

        #endregion


        #region Private Methods

        private byte[] _Encrypt(byte[] key, byte[] plaintext, byte[] nonce)
        {
            using (AuthenticatedAesCng aes = new AuthenticatedAesCng())
            {
                // Setup an authenticated chaining mode. This should be done before setting up
                // the other properties, since changing the chaining mode can update things such as the
                // valid and current tag sizes.
                aes.CngMode = CngChainingMode.Gcm;

                /*
                // Keys work the same as standard AES
                if(true)//key.GetLength(0) < 32)
                {
                    byte[] keyTest = new byte[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    aes.Key = keyTest;
                }
                */
              
                aes.Key = key;

                // The IV (called the nonce in many of the authenticated algorithm specs) is not sized for
                // the input block size. Instead its size depends upon the algorithm.  12 bytes works
                // for both GCM and CCM. Generate a random 12 byte nonce here.

                nonce = nonce ?? new byte[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                aes.IV = nonce;

                // Authenticated data becomes part of the authentication tag that is generated during
                // encryption, however it is not part of the ciphertext.  That is, when decrypting the
                // ciphertext the authenticated data will not be produced.  However, if the
                // authenticated data does not match at encryption and decryption time, the
                // authentication tag will not validate.
                //aes.AuthenticatedData = Encoding.UTF8.GetBytes("");

                // Perform the encryption – this works nearly the same as standard symmetric encryption,
                // however instead of using an ICryptoTransform we use an IAuthenticatedCryptoTrasform
                // which provides access to the authentication tag.
                using (MemoryStream ms = new MemoryStream())
                using (IAuthenticatedCryptoTransform encryptor = aes.CreateAuthenticatedEncryptor())
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    // Encrypt the secret message
                    //byte[] plaintext = Encoding.UTF8.GetBytes("Secret data to be encrypted and authenticated.");
                    cs.Write(plaintext, 0, plaintext.Length);

                    // Finish the encryption and get the output authentication tag and ciphertext
                    cs.FlushFinalBlock();
                    _tag = encryptor.GetTag();
                    _ciphertext = ms.ToArray();
                    return _ciphertext;
                }
            }
        }

        
        private byte[] _Decrypt(byte[] key, byte[] ciphertext)
        {
            // To decrypt, we need to know the nonce, key, additional authenticated data, and
            // authentication tag.
            using (AuthenticatedAesCng aes = new AuthenticatedAesCng())
            {
                // Chaining modes, keys, and IVs must match between encryption and decryption
                aes.CngMode = CngChainingMode.Gcm;

                if (true)//key.GetLength(0) < 32)
                {
                    byte[] keyTest = new byte[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    aes.Key = keyTest;
                }
                //aes.Key = key;

                _nonce = new byte[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                aes.IV = _nonce;

                // If the authenticated data does not match between encryption and decryption, then the
                // authentication tag will not match either, and the decryption operation will fail.
                //aes.AuthenticatedData = Encoding.UTF8.GetBytes("");

                // The tag that was generated during encryption gets set here as input to the decryption
                // operation.  This is in contrast to the encryption code path which does not use the
                // Tag property (since it is an output from encryption).
                aes.Tag = _tag;

                // Decryption works the same as standard symmetric encryption
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(ciphertext, 0, ciphertext.Length);

                    // If the authentication tag does not match, we’ll fail here with a
                    // CryptographicException, and the ciphertext will not be decrypted.
                    cs.FlushFinalBlock();

                    byte[] plaintext = ms.ToArray();
                    return plaintext;
                }
            }
        }

        #endregion  

    }
}
