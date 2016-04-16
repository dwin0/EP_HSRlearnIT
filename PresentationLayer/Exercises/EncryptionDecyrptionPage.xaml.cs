using System.Windows;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    public partial class EncryptionDecyrptionPage
    {
        #region Public Methods
        public EncryptionDecyrptionPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Methods
        private void Encryption_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new EncryptionPage());
        }

        private void Decryption_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new DecryptionPage());
        }
        #endregion
    }
}
