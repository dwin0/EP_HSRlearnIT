using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using Castle.Core.Internal;
using EP_HSRlearnIT.BusinessLayer.CryptoTools;

namespace EP_HSRlearnIT.PresentationLayer.Exercises

{
    public class CryptoClass : Page
    {

        #region Private Members
        public AesGcmCryptoLibrary Library;
        #endregion

        #region Constructors
        public CryptoClass()
        {
            Library = new AesGcmCryptoLibrary();
        }
        #endregion

        #region Public Methods
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

        public void HexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textOutput = "";

            var hexTextBox = sender as TextBox;
            if (hexTextBox != null)
            {
                string nameHexTextBox = hexTextBox.Name;
                //get the name of the corresponding hexBlock. This name will be used, if an invalid char is entered in the HexBox. An error will be shown
                string nameHexBlock = nameHexTextBox.Substring(0, nameHexTextBox.Length - 3) + "Block";

                //Get control that will be updated
                string nameTextField = nameHexTextBox.Substring(3, nameHexTextBox.Length - 3);
                TextBox textBox = (TextBox)FindName(nameTextField);
                TextBlock textBlock = (TextBlock)FindName(nameHexBlock);
                if (textBlock == null) { return; }

                string textBlockAnzeige = textBlock.Text;

                if (textBlockAnzeige.Contains(" Ungültige Eingabe!"))
                {
                    //19 is the length of suffix text
                    textBlockAnzeige = textBlockAnzeige.Substring(0, textBlockAnzeige.Length - 19);
                    textBlock.Text = textBlockAnzeige;
                    textBlock.Foreground = Brushes.Black;
                }

                if (textBox == null) { return; }

                //remove the event handler temporary, else a loop will occure
                textBox.TextChanged -= TextBox_TextChanged;

                string hexValue = hexTextBox.Text;

                if (!IsHex(hexValue))
                {
                    textBlock.Text = textBlockAnzeige + " Ungültige Eingabe!";
                    textBlock.Foreground = Brushes.Red;
                    textBox.TextChanged += TextBox_TextChanged;
                    return;
                }

                ArrayList list = new ArrayList();
                while (hexValue.Length > 1)
                {
                    //string str = hexValue.Substring(0, 2);
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

        public void GenerateHexKey(string key, TextBox hexPasswordBox)
        {
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

            ChangeHexBox(bigKeyString.ToCharArray(), hexPasswordBox);
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public string BytesToString(byte[] bytes)
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

        public byte[] StringToBytes(string toConvert)
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

        #region Private Methods
        private bool IsHex(IEnumerable<char> chars)
        {
            string check = Convert.ToString(chars);
            return System.Text.RegularExpressions.Regex.IsMatch(check, @"\A\b[0-9a-fA-F]+\b\Z");
/*            foreach (var c in chars)
            {
                bool isHex = ((c >= '0' && c <= '9') ||
                              (c >= 'a' && c <= 'f') ||
                              (c >= 'A' && c <= 'F'));

                if (!isHex)
                {
                    return false;
                }
            }
            return true;*/
        }
        #endregion
    }
}