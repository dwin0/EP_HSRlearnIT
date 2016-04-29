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
            //key is evaluated and will be resized to 32 Byte if necessary 
            string keyString = Library.GenerateKey(DecryptionPasswortBox.Text);
            ChangeHexBox(keyString.ToCharArray(), HexDecryptionPasswortBox);

            //get the values of all fields which are needed to start the decryption
            byte[] key = Library.HexStringToByteArray(HexDecryptionPasswortBox.Text);
            byte[] ciphertext = Library.HexStringToByteArray(HexCipherTextBox.Text);
            byte[] aad = Library.HexStringToByteArray(HexAadBox.Text);
            byte[] tag = Library.HexStringToByteArray(HexTagBox.Text);

            byte[] iv = null;
            if (HexIvBox.Text != "")
            {
                iv = Library.HexStringToByteArray(HexIvBox.Text);
            }
            else
            {
                HexIvBox.Text = "000000000000000000000000";
            }

            PlainTextBox.Text = Library.BytesToString(Library.Decrypt(key, ciphertext, iv, aad, tag));

            //case authentication only --> when successfull a Popup Window will be shown
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
