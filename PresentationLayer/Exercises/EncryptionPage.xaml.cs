using EP_HSRlearnIT.BusinessLayer.CryptoTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Castle.Core.Internal;

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
            string key = EncryptionPasswordBox.Text;
            byte[] keyArray = StringToBytes(key);
            int keySize = keyArray.Length;

            
            IEnumerable<byte> bigKey = keyArray;

            if (keySize < 32)
            {
                for (int i = 1; i <= 32 / keySize; i++)
                {
                    bigKey = bigKey.Concat(keyArray);
                }
            }

            bigKey = bigKey.Take(32);
            byte[] result = new byte[32];
            int counter = 0;

            bigKey.ForEach(i =>
            {
                byte b = i;
                result[counter] = b;
                counter++;
            });

            string bigKeyString = BytesToString(result);

            ChangeHexBox(bigKeyString.ToCharArray(), HexEncryptionPasswordBox);

            Tuple<byte[], byte[]> returnValue = _library.Encrypt(bigKeyString, PlainTextBox.Text, IvBox.Text, AadBox.Text);
            byte[] tag = returnValue.Item1;
            byte[] ciphertext = returnValue.Item2;
            CipherTextBox.Text = BytesToString(ciphertext);
            TagBox.Text = BytesToString(tag);
        }

        /* private void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
         {
             PlainTextBox.Text = _library.Decrypt(DecryptionPasswordBox.Text, StringToBytes(CipherTextBox.Text));
         }
         */

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Get control that raised this event
            var textBox = sender as TextBox;

            //Get control that will be updated
            if (textBox != null)
            {
                string nameHexField = "Hex" + textBox.Name;
                TextBox hexBox = (TextBox)FindName(nameHexField);
                char[] values = textBox.Text.ToCharArray();
                ChangeHexBox(values, hexBox);
            }
        }

        private void ChangeHexBox(char[] values, TextBox hexBox)
        {
            string hexOutput = "";

            //remove the event handler temporary, else a loop will occure
            if (hexBox != null)
            {
                hexBox.TextChanged -= HexTextBox_TextChanged;
                foreach (char letter in values)
                {
                    // Get the integral value of the character
                    //TODO führende 0 einbauen --> Test Case 14
                    int value = Convert.ToInt32(letter);

                    // Convert the decimal value to a hexadecimal value in string form
                    hexOutput += string.Format("{0:X}", value);
                }
                hexBox.Text = hexOutput;
                hexBox.TextChanged += HexTextBox_TextChanged;
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
                string nameHexBlock = nameHexTextBox.Substring(0, nameHexTextBox.Length-3) + "Block";

                //Get control that will be updated
                string nameTextField = nameHexTextBox.Substring(3, nameHexTextBox.Length-3);
                TextBox textBox = (TextBox)FindName(nameTextField);

                //remove the event handler temporary, else a loop will occure
                if (textBox != null)
                {
                    textBox.TextChanged -= TextBox_TextChanged;

                    string hexValue = hexTextBox.Text;

                    if (!IsHex(hexValue))
                    {
                        TextBlock textBlock = (TextBlock) FindName(nameHexBlock);
                        textBlock.Text = "Ungültige Eingabe!";
                        textBox.TextChanged += TextBox_TextChanged;
                        return;
                    }


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

        private bool IsHex(IEnumerable<char> chars)
        {
            bool isHex;
            foreach (var c in chars)
            {
                isHex = ((c >= '0' && c <= '9') ||
                         (c >= 'a' && c <= 'f') ||
                         (c >= 'A' && c <= 'F'));

                if (!isHex)
                    return false;
            }
            return true;
        }

        private string BytesToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length];

            StringBuilder sb = new StringBuilder(); //StringBuilder sinnvoll?
            int i = 0;
            foreach (byte b in bytes) //for schlaufe besser?
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
