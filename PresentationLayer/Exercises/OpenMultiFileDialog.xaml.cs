using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Button = System.Windows.Controls.Button;


namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    /// <summary>
    /// Interaktionslogik for OpenMultiFileDialog.xaml
    /// </summary>
    public partial class OpenMultiFileDialog : Window
    {
        public OpenMultiFileDialog()
        {
            InitializeComponent();
        }

        private void ImportButton_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO: foreach trought all parameters with the files and joined
            throw new NotImplementedException();
        }

        private void AbortButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Recent),
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if(openFileDialog.ShowDialog() != true) return;

            var button = sender as Button;
            if (button != null)
            {
                //Index 6 because cut substring button -> for example: ButtonIV cutting to IV
                string fieldName = "Text" + button.Name.Substring(6);
                TextBox textField = FindName(fieldName) as TextBox;
                if (textField != null)
                {
                    textField.Text = openFileDialog.FileName;
                }
            }
        }
    }
}
