using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EP_HSRlearnIT.BusinessLayer.Extensions;
using EP_HSRlearnIT.BusinessLayer.Persistence;
using EP_HSRlearnIT.PresentationLayer.Exercises;
using EP_HSRlearnIT.PresentationLayer.Games;

namespace EP_HSRlearnIT.PresentationLayer.Tutorials
{
    /// <summary>
    /// Page to inform the user about AES-GCM with a StepByStep-Tutorial.
    /// </summary>
    public partial class StepByStepPage
    {
        #region Private Members
        private int _step;
        private int _titleNumber;
        private const int StepMin = 0;
        private const int PathMin = 1;
        private const int PathMax = 15;
        private const int StepMax = 18;
        private readonly Dictionary<int, Path> _stepPaths = new Dictionary<int, Path>();
        private int _highlightedPath;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor which generates a StepByStep-Page starting with the given stepNumber.
        /// </summary>
        /// <param name="stepNumber">Number of the step which is loaded.</param>
        public StepByStepPage(int stepNumber)
        {
            InitializeComponent();

            if (StepMin <= stepNumber && stepNumber <= StepMax)
            {
                _titleNumber = stepNumber;
                Progress.SaveProgress("StepByStepPage_CurrentStep", stepNumber);
            }
            NavigationService?.Navigate(new StepByStepPage());
        }

        /// <summary>
        /// Constructor which loads a StepByStep-Page including clickable image, description and calculation of AES-GCM.
        /// </summary>
        public StepByStepPage()
        {
            InitializeComponent();
            LoadBackground(StepByStepCanvas, "StepByStepBackground");

            var progressPaths = Progress.GetProgress("StepByStepPage_Paths");
            if (progressPaths != null)
            {
                _stepPaths = progressPaths as Dictionary<int, Path>;
            }
            else
            {
                LoadStepByStepPaths(StepByStepCanvas);
            }

            var progressCurrentStep = Progress.GetProgress("StepByStepPage_CurrentStep");
            if (progressCurrentStep != null)
            {
                _step = Convert.ToInt32(progressCurrentStep);
            }

            ReplaceContent(_step);

            var progressActivateButtons = (bool?)Progress.GetProgress("StepByStepPage_ButtonState");
            if (progressActivateButtons != null && progressActivateButtons.Value)
            {
                ActivateButtons();
            }
            SetFocus(_step);
            LoadStepPaths(StepByStepCanvas);
        }

        #endregion


        #region Private Methods
        private void SetFocus(int step)
        {
            if (step < StepMax)
            {
                NextStepButton.Focus();
            }
            else
            {
                PreviousStepButton.Focus();
            }
        }

