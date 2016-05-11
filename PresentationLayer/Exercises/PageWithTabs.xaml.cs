using System;
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
                if (element.Name == "HexCiphertextBox")
                {
                    string ciphertext = element.Text;
                    Progress.SaveProgress("DecryptionPage_HexCiphertextBox", ciphertext);
                }
                else if (element.Name == "HexAadBox")
                {
                    string aad = element.Text;
                    Progress.SaveProgress("DecryptionPage_HexAadBox", aad);
                }
                else if (element.Name == "HexIvBox")
                {
                    string iv = element.Text;
                    Progress.SaveProgress("DecryptionPage_HexIvBox", iv);
                }
                else if (element.Name == "HexTagBox")
                {
                    string tag = element.Text;
                    Progress.SaveProgress("DecryptionPage_HexTagBox", tag);
                }
            }
        }

        private void TakeValuesToEncryption()
        {
            foreach (var element in DependencyObjectExtension.GetAllChildren<TextBox>(this))
            {
                if (element.Name == "HexPlaintextBox")
                {
                    string plaintext = element.Text;
                    Progress.SaveProgress("EncryptionPage_HexPlaintextBox", plaintext);
                }
                else if (element.Name == "HexIvBox")
                {
                    string iv = element.Text;
                    Progress.SaveProgress("EncryptionPage_HexIvBox", iv);
                }
                else if (element.Name == "HexAadBox")
                {
                    string aad = element.Text;
                    Progress.SaveProgress("EncryptionPage_HexAadBox", aad);
                }
            }
        }

        #endregion

        
    }
}
