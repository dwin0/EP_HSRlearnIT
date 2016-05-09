using System.Windows.Controls;
using EP_HSRlearnIT.PresentationLayer.Exercises;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    public partial class PageWithTabs
    {
        #region Constructor
        public PageWithTabs()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var sendingPage in DependencyObjectExtension.GetAllChildren<Page>(this))
            {
                if (sendingPage.Title == "EncryptionPage")
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
                } else if (sendingPage.Title == "DecryptionPage")
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
            }
        }

        #endregion

        
    }
}
