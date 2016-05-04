using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using Microsoft.Win32;
using Button = System.Windows.Controls.Button;


namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    /// <summary>
    /// Interaktionslogik for OpenMultiFileDialog.xaml
    /// </summary>
    public partial class OpenMultiFileDialog : Window
    {
        #region Private Members
        private readonly Dictionary<string, string> _fileList = new Dictionary<string, string>();

        #endregion

        #region Constructors
        public OpenMultiFileDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods
        private void AbortButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "HEX files (*.hex)|*.hex|All files (*.*)|*.*"
            };

            if(openFileDialog.ShowDialog() != true) return;

            var button = sender as Button;
            if (button != null)
            {
                //Index 6 because cut substring button -> for example: ButtonIV cutting to IV
                string fieldName = button.Name.Substring(6);
                TextBox textField = FindName("Text" + fieldName) as TextBox;

                if (textField != null)
                {
                    textField.Text = openFileDialog.FileName;
                    //Set cursorfocus at the end of textbox
                    textField.Select(textField.Text.Length, 0);

                    //fillup internal array
                    if (_fileList.ContainsKey(fieldName))
                    {
                        _fileList[fieldName] = textField.Text;
                    }
                    else
                    {
                        _fileList.Add(fieldName, textField.Text);
                    }
                    
                }
            }
        }
        private void ImportButton_OnClick(object sender, RoutedEventArgs e)
        {
            Progress.SaveProgress("OpenMultiFileDialog_Import", _fileList);
            Close();
        }

        #endregion

    }
}
