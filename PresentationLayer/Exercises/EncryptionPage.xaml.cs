using EP_HSRlearnIT.BusinessLayer.CryptoTools;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    /// <summary>
    /// Interaction logic for EncryptionPage.xaml
    /// </summary>
    public partial class EncryptionPage : Page
    {
        #region Private Members

        private AesGcmCryptoLibrary _library;

        #endregion


        #region Constructors

        public EncryptionPage()
        {
            InitializeComponent();
            _library = new AesGcmCryptoLibrary();
        }

        #endregion


        #region Private Methods

        private void OnEnryptionButtonClick(object sender, RoutedEventArgs e)
        {
            byte[] ciphertext = _library.Encrypt(EncryptionPasswordBox.Text, PlainTextBox.Text);
            CipherTextBox.Text = BytesToString(ciphertext);
        }

       /* private void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
        {
            PlainTextBox.Text = _library.Decrypt(DecryptionPasswordBox.Text, StringToBytes(CipherTextBox.Text));
        }
        */

        private string BytesToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length];

            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (byte b in bytes)
            {
                chars[i] = Convert.ToChar(b);
                sb.Append(chars[i]);
                i++;
            }

            return sb.ToString();
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

        #endregion
    }
}