        private void StepButton_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    PreviousStepButton_OnClick(sender, e);
                    break;
                case Key.Right:
                    NextStepButton_OnClick(sender, e);
                    break;
                default:
                    return;
            }
        }
        
        private void JumpToStart_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var stepPath in _stepPaths)
            {
                ClearPath(stepPath.Value);
            }
            _step = StepMin;
            Progress.SaveProgress("StepByStepPage_CurrentStep", _step);
            ReplaceContent(_step);
        }

        private void PreviousStepButton_OnClick(object sender, RoutedEventArgs e)
        {
            ReplaceContent(--_step);
            Progress.SaveProgress("StepByStepPage_CurrentStep", _step);
        }

        private void NextStepButton_OnClick(object sender, RoutedEventArgs e)
        {
            ReplaceContent(++_step);
            Progress.SaveProgress("StepByStepPage_CurrentStep", _step);
        }

        private void StartEncryptionDecryption_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new EncryptionDecryptionTabs());
        }

        private void StartDragDrop_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new DragDropPage());
        }

        private void ActivateButtons()
        {
            StartDragDrop.Visibility = Visibility.Visible;
            StartEncryptionDecryption.Visibility = Visibility.Visible;
            Progress.SaveProgress("StepByStepPage_ButtonState", true);
        }

        private void ReplaceContent(int stepNumber)
        {
            stepNumber = CheckRange(stepNumber);
            ShowNextOptionsWindow();

            switch (stepNumber)
            {
                case StepMin:
                    PreviousStepButton.IsEnabled = false;
                    NextStepButton.IsEnabled = true;
                    break;
                case StepMax:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = false;
                    ActivateButtons();
                    break;
                default:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = true;
                    break;
            }

            StepDescriptionBox.Text = Application.Current.FindResource("Step" + stepNumber) as string;
            if (!StepDescriptionBox.ClipToBounds)
            {
                TextScrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility)Visibility.Hidden;
            }
            StepTitle.Text = GetTitleNumber(stepNumber) + WriteTitle(stepNumber);
            SetInputAndOutput(stepNumber);
            DrawImage(stepNumber);
        }

        private void ShowNextOptionsWindow()
        {
            var buttonStatus = (bool?)Progress.GetProgress("StepByStepPage_ButtonState");
            if ((buttonStatus == null || buttonStatus == false) && _step == StepMax)
            {
                NextOptionsWindow.Visibility = Visibility.Visible;
            }
            else
            {
                NextOptionsWindow.Visibility = Visibility.Hidden;
            }
        }

        private int CheckRange(int stepNumber)
        {
            if (StepMin <= stepNumber && stepNumber <= StepMax) { return _step; }
            _step = stepNumber < StepMin ? StepMin : StepMax;
            return _step;
        }

        private void SetInputAndOutput(int stepNumber)
        {
            var inputText = Application.Current.TryFindResource("InputStep" + stepNumber) as string;
            if (inputText == null)
            {
                Input.Text = "Hier wird nichts berechnet";
            }
            else
            {
                Input.Text = inputText;
                if (!Input.ClipToBounds)
                {
                    InputScrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility)Visibility.Hidden;
                }
            }

            
            var outputText = Application.Current.TryFindResource("OutputStep" + stepNumber) as string;
            if (outputText == null)
            {
                Output.Text = "Hier wird nichts berechnet";
            } else
            {
                Output.Text = outputText;
                if (!Output.ClipToBounds)
                {
                    OutputScrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility)Visibility.Hidden;
                }
            }


            if (stepNumber < PathMin)
            {
                Input.Text = "Hier wird noch nichts berechnet";
                Output.Text = "Hier wird noch nichts berechnet";
            }
            else if (stepNumber > PathMax)
            {
                Input.Text = "Hier wird nichts mehr berechnet";
                Output.Text = "Hier wird nichts mehr berechnet";
            }
        }

        private void DrawImage(int stepNumber)
        {
            if (_stepPaths.ContainsKey(stepNumber))
            {
                StepViewBox.Visibility = Visibility.Visible;
                StepImage.Visibility = Visibility.Hidden;
                Path toFillPath;
                Path toEmptyPath;
                bool newPath = _stepPaths.TryGetValue(stepNumber, out toFillPath);
                bool oldPath = _stepPaths.TryGetValue(_highlightedPath, out toEmptyPath);
                
                if (newPath)
                {   
                    FillupPath(toFillPath);
                }
                if (oldPath)
                {
                    ClearPath(toEmptyPath);
                }
                _highlightedPath = stepNumber;
            }
            else
            {
                StepViewBox.Visibility = Visibility.Hidden;
                StepImage.Visibility = Visibility.Visible;
                StepImage.Source = new BitmapImage(new Uri(@"/Images/Step" + stepNumber + ".png", UriKind.RelativeOrAbsolute));
                _highlightedPath = stepNumber;
            }
        }

        private void LoadStepByStepPaths(Canvas canvas)
        {
            for (int i = 1; i <= NumOfStepPaths; i++)
            {
                Path ressourcePath = Application.Current.FindResource("StepPath" + i) as Path;
                if (ressourcePath == null || !ressourcePath.Name.Contains("_stepByStep")) continue;

                //Create a copy of the Ressource StepPath to prevent multiple Event Listener on MouseEnter / MouseLeave
                Path stepPath = ressourcePath.Clone() as Path;
                _stepPaths.Add(i, stepPath);

                if (stepPath != null)
                {
                    stepPath.SetValue(Panel.ZIndexProperty, 0);
                    canvas.Children.Add(stepPath);
                }
            }
            Progress.SaveProgress("StepByStepPage_StepPaths", _stepPaths);
        }

        private void FillupPath(Path path)
        {
            path.Stroke = Application.Current.FindResource("MenuBorderBrush") as SolidColorBrush;
            path.Fill = Application.Current.FindResource("BackAreaBrush") as SolidColorBrush; 
            path.Opacity = 0.4;
            path.SetValue(Panel.ZIndexProperty, 1);
        }

        private void ClearPath(Path path)
        {
            path.ClearValue(Panel.ZIndexProperty);
            path.Fill = null;
            path.Stroke = null;
        }

        private string WriteTitle(int stepNumber)
        {
            string title = GetTitle(stepNumber);
            if (title == null)
            {
                string oldTitle = GetTitle(_titleNumber);
                if (oldTitle == null || stepNumber != _titleNumber)
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
            _titleNumber = stepNumber;
            return title;
        }

        private string GetTitle(int titleNumber)
        {
            string titleName = "Step" + titleNumber + "Title";
            return Application.Current.TryFindResource(titleName) as string;
        }

        private string GetTitleNumber(int stepNumber)
        {
            string subtitle = "";
            int mainNumber = 0;
            if (PathMin <= stepNumber && stepNumber <= PathMax)
            {
                for (int listIndex = PathMin; listIndex <= stepNumber; listIndex++)
                {
                    if (GetSubtitleNumber(listIndex) == 1)
                    {
                        mainNumber++;
                    }
                }
                subtitle = mainNumber + "." + GetSubtitleNumber(stepNumber) + " ";
            }
            return subtitle;
        }

        private int GetSubtitleNumber(int stepNumber)
        {
            if (GetTitle(stepNumber) != null)
            {
                return 1;
            }
            stepNumber--;
            int subtitle = 1;
            return subtitle + GetSubtitleNumber(stepNumber);
        }

        private void StepButton_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SetFocus(_step);
        }

        #endregion
    }
}