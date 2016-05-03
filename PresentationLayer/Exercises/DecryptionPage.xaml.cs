using System;
using System.Collections.Generic;
using System.IO;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using System.Windows;
using System.Windows.Controls;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    public partial class DecryptionPage
    {
        #region Constructors
        public DecryptionPage()
        {
            InitializeComponent();
            HexCiphertextBox.Text = Progress.GetProgress("DecryptionPage_HexCiphertextBox") as string;
            HexAadBox.Text = Progress.GetProgress("DecryptionPage_HexAadBox") as string;
            HexIvBox.Text = Progress.GetProgress("DecryptionPage_HexIvBox") as string;
            HexTagBox.Text = Progress.GetProgress("DecryptionPage_HexTagBox") as string;
        }

        public DecryptionPage(string ciphertext, string iv, string aad, string tag)
        {
            InitializeComponent();
            HexCiphertextBox.Text = ciphertext;
            HexAadBox.Text = aad;
            HexIvBox.Text = iv;
            HexTagBox.Text = tag;
            Progress.SaveProgress("DecryptionPage_HexCiphertextBox", ciphertext);
            Progress.SaveProgress("DecryptionPage_HexAadBox", aad);
            Progress.SaveProgress("DecryptionPage_HexIvBox", iv);
            Progress.SaveProgress("DecryptionPage_HexTagBox", tag);
        }

        #endregion

        #region Private Methods
        private void OnImportButtonClick(object sender, RoutedEventArgs e)
        {
/*            string ciphertext = "";
            string aad = "";
            string iv = "";
            string tag = "";*/

            OpenMultiFileDialog openMultiFileDialog = new OpenMultiFileDialog()
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            openMultiFileDialog.Closed += new EventHandler(openMultiFileDialog_Closed);
            openMultiFileDialog.Show();


/*            Progress.SaveProgress("DecryptionPage_HexCiphertextBox", ciphertext);
            Progress.SaveProgress("DecryptionPage_HexAadBox", aad);
            Progress.SaveProgress("DecryptionPage_HexIvBox", iv);
            Progress.SaveProgress("DecryptionPage_HexTagBox", tag);*/
        }

        private void openMultiFileDialog_Closed(object sender, EventArgs e)
        {
            //if (e == null) throw new ArgumentNullException(nameof(e));
            //if (e == null) return;
            var importFiles = Progress.GetProgress("OpenMultiFileDialog_Import") as Dictionary<string, string>;
            if (importFiles != null)
            {
                foreach (var fileName in importFiles)
                {
                    TextBox textBox = FindName("Hex" + fileName.Key + "Box") as TextBox;
                    if (textBox != null)
                    {
                        textBox.Text = File.ReadAllText(fileName.Value);
                        Progress.SaveProgress("DecryptionPage_Hex" + textBox.Name + "Box", textBox.Text);
                    }
                }
            }
        }

        private void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
        {
            //key is evaluated and will be resized to 32 Byte if necessary 
            string keyString = Library.GenerateKey(DecryptionPasswortBox.Text);
            ChangeHexBox(keyString.ToCharArray(), HexDecryptionPasswortBox);

            //get the values of all fields which are needed to start the decryption
            byte[] key = Library.HexStringToByteArray(HexDecryptionPasswortBox.Text);
            byte[] ciphertext = Library.HexStringToByteArray(HexCiphertextBox.Text);
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

            PlaintextBox.Text = Library.BytesToString(Library.Decrypt(key, ciphertext, iv, aad, tag));

            //case authentication only --> when successfull
            if (PlaintextBox.Text == "")
            {
                MessageBox.Show("Der Text wurde erfolgreich authentifiziert.", "alleinstehenden Authentifizierung",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnEncryptionButtonClick(object sender, RoutedEventArgs e)
        {
            string plaintext = HexPlaintextBox.Text;
            string iv = HexIvBox.Text;
            string aad = HexAadBox.Text;
            NavigationService?.Navigate(new EncryptionPage(plaintext, iv, aad));
        }

        #endregion
    }
}
