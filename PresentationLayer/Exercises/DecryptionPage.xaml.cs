using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
            HexPasswordBox.Text = Progress.GetProgress("DecryptionPage_HexPasswordBox") as string;
        }

        #endregion


        #region Private Methods
        private async void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
        {
            //key is evaluated and will be resized to 32 Byte if necessary 
            string keyString = Library.GenerateKey(UtfPasswordBox.Text);
            ChangeHexBox(keyString, HexPasswordBox);

            if (HexIvBox.Text == "")
            {
                HexIvBox.Text = "000000000000000000000000";
            }

            try
            {
                HexPlaintextBox.Text = await DecryptionTask(HexPasswordBox.Text, HexCiphertextBox.Text,
                    HexIvBox.Text, HexAadBox.Text, HexTagBox.Text);

                //case authentication only --> when successfull
                if (UtfPlaintextBox.Text == "")
                {
                    MessageBox.Show("Der Text wurde erfolgreich authentifiziert.", "alleinstehende Authentisierung",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (CryptographicException ex)
            {
                MessageBox.Show(
                    "Ein oder mehrere Parameter der Entschlüsselung wurde falsch eingegeben. Bitte überprüfen Sie Ihre Eingabe und passen Sie sie an.",
                    "Achtung", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                string triggeringField = "(Feld konnte leider nicht bestimmt werden)";
                foreach (var elem in DependencyObjectExtension.GetAllChildren<TextBox>(this))
                {
                    if (elem.Name.Contains("Hex"))
                    {
                        if (elem.Text.Length % 2 != 0)
                        {
                            triggeringField = elem.Name.Substring(3, elem.Name.Length - 6);
                        }
                    }
                }

                MessageBox.Show(
                    "In dem Feld " + triggeringField + " wurde ein ungerader Hex-Wert eingegeben. Bitte überprüfen Sie Ihre Eingabe und passen Sie sie an.",
                    "Achtung", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async Task<string> DecryptionTask(string key, string ciphertext, string iv, string aad, string tag)
        {
            return await Task.Run(() => Library.Decrypt(key, ciphertext, iv, aad, tag));
        }

        #endregion
    }
}
