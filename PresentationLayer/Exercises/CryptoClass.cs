using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EP_HSRlearnIT.BusinessLayer.CryptoTools;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.PresentationLayer.Exercises

{
    public class CryptoClass : Page
    {

        #region Public Members
        public AesGcmCryptoLibrary Library;
        #endregion

        #region Constructors
        public CryptoClass()
        {
            Library = new AesGcmCryptoLibrary();
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
                char[] value = textBox.Text.ToCharArray();
                ChangeHexBox(value, hexBox);
            }
        }

        /// <summary>
        /// This Method is used to convert the TextBox input into Hex Values and update the correspondig HexTextBox.
        /// It can be called, if there is no way to register an event.
        /// </summary>
        /// <param name="values">Values which are converted into hex.</param>
        /// <param name="hexBox">The hexBox which will be updated with the converted values.</param>
        public void ChangeHexBox(char[] values, TextBox hexBox)
        {
            StringBuilder sb = new StringBuilder();

            if (hexBox != null)
            {
                //remove the event handler temporary, else a loop will occure
                hexBox.TextChanged -= HexTextBox_TextChanged;
                foreach (char letter in values)
                {
                    int value = Convert.ToInt32(letter);

                    // Convert the decimal value to a hexadecimal value in string form
                    sb.AppendFormat("{0:x2}", value);
                }
                hexBox.Text = sb.ToString();
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

        #endregion

        #region Private Methods
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
            var parent = VisualTreeHelper.GetParent(element);
            while (!(parent is Page))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            string pageName = (parent as Page).Title;
            string key = pageName + "_" + element.Name;
            Progress.SaveProgress(key, element.Text);
        }

        #endregion
    }
}