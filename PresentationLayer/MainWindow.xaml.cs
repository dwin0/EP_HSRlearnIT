using EP_HSRlearnIT.PresentationLayer.Tutorials;
using EP_HSRlearnIT.PresentationLayer.Games;
using System.Windows;
using System.Windows.Input;
using System;
using System.Windows.Media.Animation;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using EP_HSRlearnIT.PresentationLayer.Exercises;

namespace EP_HSRlearnIT.PresentationLayer
{
    /// <summary>
    /// Window to present all pages and the menu
    /// </summary>
    public partial class MainWindow
    {
        #region Private Members

        private string _mouseDownMenuItem;
        #endregion

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

        #region Private Methods

        private void MenuButton_OnClick(object sender, RoutedEventArgs e)
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

        private void MenuItem_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mouseDownMenuItem = ((FrameworkElement)e.OriginalSource).DataContext as string;
        }

        private void MenuItem_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as string;
            if (item == null)
            {
                ExceptionLogger.WriteToLogfile("MenuItem_OnMouseUp", "item was null", "");
            } else if (item.Equals(_mouseDownMenuItem))
            {
                switch (item)
                {
                    case "Startseite":
                        MainFrame.Navigate(new MainPage());
                        break;
                    case "AES-GCM - Übersicht":
                        MainFrame.Navigate(new AesGcmOverviewPage());
                        break;
                    case "Schritt für Schritt - Anleitung":
                        MainFrame.Navigate(new StepByStepPage());
                        break;
                    case "Ver- & Entschlüsselungs - Anwendung":
                        MainFrame.Navigate(new EncryptionDecryptionTabs());
                        break;
                    case "Drag & Drop - Spiel":
                        MainFrame.Navigate(new DragDropPage());
                        break;
                }
                CloseMenu();
            }
        }

        private void CloseMenu()
        {
            MenuStackPanel.Visibility = Visibility.Collapsed;
        }

        private void OpenMenu()
        {
            MenuStackPanel.Visibility = Visibility.Visible;
            Storyboard sb = Application.Current.FindResource("StoryboardShowLeftMenu") as Storyboard;
            sb?.Begin(MenuStackPanel);
        }

        //This method is used only for a system test in order to control the correctness
        //of the Global Exception Handler and will be deleted for the production code
        private void Save_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new Exception("Exception in Logfile vorhanden?");
        }

        private void CollapseMenu_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(MainGrid);

            //Check if the menu is open and the click was outside the menu
            if (MenuStackPanel.IsVisible && point.X > MenuStackPanel.ActualWidth + MenuStackPanel.Margin.Left + MenuStackPanel.Margin.Right)
            {
                CloseMenu();
                e.Handled = true;
            }
        }
        #endregion
    }
}