﻿using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<Tuple<byte[], byte[]>> EncryptionTask(byte[] key, byte[] plaintext, byte[] iv, byte[] aad)
        {
            return await Task.Run(() => Library.Encrypt(key, plaintext, iv, aad));
        }

        private void OnExportButtonClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                OverwritePrompt = true,
                AddExtension = true,
                DefaultExt = "txt",
                ValidateNames = true,
                Title = "Exportieren der Verschlüsselungsparamter"
            };

            if (saveFileDialog.ShowDialog() == true)
            {

                string fullFilePath = saveFileDialog.FileName;
                var extension = Path.GetExtension(fullFilePath);
                if (extension != null && extension.ToLower() != ".txt")
                {
                    fullFilePath += ".txt";
                }
         
                if (!FileManager.IsExist(fullFilePath))
                {
                    FileManager.SaveFile(fullFilePath);
                }
                //Fillup all fields with name of parameter and value into exportfile.
                StringBuilder line = new StringBuilder();
                foreach (TextBox element in DependencyObjectExtension.GetAllChildren<TextBox>(this))
                {
                    //Filter for fields to export
                    if (element.Name.Contains("Hex"))
                    {
                        //cut Hex and Box, for Example: HexIvBox -> Iv
                        line.AppendLine(element.Name.Substring(3, element.Name.Length-6) + "=0x" + element.Text);
                    }
                }
                FileManager.UpdateContent(fullFilePath, line.ToString());
                MessageBox.Show("Der Export war erfolgreich!", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion
    }
}
