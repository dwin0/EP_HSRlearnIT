using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using Microsoft.Win32;

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
            Progress.SaveProgress("EncryptionPage_HexPlaintextBox", plaintext);
            Progress.SaveProgress("EncryptionPage_HexIvBox", iv);
            Progress.SaveProgress("EncryptionPage_HexAadBox", aad);
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
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                OverwritePrompt = true,
                AddExtension = true,
                ValidateNames = true,
                Title = "Exportieren der Verschlüsselungsparamter"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string fullFilePath = saveFileDialog.FileName;
                Console.WriteLine(fullFilePath);
                if (!FileManager.IsExist(fullFilePath))
                {
                    FileManager.SaveFile(fullFilePath);
                }

                StringBuilder line = new StringBuilder();

                var listOfTextBox = new List<TextBox>();
                foreach (var element in DependencyObjectExtension.GetAllChildren<TextBox>(this))
                {
                        listOfTextBox.Add(element);
                        Console.WriteLine("My name is " + element.Name.Substring(3, element.Name.Length - 6));
                }
                foreach (TextBox textBox in listOfTextBox)
                {
                    if (textBox.Name.Contains("Hex") && !(textBox.Name.Contains("Password")))
                    {
                        //cut Hex and Box, for Example: HexIvBox -> Iv
                        line.AppendLine(textBox.Name.Substring(3, textBox.Name.Length-6) + "=0x" + textBox.Text);
                    }
                }
                FileManager.UpdateFileContent(fullFilePath, line.ToString());
                MessageBox.Show("Der Export war erfolgreich!", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
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
