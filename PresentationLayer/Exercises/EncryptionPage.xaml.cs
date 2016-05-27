using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EP_HSRlearnIT.BusinessLayer.Persistence;
using EP_HSRlearnIT.BusinessLayer.Extensions;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    /// <summary>
    /// Encryption page for the AesGcm Algorithmus
    /// </summary>
    public partial class EncryptionPage
    {
        #region Constructors
        /// <summary>
        /// Initializes the EncryptionPage and loads the Progress for iv, aad, plaintext and key.
        /// </summary>
        public EncryptionPage()
        {
            InitializeComponent();
            HexIvBox.Text = Progress.GetProgress("EncryptionPage_HexIvBox") as string;
            HexAadBox.Text = Progress.GetProgress("EncryptionPage_HexAadBox") as string;
            HexPlaintextBox.Text = Progress.GetProgress("EncryptionPage_HexPlaintextBox") as string;
            HexPasswordBox.Text = Progress.GetProgress("EncryptionPage_HexPasswordBox") as string;
        }

        #endregion


        #region Private Methods
        private async void EnryptionButton_OnClick(object sender, RoutedEventArgs e)
        {
            //key is evaluated and will be resized to 32 Byte if necessary
            try
            {
                string keyString = Library.GenerateKey(UtfPasswordBox.Text);
                ChangeHexBox(keyString, HexPasswordBox);
            }
            catch (OverflowException)
            {
                string message = "Es wurde ein Zeichen, welches mit mehr als einem Byte repräsentiert wird, eingegeben. " +
                                 "Bitte überprüfe und korrigiere die Eingabe.";
                string title = "Achtung";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            if (HexIvBox.Text == "")
            {
                HexIvBox.Text = "000000000000000000000000";
            }
            try
            {
                Tuple<string, string> returnValueEncryption = await EncryptionTask(HexPasswordBox.Text, HexPlaintextBox.Text, 
                    HexIvBox.Text, HexAadBox.Text);
                HexTagBox.Text = returnValueEncryption.Item1;
                HexCiphertextBox.Text = returnValueEncryption.Item2;
            }
            catch (ArgumentOutOfRangeException)
            {
                string triggeringField = "(konnte nicht bestimmt werden)";
                foreach (var elem in DependencyObjectExtension.GetAllChildren<TextBox>(this))
                {
                    if (elem.Name.Contains("Hex") && (elem.Text.Length % 2 != 0))
                    {
                        triggeringField = elem.Name.Substring(3, elem.Name.Length - 6);
                    }
                }

                string message = "Im Feld " + triggeringField + " wurde ein ungerader Hex-Wert eingegeben. " +
                                 "Bitte überprüfe und korrigiere die Eingabe.";
                string title = "Achtung";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async Task<Tuple<string, string>> EncryptionTask(string key, string plaintext, string iv, string aad)
        {
            return await Task.Run(() => Library.Encrypt(key, plaintext, iv, aad));
        }

        #endregion

    }
}
