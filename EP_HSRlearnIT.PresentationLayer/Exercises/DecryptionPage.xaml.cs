using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EP_HSRlearnIT.BusinessLayer.Persistence;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    /// <summary>
    /// Decryption page for the AesGcm Algorithmus
    /// </summary>
    public partial class DecryptionPage
    {
        #region Constructors
        /// <summary>
        /// Initializes the DecryptionPage and loads the Progress for iv, aad, ciphertext, key and tag.
        /// </summary>
        public DecryptionPage()
        {
            InitializeComponent();

            TextBox[] textBoxesToInitialize = { HexIvBox, HexAadBox, HexCiphertextBox, HexPasswordBox, HexTagBox };
            foreach (var textBox in textBoxesToInitialize)
            {
                textBox.Text = Progress.GetProgress("DecryptionPage_" + textBox.Name) as string;
            }
        }

        #endregion

        #region Private Methods
        private async void DecryptionButton_OnClick(object sender, RoutedEventArgs e)
        {
            //key is evaluated and will be resized to 32 Byte if necessary 
            try
            {
                string keyString = Library.GenerateKey(UtfPasswordBox.Text);
                ChangeHexBox(keyString, HexPasswordBox);
            }
            catch (OverflowException)
            {
                ShowTooBigCharError();
                return;
            }

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
                    const string message = "Der Text wurde erfolgreich authentifiziert.";
                    const string title = "Authentisierung";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (CryptographicException)
            {
                const string message = "Ein oder mehrere Parameter der Entschlüsselung wurden falsch eingegeben. " +
                                       "Bitte überprüfe und korrigiere die Eingabe.";
                const string title = "Achtung";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (ArgumentOutOfRangeException)
            {
                CheckIfHexIsOdd(this);
            }
        }

        private async Task<string> DecryptionTask(string key, string ciphertext, string iv, string aad, string tag)
        {
            return await Task.Run(() => Library.Decrypt(key, ciphertext, iv, aad, tag));
        }

        #endregion
    }
}
