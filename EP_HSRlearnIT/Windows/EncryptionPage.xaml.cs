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
    /// Interaction logic for EncryptionPage.xaml
    /// </summary>
    public partial class EncryptionPage : Page
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
            CipherTextBox.Text = _library.Encrypt(EncryptionPasswordBox.Text, PlainTextBox.Text);
        }

        private void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
        {
            PlainTextBox.Text = _library.Decrypt(DecryptionPasswordBox.Text, CipherTextBox.Text);
        }
        #endregion
    }
}
