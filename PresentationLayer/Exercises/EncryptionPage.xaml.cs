using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
        }

        public EncryptionPage(string plaintext, string iv, string aad)
        {
            InitializeComponent();
            HexPlaintextBox.Text = plaintext;
            HexIvBox.Text = iv;
            HexAadBox.Text = aad;
        }
        #endregion


        #region Private Methods
        private void OnEnryptionButtonClick(object sender, RoutedEventArgs e)
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

            Tuple<byte[], byte[]> returnValueEncryption = Library.Encrypt(key, plaintext, iv, aad);
            UtfTagBox.Text = Library.BytesToString(returnValueEncryption.Item1);
            UtfCiphertextBox.Text = Library.BytesToString(returnValueEncryption.Item2);
        }

        private void OnExportButtonClick(object sender, RoutedEventArgs e)
        {
            SaveMultiFileDialog saveMultiFileDialog = new SaveMultiFileDialog()
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            saveMultiFileDialog.Closed += new EventHandler(saveMultiFileDialog_Closed);
            saveMultiFileDialog.Show();
        }

        private void saveMultiFileDialog_Closed(object sender, EventArgs e)
        {
            string folderPath = Progress.GetProgress("SaveMultiFileDialog_ExportPath") as string;
            var exportFiles = Progress.GetProgress("SaveMultiFileDialog_Export") as Dictionary<string, string>;
            if (exportFiles != null)
            {
                foreach (var nameOfFiles in exportFiles)
                {
                    Console.WriteLine(nameOfFiles.Key + " is the key of " + nameOfFiles.Value);
                    TextBox textBox = FindName(nameOfFiles.Key + "Box") as TextBox;

                    if (textBox != null)
                    {
                        string newFileName = FileManager.SaveFile(folderPath, nameOfFiles.Value);
                        FileManager.UpdateFileContent(newFileName, textBox.Text);
                        Console.WriteLine(textBox.Text);
                        Progress.SaveProgress("EncryptionPage_" + nameOfFiles.Key, textBox.Text);
                    }
                }
                MessageBox.Show("Export war erfolgreich!", "Fileexport", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
        {
            string ciphertext = HexCiphertextBox.Text;
            string iv = HexIvBox.Text;
            string aad = HexAadBox.Text;
            string tag = HexTagBox.Text;
            NavigationService?.Navigate(new DecryptionPage(ciphertext, iv, aad, tag));
        }
        #endregion

    }
}
