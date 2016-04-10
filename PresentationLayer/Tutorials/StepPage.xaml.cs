﻿using System.Windows;
﻿using System;
using System.Windows.Controls;

namespace EP_HSRlearnIT.PresentationLayer.Tutorials
{
    /// <summary>
    /// Interaction logic for StepPage.xaml
    /// </summary>
    public partial class StepPage : Page
    {
        #region Private Members

        //Utilities
        private MainWindow _mainWindow;

        private int _step = 1;
        private const int _SMALLEST_STEP = 1;
        private const int _BIGGEST_STEP = 3;
        #endregion


        #region Constructors
        public StepPage(MainWindow mainWindow)
        {
            InitializeComponent();

            //Utilities
            _mainWindow = mainWindow;

            //Utilities
            object progress =_mainWindow.utilies.progress.TryGetProgress("CurrentStep");
            if(progress != null)
            {
                _step = Convert.ToInt32(progress);
            }
            
            ReplaceText(_step);

            
        }
        #endregion


        #region Private Methods

        private void OnPreviousStepButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceText(--_step);

            //Utilities
            _mainWindow.utilies.progress.saveProgress("CurrentStep", _step);
        }

        private void OnNextStepButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceText(++_step);

            //Utilities
            _mainWindow.utilies.progress.saveProgress("CurrentStep", _step);
        }

        private void ReplaceText(int stepNumber)
        {
            switch(stepNumber)
            {

                case _SMALLEST_STEP:
                    PreviousStepButton.IsEnabled = false;
                    NextStepButton.IsEnabled = true;
                    break;
                case _BIGGEST_STEP:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = false;
                    break;
                default:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = true;
                    break;
            }

            StepDescriptionBox.Text = Application.Current.FindResource("Step" + stepNumber) as string;
            StepTitle.Text = "Schritt " + stepNumber;
        }
        #endregion
    }
}