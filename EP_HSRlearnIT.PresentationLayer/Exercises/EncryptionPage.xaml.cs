using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EP_HSRlearnIT.BusinessLayer.Persistence;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    /// <summary>
    /// Encryption page for the AesGcm Algorithm
    /// </summary>
    public partial class EncryptionPage
    {
        #region Constructors
        /// <summary>
        /// Initializes the EncryptionPage and loads the Progress
        /// </summary>
        public EncryptionPage()
        {
            InitializeComponent();

            TextBox[] textBoxesToInitialize = { HexIvBox, HexAadBox, HexPlaintextBox, HexPasswordBox };
            foreach (var textBox in textBoxesToInitialize)
            {
                textBox.Text = Progress.GetProgress("EncryptionPage_" + textBox.Name) as string;
            }
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
                ShowTooBigCharError();
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
                CheckIfHexIsOdd(this);
            }
        }

        private async Task<Tuple<string, string>> EncryptionTask(string key, string plaintext, string iv, string aad)
        {
            return await Task.Run(() => Library.Encrypt(key, plaintext, iv, aad));
        }

        #endregion

    }
}
