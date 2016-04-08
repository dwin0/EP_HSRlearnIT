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
