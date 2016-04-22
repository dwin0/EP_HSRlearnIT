using System.Windows;
using EP_HSRlearnIT.Games;
using EP_HSRlearnIT.PresentationLayer.Tutorials;
using EP_HSRlearnIT.PresentationLayer.Exercises;

namespace EP_HSRlearnIT.PresentationLayer

{

    public partial class MainPage
    {
        #region Constructors

        public MainPage()
        {
            InitializeComponent();
            MenuSetup();
        }
        #endregion

        #region Private Methods
        private void MenuSetup()
        {
            var menuStrings = Application.Current.FindResource("MenuStrings") as StringCollection;
            if (menuStrings == null) return;
            foreach (var str in menuStrings)
            {
                //TODO
            }
        }

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
            NavigationService?.Navigate(new DragDrop3());
        }
        #endregion
    }
}