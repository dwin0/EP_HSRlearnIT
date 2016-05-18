using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EP_HSRlearnIT.BusinessLayer.CryptoTools;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using Microsoft.Win32;
using System.IO;

namespace EP_HSRlearnIT.PresentationLayer.Exercises

{
    public abstract class EncryptionDecryptionComponents : Page
    {

        #region Public Members
        public AesGcmAdapter Library;

        #endregion

        #region Constructors
        public EncryptionDecryptionComponents()
        {
            Library = new AesGcmAdapter();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This event is used to convert the TextBox input into Hex Values and update the correspondig HexTextBox.
        /// </summary>
        /// <param name="sender">Contains the control which raised the event.</param>
        /// <param name="e"></param>
        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Get control that raised this event
            var textBox = sender as TextBox;

            //Get control that will be updated
            if (textBox != null)
            {
                
                string hexFieldName = "Hex" + textBox.Name.Substring(3);
                TextBox hexBox = (TextBox)FindName(hexFieldName);
                ChangeHexBox(textBox.Text, hexBox);
            }
        }

        /// <summary>
        /// This Method is used to convert the TextBox input into Hex Values and update the correspondig HexTextBox.
        /// It can be called, if there is no way to register an event.
        /// </summary>
        /// <param name="values">Values which are converted into hex.</param>
        /// <param name="hexBox">The hexBox which will be updated with the converted values.</param>
        public void ChangeHexBox(string values, TextBox hexBox)
        {
            if (hexBox != null)
            {
                //remove the event handler temporary, else a loop will occure
                hexBox.TextChanged -= HexTextBox_TextChanged;
                hexBox.Text = Library.ConvertToHexString(values);
                hexBox.TextChanged += HexTextBox_TextChanged;
            }
        }

        /// <summary>
        /// This event is used to convert the HexTextBox input into chars and update the corresponding TextBox.
        /// If a non hex value is entered a warning will be shown.
        /// </summary>
        /// <param name="sender">Contains the control which raised the event.</param>
        /// <param name="e"></param>
        public void HexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var hexTextBox = sender as TextBox;
            if (hexTextBox != null)
            {
                //Get the corresponding hexWarningBlock (Field above the HexBox).
                //The Box suffix (3 chars) is removed and Block is appended.
                string hexTextBoxName = hexTextBox.Name;
                string hexWarningBlockName = hexTextBoxName.Substring(0, hexTextBoxName.Length - 3) + "Block";
                TextBlock hexWarningBlock = (TextBlock)FindName(hexWarningBlockName);

                //Get textBox that will be updated. Ex: HexIvBox -> IvBox
                string textBoxName = "Utf" + hexTextBoxName.Substring(3, hexTextBoxName.Length - 3);
                TextBox textBox = (TextBox)FindName(textBoxName);
                
                if (hexWarningBlock == null || textBox == null) { return; }

                CheckIfWarningIsAlreadySet(hexWarningBlock);

                //remove the event handler temporary, else a loop will occure
                textBox.TextChanged -= TextBox_TextChanged;

                string hexValue = hexTextBox.Text;

                if (FoundNonHexValues(hexValue, hexWarningBlock))
                {
                    textBox.TextChanged += TextBox_TextChanged;
                    return;
                }

                if (hexValue.Length % 2 == 0)
                {
                    textBox.Text = Library.BytesToString(Library.HexStringToDecimalByteArray(hexValue));
                    textBox.TextChanged += TextBox_TextChanged;
                }
            }
        }

        /// <summary>
        /// This event is used to save the Progress of a HexTextBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains the control which raised the event.</param>
        public void HexTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox hexTextBox = e.Source as TextBox;
            if (hexTextBox != null)
            {
                SaveProgressHelper(hexTextBox);
            }
        }

        /// <summary>
        /// This event is used to save the Progress of a TextBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains the control which raised the event.</param>
        public void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = e.Source as TextBox;
            if (textBox != null)
            {
                //get the name of the corresponding Hex field --> Progess will only be saved in Hex values!
                TextBox hexBox = (TextBox)FindName("Hex" + textBox.Name.Substring(3));
                SaveProgressHelper(hexBox);
            }
        }

        public void OnResetButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var element in DependencyObjectExtension.GetAllChildren<TextBox>(this))
            {
                if (element.Text != "")
                {
                    element.Text = "";
                }
            }
        }

        public void OnExportButtonClick(object sender, RoutedEventArgs e)
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
                        line.AppendLine(element.Name.Substring(3, element.Name.Length - 6) + "=0x" + element.Text);
                    }
                }
                FileManager.UpdateContent(fullFilePath, line.ToString());
                MessageBox.Show("Der Export war erfolgreich.", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void OnImportButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Importieren der Entschlüsselungsparameter"
            };

            if (openFileDialog.ShowDialog() == true && openFileDialog.SafeFileName != "")
            {
                    string filePath = openFileDialog.FileName;
                    IEnumerable<string> allLines = FileManager.ReadAllLines(filePath);

                    foreach (string line in allLines)
                    {
                        int index = line.IndexOf(Convert.ToChar('='));
                        //the first '=' is only a delimeter symbole and not part of parameter or value
                        string parameter = line.Substring(0, index - 1);
                        string value = line.Substring(index + 1);
                        FillingField(parameter, value);
                    }
                    MessageBox.Show("Der Import wurde erfolgreich abgeschlossen.", "Import einer Parameterdatei", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region Private Method
        private void CheckIfWarningIsAlreadySet(TextBlock hexWarningBlock)
        {
            //check if the text was set to " Ungültige Eingabe!" in an earlier call of this method and reverse it 
            string textBlockName = hexWarningBlock.Text;
            if (textBlockName.Contains(" Ungültige Eingabe!"))
            {
                //19 is the length of suffix text " Ungültige Eingabe!"
                textBlockName = textBlockName.Substring(0, textBlockName.Length - 19);
                hexWarningBlock.Text = textBlockName;
                hexWarningBlock.Foreground = Brushes.Black;
            }
        }

        private bool FoundNonHexValues(string hexValue, TextBlock hexWarningBlock)
        {
            if (IsHex(hexValue)) { return false; }

            hexWarningBlock.Text += " Ungültige Eingabe!";
            hexWarningBlock.Foreground = Brushes.Red;
            return true;
        }

        private bool IsHex(IEnumerable<char> chars)
        {
            return chars.Select(c => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')).All(isHex => isHex);
        }

        private void SaveProgressHelper(TextBox element)
        {
            var parentPage = DependencyObjectExtension.GetParentPage(element) as Page;
            if (parentPage != null)
            {
                string pageName = parentPage.Title;
                string key = pageName + "_" + element.Name;
                Progress.SaveProgress(key, element.Text);
            }
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

        #endregion
    }
}