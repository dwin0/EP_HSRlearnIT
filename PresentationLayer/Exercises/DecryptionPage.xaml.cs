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
            ChangeHexBox(keyString.ToCharArray(), HexDecryptionPasswordBox);

            //get the values of all fields which are needed to start the decryption
            byte[] key = Library.HexStringToDecimalByteArray(HexDecryptionPasswordBox.Text);
            byte[] ciphertext = Library.HexStringToDecimalByteArray(HexCiphertextBox.Text);
            byte[] aad = Library.HexStringToDecimalByteArray(HexAadBox.Text);
            byte[] tag = Library.HexStringToDecimalByteArray(HexTagBox.Text);

            byte[] iv = null;
            if (HexIvBox.Text != "")
            {
                iv = Library.HexStringToDecimalByteArray(HexIvBox.Text);
            }
            else
            {
                HexIvBox.Text = "000000000000000000000000";
            }

            UtfPlaintextBox.Text = Library.BytesToString(await DecryptionTask(key, ciphertext, iv, aad, tag));

            //case authentication only --> when successfull
            if (UtfPlaintextBox.Text == "")
            {
                MessageBox.Show("Der Text wurde erfolgreich authentifiziert.", "alleinstehenden Authentifizierung",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async Task<byte[]> DecryptionTask(byte[] key, byte[] ciphertext, byte[] iv, byte[] aad, byte[] tag)
        {
            return await Task.Run(() => Library.Decrypt(key, ciphertext, iv, aad, tag));
        }

        #endregion
    }
}
