using System.Threading.Tasks;
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
            HexCiphertextBox.Text = Progress.GetProgress("DecryptionPage_HexCiphertextBox") as string;
            HexAadBox.Text = Progress.GetProgress("DecryptionPage_HexAadBox") as string;
            HexIvBox.Text = Progress.GetProgress("DecryptionPage_HexIvBox") as string;
            HexTagBox.Text = Progress.GetProgress("DecryptionPage_HexTagBox") as string;
            HexDecryptionPasswordBox.Text = Progress.GetProgress("DecryptionPage_HexDecryptionPasswordBox") as string;
        }

        #endregion


        #region Private Methods
        private async void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
        {
            //key is evaluated and will be resized to 32 Byte if necessary 
            string keyString = Library.GenerateKey(UtfDecryptionPasswordBox.Text);
            ChangeHexBox(keyString, HexDecryptionPasswordBox);

            if (HexIvBox.Text == "")
            {
                HexIvBox.Text = "000000000000000000000000";
            }

            HexPlaintextBox.Text = await DecryptionTask(HexDecryptionPasswordBox.Text, HexCiphertextBox.Text, HexIvBox.Text, HexAadBox.Text, HexTagBox.Text);

            //case authentication only --> when successfull
            if (UtfPlaintextBox.Text == "")
            {
                MessageBox.Show("Der Text wurde erfolgreich authentifiziert.", "alleinstehende Authentisierung",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async Task<string> DecryptionTask(string key, string ciphertext, string iv, string aad, string tag)
        {
            return await Task.Run(() => Library.Decrypt(key, ciphertext, iv, aad, tag));
        }

        #endregion
    }
}
