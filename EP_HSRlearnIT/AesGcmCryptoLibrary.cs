using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP_HSRlearnIT
{
    internal class AesGcmCryptoLibrary
    {

        internal String Encrypt(string key, string plaintext)
        {
            return key+plaintext+"verschlüsselt";
        }

        internal String Decrypt(string key, string ciphertext)
        {
            return key+ciphertext+"entschlüsselt";
        }

    }
}
