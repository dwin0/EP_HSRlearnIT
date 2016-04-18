using System;
using System.IO;
using Security.Cryptography;
using System.Security.Cryptography;

namespace EP_HSRlearnIT.BusinessLayer.CryptoTools
{
    public class AesGcmCryptoLibrary
    {
        #region Public Methods
        public Tuple<byte[], byte[]> Encrypt(byte[] key, byte[] plaintext, byte[] nonce, byte[] aad)
        {
            using (AuthenticatedAesCng aes = new AuthenticatedAesCng())
            {
                // Setup an authenticated chaining mode. This should be done before setting up
                // the other properties, since changing the chaining mode can update things such as the
                // valid and current tag sizes.
                aes.CngMode = CngChainingMode.Gcm;

                // Keys work the same as standard AES
                aes.Key = key;

                // The IV (called the nonce in many of the authenticated algorithm specs) is not sized for
                // the input block size. Instead its size depends upon the algorithm. 12 bytes works
                // for both GCM and CCM.
                aes.IV = nonce ?? new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                // Authenticated data becomes part of the authentication tag that is generated during
                // encryption, however it is not part of the ciphertext. That is, when decrypting the
                // ciphertext the authenticated data will not be produced. However, if the
                // authenticated data does not match at encryption and decryption time, the
                // authentication tag will not validate.
                aes.AuthenticatedData = aad;

                // Perform the encryption – this works nearly the same as standard symmetric encryption,
                // however instead of using an ICryptoTransform we use an IAuthenticatedCryptoTransform
                // which provides access to the authentication tag.
                MemoryStream ms = new MemoryStream();
                IAuthenticatedCryptoTransform encryptor = aes.CreateAuthenticatedEncryptor();
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    // Encrypt the secret message
                    cs.Write(plaintext, 0, plaintext.Length);

                    // Finish the encryption and get the output authentication tag and ciphertext
                    cs.FlushFinalBlock();
                    byte[] tagEncrypt = encryptor.GetTag();
                    byte[] ciphertextEncrypt = ms.ToArray();
                    return new Tuple<byte[], byte[]>(tagEncrypt, ciphertextEncrypt);
                }
            }
        }

        public byte[] Decrypt(byte[] key, byte[] ciphertext, byte[] nonce, byte[] aad, byte[] tag)
        {
            // To decrypt, we need to know the key, the ciphertext, nonce, additional authenticated data(aad)
            // and the authentication tag.
            using (AuthenticatedAesCng aes = new AuthenticatedAesCng())
            {
                // Chaining modes, keys, and IVs must match between encryption and decryption
                aes.CngMode = CngChainingMode.Gcm;
                aes.Key = key;
                aes.IV = nonce ?? new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                // If the authenticated data does not match between encryption and decryption, then the
                // authentication tag will not match either, and the decryption operation will fail.
                aes.AuthenticatedData = aad;

                // The tag that was generated during encryption gets set here as input to the decryption
                // operation. This is in contrast to the encryption code path which does not use the
                // Tag property (since it is an output from encryption).
                aes.Tag = tag;

                // Decryption works the same as standard symmetric encryption
                MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(ciphertext, 0, ciphertext.Length);

                    // If the authentication tag does not match, we’ll fail here with a
                    // CryptographicException, and the ciphertext will not be decrypted.
                    cs.FlushFinalBlock();

                    //returns the plaintext
                    return ms.ToArray();
                }
            }
        }

        #endregion
    }
}
