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

        public DecryptionPage(string ciphertext, string iv, string aad, string tag)
        {
            InitializeComponent();
            HexCipherTextBox.Text = ciphertext;
            HexIvBox.Text = iv;
            HexAadBox.Text = aad;
            HexTagBox.Text = tag;
        }
        #endregion

        #region Private Methods
        private void OnImportButtonClick(object sender, RoutedEventArgs e)
        {

        }

        //TODO fall nur Aad behandeln!
        private void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
        {
            GenerateHexKey(DecryptionPasswortBox.Text, HexDecryptionPasswortBox);
            byte[] key = HexStringToByteArray(HexDecryptionPasswortBox.Text);
            byte[] ciphertext = HexStringToByteArray(HexCipherTextBox.Text);
            byte[] iv = null;
            if (HexIvBox.Text != "")
            {
                iv = HexStringToByteArray(HexIvBox.Text);
            }
            else
            {
                HexIvBox.Text = "000000000000000000000000";
            }
            byte[] aad = HexStringToByteArray(HexAadBox.Text);
            byte[] tag = HexStringToByteArray(HexTagBox.Text);
            PlainTextBox.Text = BytesToString(Library.Decrypt(key, ciphertext, iv, aad, tag));
        }

        private void OnEncryptionButtonClick(object sender, RoutedEventArgs e)
        {
            string plaintext = HexPlainTextBox.Text;
            string iv = HexIvBox.Text;
            string aad = HexAadBox.Text;
            NavigationService?.Navigate(new EncryptionPage(plaintext, iv, aad));
        }
        #endregion

    }
}
