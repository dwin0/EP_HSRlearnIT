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
            ChangeHexBox(keyString.ToCharArray(), HexEncryptionPasswordBox);

            //get the values of all fields which are needed to start the encryption
            byte[] key = Library.HexStringToDecimalByteArray(HexEncryptionPasswordBox.Text);
            byte[] plaintext = Library.HexStringToDecimalByteArray(HexPlaintextBox.Text);
            byte[] aad = Library.HexStringToDecimalByteArray(HexAadBox.Text);

            byte[] iv = null;
            if (HexIvBox.Text != "")
            {
                iv = Library.HexStringToDecimalByteArray(HexIvBox.Text);
            }
            else
            {
                HexIvBox.Text = "000000000000000000000000";
            }

            Tuple<byte[], byte[]> returnValueEncryption = await EncryptionTask(key, plaintext, iv, aad);
            UtfTagBox.Text = Library.BytesToString(returnValueEncryption.Item1);
            UtfCiphertextBox.Text = Library.BytesToString(returnValueEncryption.Item2);
        }

        private async Task<Tuple<byte[], byte[]>> EncryptionTask(byte[] key, byte[] plaintext, byte[] iv, byte[] aad)
        {
            return await Task.Run(() => Library.Encrypt(key, plaintext, iv, aad));
        }


        #endregion
    }
}
