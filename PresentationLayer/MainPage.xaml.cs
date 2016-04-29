using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EP_HSRlearnIT.PresentationLayer.Games;
using EP_HSRlearnIT.PresentationLayer.Exercises;
using EP_HSRlearnIT.PresentationLayer.Tutorials;

namespace EP_HSRlearnIT.PresentationLayer
{
    /// <summary>
    /// Page containing the program-navigation
    /// </summary>
    public partial class MainPage
    {
        #region Private Members

        private readonly SolidColorBrush _backgroundBrush = Application.Current.FindResource("TileBackgroundBrush") as SolidColorBrush;
        private readonly SolidColorBrush _borderBrush = Application.Current.FindResource("TileBorderBrush") as SolidColorBrush;
        #endregion

        #region Constructors

        /// <summary>
        /// Method to initialize the XAML
        /// </summary>
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
            NavigationService?.Navigate(new StepByStepPage());
        }

        private void EncryptionDecryption_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new EncryptionDecyrptionPage());
        }

        private void DragAndDrop_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new DragDropPage());
        }
        #endregion

        private void MenuTile_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Border parent = (sender as Grid)?.Parent as Border;
            if (parent != null)
            {
                parent.BorderBrush = _borderBrush;
            }
        }

        private void MenuTile_OnMouseLeave(object sender, MouseEventArgs e)
        {
            Border parent = (sender as Grid)?.Parent as Border;
            if (parent != null)
            {
                parent.BorderBrush = _backgroundBrush;
            }
        }
    }
}
