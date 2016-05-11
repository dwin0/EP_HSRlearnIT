using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

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
        }

        #endregion


        #region Private Methods
        private void OnImportButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Importieren der Entschlüsselungsparameter"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                IEnumerable<string> allLines = FileManager.ReadAllLines(filePath);

                foreach (string line in allLines)
                {
                    int index = line.IndexOf(Convert.ToChar('='));
                    //the first '=' is only a delimeter symbole and not part of parameter or value
                    string parameter = line.Substring(0, index-1);
                    string value = line.Substring(index + 1);
                    FillingField(parameter, value);
                }
            }
            MessageBox.Show("Der Import wurde erfolgreich abgeschlossen.", "Import einer Parameterdatei", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Method filling all fields from the importfile excluding the following parameternames.
        /// </summary>
        /// <param name="parameter"></param>fieldidentifier from file.
        /// <param name="value"></param>value from parametername.
        private void FillingField(string parameter, string value)
        {
            if (value.Substring(0, 2).Equals("0x"))
            {
                parameter = "Hex" + parameter;
                value = value.Substring(2);
            }
            if (value.Length > 0)
            {
                foreach (var element in DependencyObjectExtension.GetAllChildren<TextBox>(this))
                {
                    if (element.Name.Contains(parameter) && !(element.Name.Contains("Plaintext") || element.Name.Contains("Password")))
                    {
                        TextBox fieldOnForm = FindName(element.Name) as TextBox;
                        if (fieldOnForm != null)
                        {
                            fieldOnForm.Text = value;
                        }
                        break;
                    }
                }
            }
        }

        private async void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
        {
            //key is evaluated and will be resized to 32 Byte if necessary 
            string keyString = Library.GenerateKey(UtfDecryptionPasswortBox.Text);
            ChangeHexBox(keyString.ToCharArray(), HexDecryptionPasswortBox);

            //get the values of all fields which are needed to start the decryption
            byte[] key = Library.HexStringToDecimalByteArray(HexDecryptionPasswortBox.Text);
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

        public async Task<byte[]> DecryptionTask(byte[] key, byte[] ciphertext, byte[] iv, byte[] aad, byte[] tag)
        {
            return await Task.Run(() => Library.Decrypt(key, ciphertext, iv, aad, tag));
        }

        #endregion
    }
}
