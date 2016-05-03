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
        #region Constructors
        public SaveMultiFileDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods
        private void OnExportFilesButton_Click(object sender, RoutedEventArgs e)
        {
            WPFFolderBrowserDialog folderBrowserDialog = new WPFFolderBrowserDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (folderBrowserDialog.ShowDialog() != true) return;
            //TODO: Update/Save all Variables into an array/dictionary
            string folderPath = folderBrowserDialog.FileName;
            Console.WriteLine(folderPath);
            //TODO: start foreach about array for all entities with the folderPath
        }

        private void OnAbortButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

           #endregion

    }
}
