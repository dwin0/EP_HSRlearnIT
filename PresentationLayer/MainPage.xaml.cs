using EP_HSRlearnIT.PresentationLayer.Exercises;
using EP_HSRlearnIT.PresentationLayer.Games;
using EP_HSRlearnIT.PresentationLayer.Tutorials;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EP_HSRlearnIT.PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        MainWindow _main;

        #region Constructors

        public MainPage(MainWindow main)
        {
            InitializeComponent();
            _main = main;
        }
        #endregion


        #region Private Methods

        private void OverviewScreen_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AesGcmOverviewPage());
        }

        private void StepByStep_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StepPage(_main));
        }

        private void EncryptionDecryption_Click(object sender, RoutedEventArgs e)
        {
          
     
           NavigationService.Navigate(new EncryptionDecyrptionPage());
        }

        private void DragAndDrop_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DragDropPage1());
        }

        #endregion
    }
}
