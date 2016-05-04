using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using WPFFolderBrowser;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    /// <summary>
    /// Interaktionslogik für SaveMultiFileDialog.xaml
    /// </summary>
    public partial class SaveMultiFileDialog : Window
    {
        #region Private Members
        private readonly Dictionary<string, string> _exportFiles = new Dictionary<string, string>();

        #endregion
        

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
            
            string folderPath = folderBrowserDialog.FileName;
            if (folderPath != null)
            {
                Console.WriteLine(folderPath);
                Progress.SaveProgress("SaveMultiFileDialog_ExportPath", folderPath);
                if (_exportFiles != null)
                {
                    Progress.SaveProgress("SaveMultiFileDialog_Export", _exportFiles);
                    Close();
                }
                else
                {
                    MessageBox.Show("Wähle die zu exportierenden Dateien aus.", "Fehlende Exportdateien", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            else
            {
                MessageBox.Show("Wähle ein Speicherverzeichnis.", "Fehlendes Speicherverzeichnis", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void OnAbortButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Utf_OnChecked(object sender, RoutedEventArgs e)
        {
            
            SaveCheckedBoxStatus(sender, ".utf");
        }

        private void Hex_OnChecked(object sender, RoutedEventArgs e)
        {
            SaveCheckedBoxStatus(sender, ".hex");
        }

        private void SaveCheckedBoxStatus(object sender, string fileExtension)
        {
            string ending = fileExtension;
            CheckBox check = sender as CheckBox;
            if (check?.IsChecked == true)
            {
                string checkboxName = check.Name;
                //Index 3 because cut substring Hex -> for example: HexIv cutting to Iv
                string parameterName = checkboxName.Substring(3);
                TextBox textField = FindName("FileName" + parameterName) as TextBox;
                if (textField != null)
                {
                    if (textField.Text == "")
                    {
                        textField.Text = parameterName;
                    }
                    string fileName = textField.Text + ending;
                    Console.WriteLine(fileName);

                    if (_exportFiles.ContainsKey(checkboxName))
                    {
                        _exportFiles[checkboxName] = fileName;
                    }
                    else
                    {
                        _exportFiles.Add(checkboxName, fileName);
                    }
                }
                else
                {
                    if (check.IsChecked == false)
                    {
                        if (_exportFiles.ContainsKey(checkboxName))
                        {
                            _exportFiles.Remove(checkboxName);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
