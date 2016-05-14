using System;
using System.Windows;
using System.Windows.Controls;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    public partial class EncryptionDecryptionTabs
    {
        #region Constructor
        public EncryptionDecryptionTabs()
        {
            InitializeComponent();
            EncryptionFrame.Source = new Uri("EncryptionPage.xaml", UriKind.RelativeOrAbsolute);
        }

        public EncryptionDecryptionTabs(TabItem tabItem)
        {
            InitializeComponent();

            Page page = null;

            if (tabItem.Name == "EncryptionItem")
            {
                EncryptionItem.IsSelected = true;
                DecryptionItem.IsSelected = false;
                page = new EncryptionPage();
                EncryptionFrame.Navigate(page);
            }
            else if (tabItem.Name == "DecryptionItem")
            {
                EncryptionItem.IsSelected = false;
                DecryptionItem.IsSelected = true;
                page = new DecryptionPage();
                DecryptionFrame.Navigate(page);
            }

            var visibility = Progress.GetProgress("EncryptionDecryptionTabs_visibility");

            if (visibility != null)
            {
                SetVisibility(page?.Content as Grid, (Visibility)visibility);
            }
        }

        #endregion

        #region Private Methods
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var sendingPage in DependencyObjectExtension.GetAllChildren<Page>(this))
            {
                if (sendingPage.Title == "EncryptionPage")
                {
                    TakeValuesToDecryption();
                    NavigationService?.Navigate(new EncryptionDecryptionTabs(DecryptionItem));
                } else if (sendingPage.Title == "DecryptionPage")
                {
                    TakeValuesToEncryption();
                    NavigationService?.Navigate(new EncryptionDecryptionTabs(EncryptionItem));
                } 
            }
        }

        private void TakeValuesToDecryption()
        {
            foreach (var element in DependencyObjectExtension.GetAllChildren<TextBox>(this))
            {
                switch (element.Name)
                {
                    case "HexCiphertextBox":
                        Progress.SaveProgress("DecryptionPage_HexCiphertextBox", element.Text);
                        break;
                    case "HexAadBox":
                        Progress.SaveProgress("DecryptionPage_HexAadBox", element.Text);
                        break;
                    case "HexIvBox":
                        Progress.SaveProgress("DecryptionPage_HexIvBox", element.Text);
                        break;
                    case "HexTagBox":
                        Progress.SaveProgress("DecryptionPage_HexTagBox", element.Text);
                        break;
                    case "HexEncryptionPasswordBox":
                        Progress.SaveProgress("DecryptionPage_HexDecryptionPasswordBox", element.Text);
                        break;
                }
            }
        }

        private void TakeValuesToEncryption()
        {
            foreach (var element in DependencyObjectExtension.GetAllChildren<TextBox>(this))
            {
                switch (element.Name)
                {
                    case "HexPlaintextBox":
                        Progress.SaveProgress("EncryptionPage_HexPlaintextBox", element.Text);
                        break;
                    case "HexAadBox":
                        Progress.SaveProgress("EncryptionPage_HexAadBox", element.Text);
                        break;
                    case "HexIvBox":
                        Progress.SaveProgress("EncryptionPage_HexIvBox", element.Text);
                        break;
                    case "HexDecryptionPasswordBox":
                        Progress.SaveProgress("EncryptionPage_HexEncryptionPasswordBox", element.Text);
                        break;
                }
            }
        }

        private void OnExpertmodusButtonClick(object sender, RoutedEventArgs e)
        {
            Button expertmodus = e.Source as Button;
            TabItem selected = TabControl.SelectedItem as TabItem;
            Frame frame = selected?.Content as Frame;
            Page page = frame?.Content as Page;
            Grid grid = page?.Content as Grid;

            if (expertmodus != null)
            {
                Visibility visibility;

                if (expertmodus.Content.ToString().Contains("aus"))
                {
                    visibility = Visibility.Visible;
                }
                else
                {
                    visibility = Visibility.Collapsed;
                }

                SetVisibility(grid, visibility);
                Progress.SaveProgress("EncryptionDecryptionTabs_visibility", visibility);
            }
        }

        private void SetVisibility(Grid grid, Visibility visibility)
        {
            if (grid != null)
            {
                var children = DependencyObjectExtension.GetAllChildren<FrameworkElement>(grid);

                foreach (var child in children)
                {
                    if (child.Name.Contains("Hex"))
                    {
                        child.Visibility = visibility;
                    }

                    GridLength gridLength;
                    if (visibility == Visibility.Visible)
                    {
                        gridLength = new GridLength(1, GridUnitType.Star);
                        ExpertmodusButton.Content = "Expertenmodus: ein";
                    }
                    else
                    {
                        gridLength = new GridLength(0);
                        ExpertmodusButton.Content = "Expertenmodus: aus";
                    }

                    grid.ColumnDefinitions[1].Width = gridLength;

                }
            }
        }

        #endregion
    }
}
