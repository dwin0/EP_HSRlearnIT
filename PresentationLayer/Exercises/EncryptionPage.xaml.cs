using System;
using System.IO;
using System.Windows;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using EP_HSRlearnIT.Exercises;
using Microsoft.Win32;
using WPFFolderBrowser;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    public partial class EncryptionPage
    {
        #region Constructors
        public EncryptionPage()
        {
            InitializeComponent();
            HexIvBox.Text = Progress.GetProgress("EncryptionPage_HexIvBox") as string;
            HexAadBox.Text = Progress.GetProgress("EncryptionPage_HexAadBox") as string;
            HexPlainTextBox.Text = Progress.GetProgress("EncryptionPage_HexPlainTextBox") as string;
        }

        public EncryptionPage(string plaintext, string iv, string aad)
        {
            InitializeComponent();
            HexPlainTextBox.Text = plaintext;
            HexIvBox.Text = iv;
            HexAadBox.Text = aad;
        }
        #endregion


        #region Private Methods
        private void OnEnryptionButtonClick(object sender, RoutedEventArgs e)
        {
            //key is evaluated and will be resized to 32 Byte if necessary
            string keyString = Library.GenerateKey(EncryptionPasswordBox.Text);
            ChangeHexBox(keyString.ToCharArray(), HexEncryptionPasswordBox);

            //get the values of all fields which are needed to start the encryption
            byte[] key = Library.HexStringToByteArray(HexEncryptionPasswordBox.Text);
            byte[] plaintext = Library.HexStringToByteArray(HexPlainTextBox.Text);
            byte[] aad = Library.HexStringToByteArray(HexAadBox.Text);

            byte[] iv = null;
            if (HexIvBox.Text != "")
            {
                iv = Library.HexStringToByteArray(HexIvBox.Text);
            }
            else
            {
                HexIvBox.Text = "000000000000000000000000";
            }

            Tuple<byte[], byte[]> returnValueEncryption = Library.Encrypt(key, plaintext, iv, aad);
            TagBox.Text = Library.BytesToString(returnValueEncryption.Item1);
            CipherTextBox.Text = Library.BytesToString(returnValueEncryption.Item2);
        }

        private void OnExportButtonClick(object sender, RoutedEventArgs e)
        {
            //NavigationService?.Navigate(new SaveMultiFileDialog(HexCipherTextBox.Text));
            NavigationService?.Navigate(new SaveMultiFileDialog());
/*            WPFFolderBrowserDialog folderBrowserDialog = new WPFFolderBrowserDialog();
            folderBrowserDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (folderBrowserDialog.ShowDialog() == true)
            {
                Console.WriteLine(folderBrowserDialog.FileName);
            }*/

/*            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, HexCipherTextBox.Text);
            }*/
        }

        private void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
        {
            string ciphertext = HexCipherTextBox.Text;
            string iv = HexIvBox.Text;
            string aad = HexAadBox.Text;
            string tag = HexTagBox.Text;
            NavigationService?.Navigate(new DecryptionPage(ciphertext, iv, aad, tag));
        }
        #endregion

    }
}
