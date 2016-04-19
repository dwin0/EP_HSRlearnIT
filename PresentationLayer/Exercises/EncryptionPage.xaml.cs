using System;
using System.Windows;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    public partial class EncryptionPage
    {
        #region Constructors
        public EncryptionPage()
        {
            InitializeComponent();
        }

        public EncryptionPage(string plaintext, string iv, string aad)
        {
            InitializeComponent();
            HexPlainTextBox.Text = plaintext;
            HexIvBox.Text = iv;
            HexAadBox.Text = aad;
        }
        #endregion


        #region Private Methods
        private void OnEnryptionButtonClick(object sender, RoutedEventArgs e)
        {
            GenerateHexKey(EncryptionPasswordBox.Text, HexEncryptionPasswordBox);
            byte[] key = HexStringToByteArray(HexEncryptionPasswordBox.Text);
            byte[] plaintext = HexStringToByteArray(HexPlainTextBox.Text);
            byte[] iv = null;
            if (HexIvBox.Text != "")
            {
                iv = HexStringToByteArray(HexIvBox.Text);
            }
            byte[] aad = HexStringToByteArray(HexAadBox.Text);

            Tuple<byte[], byte[]> returnValueEncryption = Library.Encrypt(key, plaintext, iv, aad);

            TagBox.Text = BytesToString(returnValueEncryption.Item1);
            CipherTextBox.Text = BytesToString(returnValueEncryption.Item2);
        }

        private void OnExportButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
        {
            string ciphertext = HexCipherTextBox.Text;
            string iv = HexIvBox.Text;
            string aad = HexAadBox.Text;
            string tag = HexTagBox.Text;
            NavigationService?.Navigate(new DecryptionPage(ciphertext, iv, aad, tag));
        }
        #endregion

    }
}
