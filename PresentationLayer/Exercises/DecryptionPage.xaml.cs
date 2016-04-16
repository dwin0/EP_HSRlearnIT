using System.Windows;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    public partial class DecryptionPage
    {
        #region Constructors
        public DecryptionPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Methods
        private void OnImportButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnDecryptionButtonClick(object sender, RoutedEventArgs e)
        {
            //Library.Decrypt();
            //PlainTextBox.Text = _library.Decrypt(DecryptionPasswortBox.Text, CipherTextBox.Text, IvBox.Text, AadBox.Text, TagBox.Text);
        }

        private void OnEncryptionButtonClick(object sender, RoutedEventArgs e)
        {

        }
        #endregion

    }
}
