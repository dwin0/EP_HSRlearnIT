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
                case "Startseite":
                    MainFrame.Navigate(new MainPage());
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
                    MainFrame.Navigate(new DragDropPage());
                    break;
            }
          CloseMenu(); 
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

            ShowHideMenu("SbShowLeftMenu", MenuStackPanel);
        }

        private void ShowHideMenu(string storyboard, FrameworkElement pnl)
        {
            Storyboard sb = Application.Current.FindResource(storyboard) as Storyboard;
            sb?.Begin(pnl);
        }

        private void OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileSaver.UpdateFileContent(FileSaver.SaveFile(@"C:\temp\HSRlearnIT", "AES-GCM.txt"), "The program is started!");
        }

        //This method is used only for a system test in order to contorl the correctness of the Global Exception Handler and will be deleted for the production code 
        private void OnExceptionClick(object sender, MouseButtonEventArgs e)
        {
            throw new Exception("Exception in Logfile vorhanden?");
        }

        //Collapse Menu Click was outside
       private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(MainGrid);

            if (point.X > MenuStackPanel.ActualWidth + MenuStackPanel.Margin.Left + MenuStackPanel.Margin.Right)
            {
                CloseMenu();
            }
        }
    }
}