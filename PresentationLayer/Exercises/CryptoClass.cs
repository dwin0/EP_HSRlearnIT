using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Get control that raised this event
            var textBox = sender as TextBox;

            //Get control that will be updated
            if (textBox != null)
            {
                string nameHexField = "Hex" + textBox.Name;
                TextBox hexBox = (TextBox)FindName(nameHexField);
                char[] value = textBox.Text.ToCharArray();
                ChangeHexBox(value, hexBox);
            }
        }

        /// <summary>
        /// This Method is used to convert the TextBox input into Hex Values and update the correspondig HexTextBox.
        /// It can be called, if there is no way to register an event.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="hexBox"></param>
        public void ChangeHexBox(char[] values, TextBox hexBox)
        {
            string hexOutput = "";

            if (hexBox != null)
            {
                //remove the event handler temporary, else a loop will occure
                hexBox.TextChanged -= HexTextBox_TextChanged;
                foreach (char letter in values)
                {
                    int value = Convert.ToInt32(letter);

                    // Convert the decimal value to a hexadecimal value in string form
                    hexOutput += string.Format("{0:x2}", value);
                }
                hexBox.Text = hexOutput;
                hexBox.TextChanged += HexTextBox_TextChanged;
            }
        }

        /// <summary>
        /// This event is used to convert the HexTextBox input into chars and update the corresponding TextBox.
        /// If a non hex value is entered a warning will be shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textOutput = "";

            var hexTextBox = sender as TextBox;
            if (hexTextBox != null)
            {
                string nameHexTextBox = hexTextBox.Name;
                // get the name of the corresponding hexBlock. The Box suffix (3 chars) is removed and Block is appended. 
                string nameHexBlock = nameHexTextBox.Substring(0, nameHexTextBox.Length - 3) + "Block";

                //Get controls that will be updated
                string nameTextBox = nameHexTextBox.Substring(3, nameHexTextBox.Length - 3);
                TextBox textBox = (TextBox)FindName(nameTextBox);
                TextBlock textBlock = (TextBlock)FindName(nameHexBlock);
                if (textBlock == null) { return; }

                //check if the text was set to " Ungültige Eingabe!" in an earlier call of this method and reverse it 
                string textBlockName = textBlock.Text;
                if (textBlockName.Contains(" Ungültige Eingabe!"))
                {
                    //19 is the length of suffix text " Ungültige Eingabe!"
                    textBlockName = textBlockName.Substring(0, textBlockName.Length - 19);
                    textBlock.Text = textBlockName;
                    textBlock.Foreground = Brushes.Black;
                }

                if (textBox == null) { return; }

                //remove the event handler temporary, else a loop will occure
                textBox.TextChanged -= TextBox_TextChanged;

                string hexValue = hexTextBox.Text;

                //if the input is invalid the following will be executed
                if (!IsHex(hexValue))
                {
                    textBlock.Text = textBlockName + " Ungültige Eingabe!";
                    textBlock.Foreground = Brushes.Red;
                    textBox.TextChanged += TextBox_TextChanged;
                    return;
                }

                
                ArrayList list = new ArrayList();
                while (hexValue.Length > 1)
                {
                    list.Add(hexValue.Substring(0, 2));
                    hexValue = hexValue.Substring(2, hexValue.Length - 2);
                }

                foreach (string hex in list)
                {
                    // Convert the number expressed in base-16 to an integer.
                    int value = Convert.ToInt32(hex, 16);

                    textOutput += char.ConvertFromUtf32(value);
                }

                textBox.Text = textOutput;
                textBox.TextChanged += TextBox_TextChanged;
            }
        }

        /// <summary>
        /// This event is used to save the Progress of a HexTextBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HexTextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            TextBox source = e.Source as TextBox;
            if (source != null)
            {
                string elementName = source.Name;
                SaveProgressHelper(source, elementName);
            }
        }

        /// <summary>
        /// This event is used to save the Progress of a TextBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            TextBox source = e.Source as TextBox;
            if (source != null)
            {
                //get the name of the corresponding Hex field --> Progess will only be saved in Hex values!
                string elementName = "Hex" + source.Name;
                TextBox hexBox = (TextBox)FindName(elementName);
                SaveProgressHelper(hexBox, elementName);
            }

        }

        #endregion

        #region Private Methods
        private bool IsHex(IEnumerable<char> chars)
        {
            return chars.Select(c => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')).All(isHex => isHex);
        }

        private void SaveProgressHelper(TextBox source, string elementName)
        {
            var parent = VisualTreeHelper.GetParent(source);
            while (!(parent is Page))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            string pageName = (parent as Page).Title;

            string key = pageName + "_" + elementName;
            Progress.SaveProgress(key, source.Text);
        }

        #endregion
    }
}