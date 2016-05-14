using System;
using System.Windows;
using System.Windows.Controls;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    public partial class PageWithTabs
    {
        #region Constructor
        public PageWithTabs()
        {
            InitializeComponent();
            EncryptionFrame.Source = new Uri("EncryptionPage.xaml", UriKind.RelativeOrAbsolute);
        }

        public PageWithTabs(TabItem tabItem)
        {
            InitializeComponent();
            
            if (tabItem.Name == "EncryptionItem")
            {
                EncryptionItem.IsSelected = true;
                DecryptionItem.IsSelected = false;
                EncryptionFrame.Source = new Uri("EncryptionPage.xaml", UriKind.RelativeOrAbsolute);
            }
            else if (tabItem.Name == "DecryptionItem")
            {
                EncryptionItem.IsSelected = false;
                DecryptionItem.IsSelected = true;
                DecryptionFrame.Source = new Uri("DecryptionPage.xaml", UriKind.RelativeOrAbsolute);
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
                    NavigationService?.Navigate(new PageWithTabs(DecryptionItem));
                } else if (sendingPage.Title == "DecryptionPage")
                {
                    TakeValuesToEncryption();
                    NavigationService?.Navigate(new PageWithTabs(EncryptionItem));
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

        public void OnExpertmodusButtonClick(object sender, RoutedEventArgs e)
        {
            Button expertmodus = e.Source as Button;
            if (expertmodus != null)
            {
                if (expertmodus.Content.ToString().Contains("aus"))
                {
                    expertmodus.Content = "Expertenmodus: ein";
                }
                else
                {
                    expertmodus.Content = "Expertenmodus: aus";
                }
            }
        }

        #endregion
    }
}
