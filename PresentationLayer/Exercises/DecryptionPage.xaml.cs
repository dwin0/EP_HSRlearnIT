using System;
using System.Windows;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    public partial class DecryptionPage
    {
        #region Constructors
        public DecryptionPage()
        {
            InitializeComponent();
        }

        public DecryptionPage(string ciphertext, string nonce, string aad, string tag)
        {
            InitializeComponent();
            HexCipherTextBox.Text = ciphertext;
            HexIvBox.Text = nonce;
            HexAadBox.Text = aad;
            HexTagBox.Text = tag;
        }
        #endregion

        #region Private Methods
        private void OnImportButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
        {
            GenerateHexKey(DecryptionPasswortBox.Text, HexDecryptionPasswortBox);
            byte[] key = HexStringToByteArray(HexDecryptionPasswortBox.Text);
            byte[] ciphertext = HexStringToByteArray(HexCipherTextBox.Text);
            byte[] nonce = null;
            if (HexIvBox.Text != "")
            {
                nonce = HexStringToByteArray(HexIvBox.Text);
            }
            byte[] aad = HexStringToByteArray(HexAadBox.Text);
            byte[] tag = HexStringToByteArray(HexTagBox.Text);
            PlainTextBox.Text = BytesToString(Library.Decrypt(key, ciphertext, nonce, aad, tag));
        }

        private void OnEncryptionButtonClick(object sender, RoutedEventArgs e)
        {
            string plaintext = HexPlainTextBox.Text;
            string nonce = HexIvBox.Text;
            string aad = HexAadBox.Text;
            NavigationService?.Navigate(new EncryptionPage(plaintext, nonce, aad));
        }
        #endregion

    }
}
