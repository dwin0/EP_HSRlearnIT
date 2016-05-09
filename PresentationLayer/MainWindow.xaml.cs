using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using EP_HSRlearnIT.PresentationLayer.Tutorials;
using EP_HSRlearnIT.PresentationLayer.Games;
using System.Windows;
using System.Windows.Input;
using System;
using System.Windows.Media.Animation;
using EP_HSRlearnIT.PresentationLayer.Exercises;

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
            MainFrame.Navigate(new MainPage());
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
                    MainFrame.Navigate(new MainPage());
                    break;
                case "Übersicht AES-GCM":
                    MainFrame.Navigate(new AesGcmOverviewPage());
                    break;
                case "Schritt für Schritt - Anleitung":
                    MainFrame.Navigate(new StepByStepPage());
                    break;
                case "Ver- und Entschlüsselung":
                    MainFrame.Navigate(new PageWithTabs());
                    break;
                case "Drag & Drop - Spiel":
                    MainFrame.Navigate(new DragDropPage());
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
            MenuOpenEffect("StoryboardShowLeftMenu", MenuStackPanel);
        }

        private void MenuOpenEffect(string storyboard, FrameworkElement pnl)
        {
            Storyboard sb = Application.Current.FindResource(storyboard) as Storyboard;
            sb?.Begin(pnl);
        }

        //This method is only for manuel testing from FileManager but it will removed after implement export function
        private void OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileManager.UpdateContent(FileManager.SaveFile(@"C:\temp\HSRlearnIT", "AES-GCM.txt"), "The program is started!");
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