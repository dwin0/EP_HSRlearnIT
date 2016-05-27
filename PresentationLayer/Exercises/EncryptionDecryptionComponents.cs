using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EP_HSRlearnIT.BusinessLayer.AesGcmLibrary;
using EP_HSRlearnIT.BusinessLayer.Persistence;
using Microsoft.Win32;
using System.IO;
using EP_HSRlearnIT.BusinessLayer.Extensions;

namespace EP_HSRlearnIT.PresentationLayer.Exercises

{
    /// <summary>
    /// In this class are all components which can be called on both Encryption and Decryption in the same way.
    /// </summary>
    public abstract class EncryptionDecryptionComponents : Page
    {

        #region Public Members
        public AesGcmAdapter Library;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a AesGcmAdapter, which can be used in this class.
        /// </summary>
        protected EncryptionDecryptionComponents()
        {
            Library = new AesGcmAdapter();
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// This event is used to convert the TextBox input into Hex Values and update the correspondig HexTextBox.
        /// The input will be validate, if it is a key or an iv. This validation checks if the key or iv is big enough
        /// if it is not big enough a warning is shown. 
        /// </summary>
        /// <param name="sender">Contains the control which raised the event.</param>
        /// <param name="e"></param>
        public void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //Get control that raised this event
            var textBox = sender as TextBox;

            //Get control that will be updated
            if (textBox != null)
            {
                string hexFieldName = "Hex" + textBox.Name.Substring(3);
                TextBox hexBox = (TextBox)FindName(hexFieldName);
                ChangeHexBox(textBox.Text, hexBox);
                SetShortInputWarning(hexBox);
            }
        }

        /// <summary>
        /// This Method is used to convert the TextBox input into Hex Values and update the correspondig HexTextBox.
        /// It can be called, if there is no way to register an event.
        /// </summary>
        /// <param name="values">String which is converted into hex.</param>
        /// <param name="hexBox">The hexBox which will be updated with the converted values.</param>
        public void ChangeHexBox(string values, TextBox hexBox)
        {
            if (hexBox == null) { return;}

            //remove the event handler temporary, else a loop will occure
            hexBox.TextChanged -= HexTextBox_OnTextChanged;
            hexBox.Text = Library.ConvertToHexString(values);
            hexBox.TextChanged += HexTextBox_OnTextChanged;
        }

        /// <summary>
        /// This event is used to convert the HexTextBox input into chars and update the corresponding TextBox.
        /// If a non hex value is entered a warning will be shown.
        /// The input will be validate, if it is a key or an iv. This validation checks if the key or iv is big enough
        /// if it is not big enough a warning is shown. 
        /// </summary>
        /// <param name="sender">Contains the control which raised the event.</param>
        /// <param name="e"></param>
        public void HexTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var hexTextBox = sender as TextBox;
            if (hexTextBox != null)
            {
                SetShortInputWarning(hexTextBox);

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
                textBox.TextChanged -= TextBox_OnTextChanged;

                string hexValue = hexTextBox.Text;

                if (FoundNonHexValues(hexValue, hexWarningBlock))
                {
                    textBox.TextChanged += TextBox_OnTextChanged;
                    return;
                }

                if (hexValue.Length % 2 == 0)
                {
                    textBox.Text = Library.BytesToString(Library.HexStringToDecimalByteArray(hexValue));
                    textBox.TextChanged += TextBox_OnTextChanged;
                }
            }
        }

