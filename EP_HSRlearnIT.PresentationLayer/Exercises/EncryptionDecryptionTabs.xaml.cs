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
            InitializeExpertMode(page);
        }


        /// <summary>
        /// If a tab change is made, this constructor is called.
        /// </summary>
        /// <param name="tabItem">Contains the name of the page to navigate next.</param>
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

            InitializeExpertMode(page);
        }

        #endregion

        #region Private Methods
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var sendingPage in this.GetAllChildren<Page>())
            {
                switch (sendingPage.Title)
                {
                    case "EncryptionPage":
                        TakeValuesToDecryption();
                        NavigationService?.Navigate(new EncryptionDecryptionTabs(DecryptionItem));
                        break;
                    case "DecryptionPage":
                        TakeValuesToEncryption();
                        NavigationService?.Navigate(new EncryptionDecryptionTabs(EncryptionItem));
                        break;
                } 
            }
        }

        private void TakeValuesToDecryption()
        {
            foreach (var element in this.GetAllChildren<TextBox>())
            {
                if (element.Name != "HexPlaintextBox")
                {
                    Progress.SaveProgress("DecryptionPage_" + element.Name, element.Text);
                }
            }
        }

        private void TakeValuesToEncryption()
        {
            foreach (var element in this.GetAllChildren<TextBox>())
            {
                if (element.Name != "HexCiphertextBox" || element.Name != "HexTagBox")
                {
                    Progress.SaveProgress("EncryptionPage_" + element.Name, element.Text);
                }
            }
        }

        private void ExpertmodusButton_OnClick(object sender, RoutedEventArgs e)
        {
            Grid grid = (((TabControl.SelectedItem as TabItem)?.Content as Frame)?.Content as Page)?.Content as Grid;

            Visibility visibility = ExpertmodusButton.Content.ToString().Contains("aus") ? Visibility.Visible : Visibility.Collapsed;

            SetVisibility(grid, visibility);
        }

        private void InitializeExpertMode(Page page)
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

            foreach (var child in grid.GetAllChildren<FrameworkElement>())
            {
                if (child.Name.Contains("Hex"))
                {
                    child.Visibility = visibility;
                }

                GridLength gridLength;
                if (visibility == Visibility.Visible)
                {
                    gridLength = new GridLength(1, GridUnitType.Star);
                    ExpertmodusButton.Content = "Expertenmodus: eingeschaltet";
                }
                else
                {
                    gridLength = new GridLength(0);
                    ExpertmodusButton.Content = "Expertenmodus: ausgeschaltet";
                }

                grid.ColumnDefinitions[1].Width = gridLength;

            }
            Progress.SaveProgress("EncryptionDecryptionTabs_visibility", visibility);
        }

        #endregion
    }
}
