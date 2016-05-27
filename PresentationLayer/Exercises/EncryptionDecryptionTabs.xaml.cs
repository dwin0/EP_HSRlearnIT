using System.Windows;
using System.Windows.Controls;
using EP_HSRlearnIT.BusinessLayer.Persistence;
using EP_HSRlearnIT.BusinessLayer.Extensions;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    /// <summary>
    /// This class contains the logic for the tabs.
    /// </summary>
    public partial class EncryptionDecryptionTabs
    {
        #region Constructor
        /// <summary>
        /// This constructor can be called, if the default tab should be started. The default tab is the EncryptionPage.
        /// </summary>
        public EncryptionDecryptionTabs()
        {
            InitializeComponent();
            Page page = new EncryptionPage();
            EncryptionFrame.Navigate(page);
            InitializeVisibility(page);
        }

        /// <summary>
        /// If a tab change is made, this constructor is called.
        /// </summary>
        /// <param name="tabItem"></param>
        public EncryptionDecryptionTabs(TabItem tabItem)
        {
            InitializeComponent();

            Page page = null;

            if (tabItem.Name == "EncryptionItem")
            {
                //IsSelected has to be set manually, else the switching of the tabs won't work properly
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

            InitializeVisibility(page);
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
                    case "HexAadBox":
                    case "HexIvBox":
                    case "HexTagBox":
                    case "HexPasswordBox":
                        Progress.SaveProgress("DecryptionPage_" + element.Name, element.Text);
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
                    case "HexAadBox":
                    case "HexIvBox":
                    case "HexPasswordBox":
                        Progress.SaveProgress("EncryptionPage_" + element.Name, element.Text);
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
            Visibility visibility;

            if (expertmodus == null) { return;}
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

        private void InitializeVisibility(Page page)
        {
            var visibility = Progress.GetProgress("EncryptionDecryptionTabs_visibility");
            if (visibility != null)
            {
                SetVisibility(page?.Content as Grid, (Visibility)visibility);
            }
        }

        private void SetVisibility(Grid grid, Visibility visibility)
        {
            if (grid == null) { return;}

            foreach (var child in DependencyObjectExtension.GetAllChildren<FrameworkElement>(grid))
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

        #endregion
    }
}
