using EP_HSRlearnIT.PresentationLayer.Exercises;
using EP_HSRlearnIT.PresentationLayer.Games;
using EP_HSRlearnIT.PresentationLayer.Tutorials;
using System.Windows;
using System.Windows.Controls;

namespace EP_HSRlearnIT.PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {

        #region Constructors

        public MainPage()
        {
            InitializeComponent();
        }
        #endregion


        #region Private Methods

        private void OverviewScreen_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AesGcmOverviewPage());
        }

        private void StepByStep_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new StepPage());
        }

        private void EncryptionDecryption_Click(object sender, RoutedEventArgs e)
        {
           NavigationService?.Navigate(new EncryptionDecyrptionPage());
        }

        private void DragAndDrop_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new DragDropPage1());
        }

        #endregion
    }
}
