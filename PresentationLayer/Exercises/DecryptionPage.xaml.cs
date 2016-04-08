using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EP_HSRlearnIT.Windows
{
    /// <summary>
    /// Interaktionslogik für DecryptionPage.xaml
    /// </summary>
    public partial class DecryptionPage : Page
    {

        #region Private Members

        private AesGcmCryptoLibrary _library;

        #endregion

        #region Constructors

        public DecryptionPage()
        {
            InitializeComponent();
            _library = new AesGcmCryptoLibrary();
        }

        #endregion

        #region Private Methods

        private void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
        {
            PlainTextBox.Text = _library.Decrypt(DecryptionPasswordBox.Text, StringToBytes(CipherTextBox.Text));
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
