using System.Windows;
﻿using System;
using System.Windows.Media.Imaging;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using EP_HSRlearnIT.Games;

namespace EP_HSRlearnIT.PresentationLayer.Tutorials
{
    public partial class StepPage
    {
        #region Private Members
        private int _step;
        private int _title;
        private const int StepMin = 0;
        private const int StepMax = 18;

        #endregion


        #region Constructors
        public StepPage()
        {
            InitializeComponent();

            var progressCurrentStep = Progress.GetProgress("StepPage_CurrentStep");
            if(progressCurrentStep != null)
            {
                _step = Convert.ToInt32(progressCurrentStep);      
            }

            var progressTitleStep = Progress.GetProgress("StepPage_Title");
            if (progressTitleStep != null)
            {
                _title = Convert.ToInt32(progressTitleStep);
            }

            ReplaceContent(_step);

            var progressActivateGame = Progress.GetProgress("StepPage_Game");
            if (progressActivateGame != null)
            {
                ActivateGameButton();
            }
        }

        #endregion


        #region Private Methods
        private void OnPreviousStepButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceContent(--_step);
            Progress.SaveProgress("StepPage_CurrentStep", _step);
            Progress.SaveProgress("StepPage_Title", _title);
        }

        private void OnNextStepButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceContent(++_step);
            Progress.SaveProgress("StepPage_CurrentStep", _step);
            Progress.SaveProgress("StepPage_Title", _title);
        }

        private void OnStartDragDropButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new DragDrop3());
        }

        private void ActivateGameButton()
        {
            StartDragDrop.Visibility = Visibility.Visible;
            Progress.SaveProgress("StepPage_GameButton", StartDragDrop.IsVisible);
        }

        private void ReplaceContent(int stepNumber)
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
                    ActivateGameButton();
                    break;
                default:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = true;
                    break;
            }

            StepDescriptionBox.Text = Application.Current.FindResource("Step" + stepNumber) as string;
            StepImage.Source = new BitmapImage(new Uri(@"/Images/Step" + stepNumber + ".png", UriKind.RelativeOrAbsolute));
            StepTitle.Text = WriteTitle(stepNumber);
        }

        private string WriteTitle(int stepNumber)
        {
            string title = GetTitle(stepNumber);
            if (title == null)
            {
                string oldTitle = GetTitle(_title);
                if (oldTitle == null || stepNumber < _title)
                {
                    for (int i = stepNumber; i >= StepMin; i--)
                    {
                        title = GetTitle(i);
                        if (title != null)
                        {
                            return title;
                        }
                    }
                }
                return oldTitle;
            }
            _title = stepNumber;
            return title;
        }

        private string GetTitle(int titleNumber)
        {
            string titleName = "Step" + titleNumber + "Title";
            if(Application.Current.Resources.Contains(titleName))
            {
                return Application.Current.FindResource(titleName) as string;
            }
            return null;
        }

        #endregion
    }
}
