using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using System.Windows;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    public partial class DecryptionPage
    {
        #region Constructors
        public DecryptionPage()
        {
            InitializeComponent();
            HexCipherTextBox.Text = Progress.GetProgress("DecryptionPage_HexCipherTextBox") as string;
            HexAadBox.Text = Progress.GetProgress("DecryptionPage_HexAadBox") as string;
            HexIvBox.Text = Progress.GetProgress("DecryptionPage_HexIvBox") as string;
            HexTagBox.Text = Progress.GetProgress("DecryptionPage_HexTagBox") as string;
        }

        public DecryptionPage(string ciphertext, string iv, string aad, string tag)
        {
            InitializeComponent();
            HexCipherTextBox.Text = ciphertext;
            HexAadBox.Text = aad;
            HexIvBox.Text = iv;
            HexTagBox.Text = tag;
            Progress.SaveProgress("DecryptionPage_HexCipherTextBox", ciphertext);
            Progress.SaveProgress("DecryptionPage_HexAadBox", aad);
            Progress.SaveProgress("DecryptionPage_HexIvBox", iv);
            Progress.SaveProgress("DecryptionPage_HexTagBox", tag);
        }
        #endregion

        #region Private Methods
        private void OnImportButtonClick(object sender, RoutedEventArgs e)
        {

            
            string ciphertext = "";
            string aad = "";
            string iv = "";
            string tag = "";
            Progress.SaveProgress("DecryptionPage_HexCipherTextBox", ciphertext);
            Progress.SaveProgress("DecryptionPage_HexAadBox", aad);
            Progress.SaveProgress("DecryptionPage_HexIvBox", iv);
            Progress.SaveProgress("DecryptionPage_HexTagBox", tag);
        }

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
            if (PlainTextBox.Text == "")
            {
                ShowAadMessageBox(sender, e);
            }
        }

        private static void ShowAadMessageBox(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Der Text wurde erfolgreich authentifiziert.", "alleinstehenden Authentifizierung", MessageBoxButton.OK, MessageBoxImage.Information);
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
