using EP_HSRlearnIT.BusinessLayer.CryptoTools;
using System;
using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    /// <summary>
    /// Interaction logic for EncryptionPage.xaml
    /// </summary>
    public partial class EncryptionPage
    {
        #region Private Members

        private AesGcmCryptoLibrary _library;

        #endregion


        #region Constructors

        public EncryptionPage()
        {
            InitializeComponent();
            _library = new AesGcmCryptoLibrary();
        }

        #endregion


        #region Private Methods

        private void OnEnryptionButtonClick(object sender, RoutedEventArgs e)
        {
            byte[] ciphertext = _library.Encrypt(EncryptionPasswordBox.Text, PlainTextBox.Text);
            CipherTextBox.Text = BytesToString(ciphertext);
        }

        /* private void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
         {
             PlainTextBox.Text = _library.Decrypt(DecryptionPasswordBox.Text, StringToBytes(CipherTextBox.Text));
         }
         */

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string hexOutput = "";

            //Get control that raised this event
            var textBox = sender as TextBox;

            //Get control that will be updated
            if (textBox != null)
            {
                string nameHexField = "Hex" + textBox.Name;
                TextBox hexBox = (TextBox)FindName(nameHexField);

                //remove the event handler temporary, else a loop will occure
                if (hexBox != null)
                {
                    hexBox.TextChanged -= HexTextBox_TextChanged;

                    char[] values = textBox.Text.ToCharArray();
                    foreach (char letter in values)
                    {
                        // Get the integral value of the character
                        int value = Convert.ToInt32(letter);

                        // Convert the decimal value to a hexadecimal value in string form
                        hexOutput += string.Format("{0:X}", value);
                    }
                    hexBox.Text = hexOutput;
                    hexBox.TextChanged += HexTextBox_TextChanged;
                }
            }
        }

        private void HexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textOutput = "";
            
            //Get control that raised this event
            var hexTextBox = sender as TextBox;
            if (hexTextBox != null)
            {
                string nameHexTextBox = hexTextBox.Name;

                //Get control that will be updated
                string nameTextField = nameHexTextBox.Substring(3, nameHexTextBox.Length-3);
                TextBox textBox = (TextBox)FindName(nameTextField);

                //remove the event handler temporary, else a loop will occure
                if (textBox != null)
                {
                    textBox.TextChanged -= TextBox_TextChanged;

                    string hexValue = hexTextBox.Text;
                    ArrayList list = new ArrayList();
                    while (hexValue.Length > 1)
                    {
                        string str = hexValue.Substring(0, 2);
                        list.Add(str);
                        hexValue = hexValue.Substring(2, hexValue.Length-2);
                    }

                    foreach (string hex in list)
                    {
                        // Convert the number expressed in base-16 to an integer.
                        int value = Convert.ToInt32(hex, 16);

                        // Get the character corresponding to the integral value.
                        textOutput += char.ConvertFromUtf32(value);
                    }
                    textBox.Text = textOutput;
                    textBox.TextChanged += TextBox_TextChanged;
                }
            }
        }

        private string BytesToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length];

            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (byte b in bytes)
            {
                chars[i] = Convert.ToChar(b);
                sb.Append(chars[i]);
                i++;
            }

            return sb.ToString();
        }

        private byte[] StringToBytes(string toConvert)
        {
            byte[] bytes = new byte[toConvert.Length];

            int i = 0;
            foreach (char c in toConvert)
            {
                bytes[i] = Convert.ToByte(c);
                i++;
            }

            return bytes;
        }

        #endregion
    }
}