        /// <summary>
        /// This event is used to save the Progress of a HexTextBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains the control which raised the event.</param>
        public void HexTextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
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
        public void TextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox textBox = e.Source as TextBox;
            if (textBox != null)
            {
                //get the name of the corresponding Hex field --> Progess will only be saved in Hex values!
                TextBox hexBox = (TextBox)FindName("Hex" + textBox.Name.Substring(3));
                SaveProgressHelper(hexBox);
            }
        }

        /// <summary>
        /// A warning is shown for the Key and the Iv, if these parameter are to small.
        /// </summary>
        /// <param name="hexTextBox">The HexBox which should be checked</param>
        public void SetShortInputWarning(TextBox hexTextBox)
        {
            if (hexTextBox.Name == "HexPasswordBox")
            {
                SetPasswordWarning(hexTextBox);
            }
            else if (hexTextBox.Name == "HexIvBox")
            {
                SetIvWarning(hexTextBox);
            }
        }

        /// <summary>
        /// This event is used to reset all TextBoxes of the Encryption or the Decryption Page.
        /// </summary>
        /// <param name="sender">is not used</param>
        /// <param name="e">is not used</param>
        public void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var element in DependencyObjectExtension.GetAllChildren<TextBox>(this))
            {
                if (element.Text != "")
                {
                    element.Text = "";
                }
                SaveProgressHelper(element);
            }
        }

        /// <summary>
        /// This event starts the windows explorer to save a hex file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ExportButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Text File (*.txt)|*.txt",
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

                if (!FileManager.IsExisting(fullFilePath))
                {
                    FileManager.SaveFile(fullFilePath);
                }
                string line = PickoutField();

                FileManager.OverwriteContent(fullFilePath, line);
                string message = "Der Export war erfolgreich.";
                string title = "Export";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// This event starts the windows explorer to import a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ImportButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Text File (*.txt)|*.txt",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Importieren der Entschlüsselungsparameter"
            };

            if (openFileDialog.ShowDialog() == true && openFileDialog.SafeFileName != "")
            {
                string filePath = openFileDialog.FileName;
                IEnumerable<string> allLines = FileManager.ReadAllLines(filePath);
                bool noHexData = false;

                foreach (string line in allLines)
                {
                    int index = line.IndexOf(Convert.ToChar('='));
                    //the first '=' is only a delimeter symbole and not part of parameter or value
                    if (index > 0)
                    {
                        string parameter = line.Substring(0, index - 1);
                        string value = line.Substring(index + 1);
                        if (value.Substring(0, 2).Equals("0x"))
                        {
                            parameter = "Hex" + parameter;
                            value = value.Substring(2);
                            FillingField(parameter, value);
                            noHexData = false;
                        }
                        else
                        {
                            noHexData = true;
                        }
                    }
                    else
                    {
                        noHexData = true;
                        break;
                    }
                }
                if (noHexData)
                {
                        FillingField(filePath);
                }
                string message = "Der Import wurde erfolgreich abgeschlossen.";
                string title = "Import";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Method to show a messageBox if a char is represented as more than one byte
        /// </summary>
        public void ShowTooBigCharError()
        {
            const string message = "Es wurde ein Zeichen, welches mit mehr als einem Byte repräsentiert wird, eingegeben. " +
                                    "Bitte überprüfe und korrigiere die Eingabe.";
            const string title = "Achtung";
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        /// <summary>
        /// Method to check if an odd number of Hex-Values exists
        /// </summary>
        public void CheckIfHexIsOdd(Page page)
        {
            string triggeringField = "(Feld konnte leider nicht bestimmt werden)";
            foreach (var elem in DependencyObjectExtension.GetAllChildren<TextBox>(page))
            {
                if (elem.Name.Contains("Hex") && (elem.Text.Length % 2 != 0))
                {
                    triggeringField = elem.Name.Substring(3, elem.Name.Length - 6);
                }
            }

            string message = "In dem Feld " + triggeringField + " wurde ein ungerader Hex-Wert eingegeben. " +
                             "Bitte überprüfe und korrigiere die Eingabe.";
            const string title = "Achtung";
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

        private void SetPasswordWarning(TextBox textBox)
        {
            foreach (var element in DependencyObjectExtension.GetAllChildren<TextBlock>(this))
            {
                if (element.Name == "PasswordBlock")
                {
                    if (textBox.Text.Length % 32 != 0)
                    {
                        if (textBox.Text.Length < 16 && textBox.Text.Length != 0)
                        {
                            if (!element.Text.Contains("klein"))
                            {
                                if (element.Text.Contains("ok"))
                                {
                                    element.Text = element.Text.Substring(0, 4);
                                }
                                element.Text = element.Text + " kleiner 8 Byte ist nicht erlaubt.";
                                element.Foreground = Brushes.Red;
                            }
                        } else { 
                            if (!element.Text.Contains("ok"))
                            {
                                if (element.Text.Contains("klein"))
                                {
                                    element.Text = element.Text.Substring(0, 4);
                                }
                                element.Text = element.Text + " ok, Standard fordert allerdings 32 Byte.";
                                element.Foreground = Brushes.DarkOrange;
                            }
                        }
                    }
                    else
                    {
                        element.Text = element.Text.Substring(0, 4);
                        element.Foreground = Brushes.Black;
                    }
                }
            }
        }

        private void SetIvWarning(TextBox textBox)
        {
            foreach (var element in DependencyObjectExtension.GetAllChildren<TextBlock>(this))
            {
                if (element.Name == "IvBlock")
                {
                    if (textBox.Text.Length % 24 != 0 && textBox.Text.Length != 0)
                    {
                        if (!element.Text.Contains("muss"))
                        {
                            element.Text = element.Text + " falls selbstgewählt, muss IV 12 Byte sein.";
                            element.Foreground = Brushes.Red;
                        }
                    }
                    else
                    {
                        element.Text = element.Text.Substring(0, 14);
                        element.Foreground = Brushes.Black;
                    }
                }
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
        /// Method fillup all HEX-fields with name of parameter and value into a string.
        /// </summary>
        /// <returns>A string with all allowed export parameters and their value.</returns>
        private string PickoutField()
        {
            StringBuilder line = new StringBuilder();
            foreach (TextBox element in DependencyObjectExtension.GetAllChildren<TextBox>(this))
            {
                if (Title.Contains("DecryptionPage"))
                {
                    //Export-Filter about all fields for DecryptionPage
                    if (element.Name.Contains("Hex") && !(element.Name.Contains("Ciphertext") || element.Name.Contains("Tag")))
                    {
                        //cut Hex and Box, for Example: HexIvBox -> Iv
                        line.AppendLine(element.Name.Substring(3, element.Name.Length - 6) + "=0x" + element.Text);
                    }
                }
                //Export-Filter about all fields for EncryptionPage
                else if (element.Name.Contains("Hex") && !element.Name.Contains("Plaintext"))
                {
                    line.AppendLine(element.Name.Substring(3, element.Name.Length - 6) + "=0x" + element.Text);
                }
            }
            return line.ToString();
        }

        /// <summary>
        /// Method filling all fields from the importfile excluding the following parameter in the filter.
        /// </summary>
        /// <param name="parameter">This is the identifier of the field from the file.</param>
        /// <param name="value">This is the value from the parameter in this function.</param>
        private void FillingField(string parameter, string value)
        {
            if (value.Length > 0)
            {
                foreach (var element in DependencyObjectExtension.GetAllChildren<TextBox>(this))
                {
                    if (Title.Equals("EncryptionPage"))
                    {
                        //Import-Filter for EncryptionPage
                        if (element.Name.Contains(parameter) && !(element.Name.Contains("Ciphertext") || element.Name.Contains("Tag")))
                        {
                            PostContent(element, value);
                            break;
                        }
                    }
                    //Import-Filter for DecryptionPage
                    else if (element.Name.Contains(parameter) && !element.Name.Contains("Plaintext"))
                    {
                        PostContent(element, value);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Method is filling the whole file into the Plaintext or Ciphertext regulated by the activ page.
        /// </summary>
        /// <param name="filePath"></param>
        private void FillingField(string filePath)
        {
            foreach (var textBox in DependencyObjectExtension.GetAllChildren<TextBox>(this))
            {
                if (textBox.Name.Contains("UtfPlaintext") || textBox.Name.Contains("UtfCiphertext"))
                {
                    PostContent(textBox, FileManager.ReadFullContent(filePath));
                    return;
                }
            }
        }

        /// <summary>
        /// Method post the content of a parameter from a other function to a defined element.
        /// </summary>
        /// <param name="element">The target TextBox in which is written.</param>
        /// <param name="value">Content which is written.</param>
        private void PostContent(TextBox element, string value)
        {
            TextBox fieldOnForm = FindName(element.Name) as TextBox;
            if (fieldOnForm != null)
            {
                fieldOnForm.Text = value;
            }
        }

        #endregion
    }
}