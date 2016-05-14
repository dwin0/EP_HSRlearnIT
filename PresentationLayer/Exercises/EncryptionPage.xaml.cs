using System;
using System.Threading.Tasks;
using System.Windows;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    public partial class EncryptionPage
    {
        #region Constructors
        public EncryptionPage()
        {
            InitializeComponent();
            HexPlaintextBox.Text = Progress.GetProgress("EncryptionPage_HexPlaintextBox") as string;
            HexIvBox.Text = Progress.GetProgress("EncryptionPage_HexIvBox") as string;
            HexAadBox.Text = Progress.GetProgress("EncryptionPage_HexAadBox") as string;
            HexEncryptionPasswordBox.Text = Progress.GetProgress("EncryptionPage_HexEncryptionPasswordBox") as string;
        }

        #endregion


        #region Private Methods
        private async void OnEnryptionButtonClick(object sender, RoutedEventArgs e)
        {
            //key is evaluated and will be resized to 32 Byte if necessary
            string keyString = Library.GenerateKey(UtfEncryptionPasswordBox.Text);
            ChangeHexBox(keyString, HexEncryptionPasswordBox);

            if (HexIvBox.Text == "")
            {
                HexIvBox.Text = "000000000000000000000000";
            }

            Tuple<string, string> returnValueEncryption = await EncryptionTask(HexEncryptionPasswordBox.Text, HexPlaintextBox.Text, HexIvBox.Text, HexAadBox.Text);
            HexTagBox.Text = returnValueEncryption.Item1;
            HexCiphertextBox.Text = returnValueEncryption.Item2;
        }

        private async Task<Tuple<string, string>> EncryptionTask(string key, string plaintext, string iv, string aad)
        {
            return await Task.Run(() => Library.Encrypt(key, plaintext, iv, aad));
        }

        #endregion
    }
}
