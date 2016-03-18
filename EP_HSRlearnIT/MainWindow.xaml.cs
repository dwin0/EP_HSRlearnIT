using EP_HSRlearnIT.Windows;
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

namespace EP_HSRlearnIT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MainPage(this));
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
                    MainFrame.Navigate(new StepPage());
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
    }
}
