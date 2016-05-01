using System;
using System.Windows;
using WPFFolderBrowser;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    /// <summary>
    /// Interaktionslogik für SaveMultiFileDialog.xaml
    /// </summary>
    public partial class SaveMultiFileDialog : Window
    {
        public SaveMultiFileDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void ExportFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            WPFFolderBrowserDialog folderBrowserDialog = new WPFFolderBrowserDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (folderBrowserDialog.ShowDialog() == true)
            {
                //Update/Save all Variables into an array/dictionary
                string folderPath = folderBrowserDialog.FileName;
                Console.WriteLine(folderPath);
                //start foreach about array for all entities with the folderPath
            }
        }

        private void AbortButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
