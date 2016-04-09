using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using EP_HSRlearnIT.PresentationLayer.Exercises;
using EP_HSRlearnIT.PresentationLayer.Games;
using EP_HSRlearnIT.PresentationLayer.Tutorials;
using System.Windows;
using System.Windows.Input;
using System;

namespace EP_HSRlearnIT.PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Utilities utilies { get; }

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MainPage(this));
            //AppDomain.CurrentDomain.UnhandledException += LoggingHandler.unhandledExceptionTrapper;

            //Utilities
            utilies = new Utilities();
        }
        #endregion

        private void OnMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if(MenuStackPanel.IsVisible)
            {
                CloseMenu();
            } else
            {
                OpenMenu();
            }
        }


        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as string;
            switch (item)
            {
                case "AES-GCM Übersicht":
                    MainFrame.Navigate(new AesGcmOverviewPage());
                    break;
                case "Schritt für Schritt":
                    MainFrame.Navigate(new StepPage(this));
                    break;
                case "Ver- und Entschlüsselung":
                    MainFrame.Navigate(new EncryptionPage());
                    break;
                case "Drag- und Drop Spiel":
                    MainFrame.Navigate(new DragDropPage());
                    break;
            }
            CloseMenu();
        }

        private void OnBackButton_Click(object sender, RoutedEventArgs e)
        {
            if(MainFrame.NavigationService.CanGoBack)
            {
                MainFrame.NavigationService.GoBack();
            }
        }

        private void OnForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if(MainFrame.NavigationService.CanGoForward)
            {
                MainFrame.NavigationService.GoForward();
            }
        }

        private void CloseMenu()
        {
            MenuStackPanel.Visibility = Visibility.Collapsed;
            var margin = MenuButton.Margin;
            margin.Left = 0;
            MenuButton.Margin = margin;
            MenuButton.Content = ">>";
        }

        private void OpenMenu()
        {
            MenuStackPanel.Visibility = Visibility.Visible;
            var margin = MenuButton.Margin;
            margin.Left = 150;
            MenuButton.Margin = margin;
            MenuButton.Content = "<<";
        }


        private void OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileSaver newFile = new FileSaver();
            newFile.CreateFile("AES-GCM", 20, "You are starting with the learntool!");
        }

        private void OnExceptionClick(object sender, MouseButtonEventArgs e)
        {
            throw new Exception("Exception in Logfile vorhanden?");
        }
    }
}
