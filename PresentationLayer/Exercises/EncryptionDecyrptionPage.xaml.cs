using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    /// <summary>
    /// Interaktionslogik für Page1.xaml
    /// </summary>
    public partial class EncryptionDecyrptionPage : Page
    {
        public EncryptionDecyrptionPage()
        {
            InitializeComponent();
        }

        private void Encryption_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EncryptionPage());

        }

        private void Decryption_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DecryptionPage());

        }
    }
}
