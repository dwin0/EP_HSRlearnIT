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
        private AesGcmCryptoLibrary library;
        public EncryptionPage()
        {
            InitializeComponent();
            library = new AesGcmCryptoLibrary();
        }

        private void OnEnryptionButtonClick(object sender, RoutedEventArgs e)
        {
            CipherTextBox.Text = library.Encrypt(EncryptionPasswordBox.Text, PlainTextBox.Text);
        }

        private void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
        {
            PlainTextBox.Text = library.Decrypt(DecryptionPasswordBox.Text, CipherTextBox.Text);
        }
    }
}
