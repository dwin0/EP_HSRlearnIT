using System;
using System.Collections.Generic;
using System.IO;
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

        /*
        public DecryptionPage(string ciphertext, string iv, string aad, string tag)
        {
            InitializeComponent();
            HexCiphertextBox.Text = ciphertext;
            HexAadBox.Text = aad;
            HexIvBox.Text = iv;
            HexTagBox.Text = tag;
            Progress.SaveProgress("DecryptionPage_HexCiphertextBox", ciphertext);
            Progress.SaveProgress("DecryptionPage_HexAadBox", aad);
            Progress.SaveProgress("DecryptionPage_HexIvBox", iv);
            Progress.SaveProgress("DecryptionPage_HexTagBox", tag);
        } */

        #endregion

        #region Private Methods
        private void OnImportButtonClick(object sender, RoutedEventArgs e)
        {
            /*            string ciphertext = "";
                        string aad = "";
                        string iv = "";
                        string tag = "";*/

            /*            OpenMultiFileDialog openMultiFileDialog = new OpenMultiFileDialog()
                        {
                            Owner = Application.Current.MainWindow,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        };
                        openMultiFileDialog.Closed += new EventHandler(openMultiFileDialog_Closed);
                        openMultiFileDialog.Show();*/

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
                IEnumerable<string> fullFileContent = File.ReadLines(filePath);

                foreach (string line in fullFileContent)
                {
                    int index = line.IndexOf(Convert.ToChar('='));
                    //the first '=' is only a delimeter symbole and not part of parameter or value
                    string parameter = line.Substring(0, index-1);
                    string value = line.Substring(index + 1);
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
            }
            MessageBox.Show("Der Import wurde erfolgreich abgeschlossen.", "Import einer Parameterdatei", MessageBoxButton.OK, MessageBoxImage.Information);
            /*            Progress.SaveProgress("DecryptionPage_HexCiphertextBox", ciphertext);
                        Progress.SaveProgress("DecryptionPage_HexAadBox", aad);
                        Progress.SaveProgress("DecryptionPage_HexIvBox", iv);
                        Progress.SaveProgress("DecryptionPage_HexTagBox", tag);*/
        }

        private void openMultiFileDialog_Closed(object sender, EventArgs e)
        {
            //if (e == null) throw new ArgumentNullException(nameof(e));
            //if (e == null) return;
            var importFiles = Progress.GetProgress("OpenMultiFileDialog_Import") as Dictionary<string, string>;
            if (importFiles != null)
            {
                foreach (var fileName in importFiles)
                {
                    TextBox textBox = FindName("Hex" + fileName.Key + "Box") as TextBox;
                    
                    if (textBox != null)
                    {
                        Console.WriteLine(textBox.Name);
                        textBox.Text = File.ReadAllText(fileName.Value);
                        Progress.SaveProgress("DecryptionPage_Hex" + textBox.Name + "Box", textBox.Text);
                    }
                }
                MessageBox.Show("Import war erfolgreich!", "Fileimport", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
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

            UtfPlaintextBox.Text = Library.BytesToString(Library.Decrypt(key, ciphertext, iv, aad, tag));

            //case authentication only --> when successfull
            if (UtfPlaintextBox.Text == "")
            {
                MessageBox.Show("Der Text wurde erfolgreich authentifiziert.", "alleinstehenden Authentifizierung",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion
    }
}
