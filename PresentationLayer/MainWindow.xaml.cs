using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using EP_HSRlearnIT.PresentationLayer.Exercises;
using EP_HSRlearnIT.PresentationLayer.Games;
using EP_HSRlearnIT.PresentationLayer.Tutorials;
using System.Windows;
using System.Windows.Input;
using System;

namespace EP_HSRlearnIT.PresentationLayer
{
    public partial class MainWindow
    {

        #region Constructors

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


        private void ListBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as string;
            switch (item)
            {
                case "AES-GCM Übersicht":
                    MainFrame.Navigate(new AesGcmOverviewPage());
                    break;
                case "Schritt für Schritt":
                    MainFrame.Navigate(new StepPage());
                    break;
                case "Ver- und Entschlüsselung":
                    MainFrame.Navigate(new EncryptionPage());
                    break;
                case "Drag- und Drop Spiel":
                    MainFrame.Navigate(new DragDropPage1());
                    break;
            }
            CloseMenu();
        }

        private void OnBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.NavigationService.CanGoBack)
            {
                MainFrame.NavigationService.GoBack();
            }
        }

        private void OnForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.NavigationService.CanGoForward)
            {
                MainFrame.NavigationService.GoForward();
            }
        }

        private void CloseMenu()
        {
            MenuStackPanel.Visibility = Visibility.Collapsed;
            MenuButton.HorizontalAlignment = HorizontalAlignment.Left;
            MenuButton.Content = "Menu";
            var margin = MenuButton.Margin;
            margin.Top = 0;
            MenuButton.Margin = margin;
        }

        private void OpenMenu()
        {
            MenuStackPanel.Visibility = Visibility.Visible;
            MenuButton.HorizontalAlignment = HorizontalAlignment.Right;
            MenuButton.Content = "<<";
            var margin = MenuButton.Margin;
            margin.Top = -10;
            MenuButton.Margin = margin;
        }


        private void OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileSaver.UpdateFileContent(FileSaver.SaveFile(@"C:\temp\HSRlearnIT", "AES-GCM.txt"), "The program is started!");
        }

        private void OnExceptionClick(object sender, MouseButtonEventArgs e)
        {
            throw new Exception("Exception in Logfile vorhanden?");
        }

        //Collapse Menu Click was outside
       private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(MainGrid);

            if (point.X > MenuStackPanel.ActualWidth)
            {
                CloseMenu();
            }
        }
    }
}