using System.Windows;
﻿using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.PresentationLayer.Tutorials
{
    /// <summary>
    /// Interaction logic for StepPage.xaml
    /// </summary>
    public partial class StepPage : Page
    {
        #region Private Members
        private int _step = 1;
        private const int StepMin = 1;
        private const int StepMax = 3; //TODO: Change max. Steps

        #endregion


        #region Constructors
        public StepPage()
        {
            InitializeComponent();
            var progress = Progress.GetProgress("CurrentStep");
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
            Progress.SaveProgress("StepPage_StepImage", _step);
        }

        private void OnNextStepButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceText(++_step);
            Progress.SaveProgress("StepPage_StepImage", _step);
        }

        private void ReplaceText(int stepNumber)
        {
            switch(stepNumber)
            {

                case StepMin:
                    PreviousStepButton.IsEnabled = false;
                    NextStepButton.IsEnabled = true;
                    break;
                case StepMax:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = false;
                    break;
                default:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = true;
                    break;
            }

            StepDescriptionBox.Text = Application.Current.FindResource("Step" + stepNumber) as string;
            StepImage.Source = new BitmapImage(new Uri(@"/Images/Step" + stepNumber + ".png", UriKind.RelativeOrAbsolute));
            StepTitle.Text = "Schritt " + stepNumber;

        }
        #endregion
    }
}
