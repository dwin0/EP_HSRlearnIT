using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using EP_HSRlearnIT.PresentationLayer.Exercises;
using EP_HSRlearnIT.PresentationLayer.Tutorials;
using EP_HSRlearnIT.PresentationLayer.Games;
using System.Windows;
using System.Windows.Input;
using System;
using System.Windows.Media.Animation;

namespace EP_HSRlearnIT.PresentationLayer
{
    /// <summary>
    /// Window to present all pages and the menu
    /// </summary>
    public partial class MainWindow
    {

        #region Constructors

        /// <summary>
        /// Method to initialize the XAML and load the first page
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MainPage2());
            Application.Current.MainWindow = this;
        }
        #endregion

        private void OnMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuStackPanel.IsVisible)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }

        private void OnMenuItemClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as string;
            switch (item)
            {
                case "Startseite":
                    MainFrame.Navigate(new MainPage2());
                    break;
                case "AES-GCM Übersicht":
                    MainFrame.Navigate(new AesGcmOverviewPage());
                    break;
                case "Schritt für Schritt":
                    MainFrame.Navigate(new StepPage());
                    break;
                case "Ver- und Entschlüsselung":
                    MainFrame.Navigate(new EncryptionDecyrptionPage());
                    break;
                case "Drag- und Drop Spiel":
                    MainFrame.Navigate(new DragDrop3());
                    break;
            }
            CloseMenu();
        }

        private void CloseMenu()
        {
            MenuStackPanel.Visibility = Visibility.Collapsed;
        }

        private void OpenMenu()
        {
            MenuStackPanel.Visibility = Visibility.Visible;
            MenuOpenEffect("SbShowLeftMenu", MenuStackPanel);
        }

        private void MenuOpenEffect(string storyboard, FrameworkElement pnl)
        {
            Storyboard sb = Application.Current.FindResource(storyboard) as Storyboard;
            sb?.Begin(pnl);
        }

        private void OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileSaver.UpdateFileContent(FileSaver.SaveFile(@"C:\temp\HSRlearnIT", "AES-GCM.txt"), "The program is started!");
        }

        //This method is used only for a system test in order to control the correctness
        //of the Global Exception Handler and will be deleted for the production code
        private void OnExceptionClick(object sender, MouseButtonEventArgs e)
        {
            throw new Exception("Exception in Logfile vorhanden?");
        }

        private void CollapseMenu(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(MainGrid);

            //Check if click was outside the menu
            if (point.X > MenuStackPanel.ActualWidth + MenuStackPanel.Margin.Left + MenuStackPanel.Margin.Right)
            {
                CloseMenu();
            }
        }
    }
}