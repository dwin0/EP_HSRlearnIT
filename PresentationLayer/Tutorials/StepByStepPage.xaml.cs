using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using EP_HSRlearnIT.PresentationLayer.Exercises;
using EP_HSRlearnIT.PresentationLayer.Games;

namespace EP_HSRlearnIT.PresentationLayer.Tutorials
{
    public partial class StepByStepPage
    {
        #region Private Members
        private int _step;
        private int _title;
        private const int StepMin = 0;
        private const int SbsMin = 1;
        private const int SbsMax = 15;
        private const int StepMax = 18;

        private const int NumOfStepPaths = 24;
        private readonly Dictionary<int, Path> _stepPaths = new Dictionary<int, Path>();
        private int _highlightedPath;

        #endregion

        #region Constructors
        public StepByStepPage(int stepNumber)
        {
            InitializeComponent();

            if (StepMin <= stepNumber && stepNumber <= StepMax)
            {
                _title = stepNumber;
                Progress.SaveProgress("StepByStepPage_CurrentStep", stepNumber);
            }
            NavigationService?.Navigate(new StepByStepPage());
        }

        public StepByStepPage()
        {
            InitializeComponent();
            LoadBackground(StepByStepCanvas);

            var progressPaths = Progress.GetProgress("StepByStepPage_Paths");
            if (progressPaths != null)
            {
                _stepPaths = progressPaths as Dictionary<int, Path>;
            }
            else
            {
                LoadStepPaths(StepByStepCanvas);
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

        private void StepByStepPage_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    OnPreviousStepButton_Click(sender, e);
                    break;
                case Key.Right:
                    OnNextStepButton_Click(sender, e);
                    break;
                default:
                    return;
            }
        }

        
        private void OnJumpToStart_Click(object sender, RoutedEventArgs e)
        {
            foreach (var stepPath in _stepPaths)
            {
                ClearPath(stepPath.Value);
            }
            JumpToStep(StepMin);
        }

        private void JumpToStep(int step)
        {
            _step = step;
            Progress.SaveProgress("StepByStepPage_CurrentStep", _step);
            ReplaceContent(_step);
        }

        private void OnPreviousStepButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceContent(--_step);
            Progress.SaveProgress("StepByStepPage_CurrentStep", _step);
        }

        private void OnNextStepButton_Click(object sender, RoutedEventArgs e)
        {
            ReplaceContent(++_step);
            Progress.SaveProgress("StepByStepPage_CurrentStep", _step);
        }

        private void OnStartEncryptionDecryptionPages_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new EncryptionDecryptionTabs());
        }

        private void OnStartDragDropButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new DragDropPage());
        }

        private void ActivateButtons()
        {
            StartDragDrop.Visibility = Visibility.Visible;
            StartEncryptionDecryptionPages.Visibility = Visibility.Visible;
            Progress.SaveProgress("StepByStepPage_ButtonState", true);
        }

        private void SwitchNextOptionsWindow()
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
            if (!(StepMin <= stepNumber && stepNumber <= StepMax))
            {
                if (stepNumber < StepMin)
                {
                    _step = StepMin;
                }
                else
                {
                    _step = StepMax;
                }
            }
            return _step;
        }

        private void ReplaceContent(int stepNumber)
        {
            stepNumber = CheckRange(stepNumber);

            switch (stepNumber)
            {
                case StepMin:
                    PreviousStepButton.IsEnabled = false;
                    NextStepButton.IsEnabled = true;
                    break;
                case StepMax:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = false;
                    SwitchNextOptionsWindow();
                    ActivateButtons();
                    break;
                default:
                    PreviousStepButton.IsEnabled = true;
                    NextStepButton.IsEnabled = true;
                    SwitchNextOptionsWindow();
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
                    FillPath(toFillPath);
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

        private void SetInputAndOutput(int stepNumber)
        {
            if (stepNumber < SbsMin)
            {
                Input.Text = "Hier wird noch nichts berechnet";
                Output.Text = "Hier wird noch nichts berechnet";
            } else if (stepNumber > SbsMax)
            {
                Input.Text = "Hier wird nichts mehr berechnet.";
                Output.Text = "Hier wird nichts mehr berechnet.";
            }

            if (!Input.ClipToBounds)
            {
                InputScrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility) Visibility.Hidden;
            }
            Input.Text = Application.Current.TryFindResource("InputStep" + stepNumber) as string;

            if (!Output.ClipToBounds)
            {
                OutputScrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility)Visibility.Hidden;
            }
            Output.Text = Application.Current.TryFindResource("OutputStep" + stepNumber) as string;
        }

        private void LoadStepPaths(Canvas canvas)
        {
            for (int i = 1; i <= NumOfStepPaths; i++)
            {
                Path ressourcePath = Application.Current.FindResource("StepPath" + i) as Path;
                if (ressourcePath == null || !ressourcePath.Name.Contains("_stepByStep")) continue;

                //Create a copy of the Ressource StepPath to prevent multiple Event Listener on MouseEnter / MouseLeave
                Path stepPath = CopyPath(ressourcePath);
                _stepPaths.Add(i, stepPath);

                stepPath.SetValue(Panel.ZIndexProperty, 0);
                canvas.Children.Add(stepPath);
            }

            Progress.SaveProgress("StepByStepPage_StepPaths", _stepPaths);
        }

        private void FillPath(Path path)
        {
            path.Fill = Application.Current.FindResource("TileSolidColor") as SolidColorBrush;
        }

        private void ClearPath(Path path)
        {
            path.Fill = null;
        }

        private Path CopyPath(Path originalPath)
        {
            Path copy = new Path
            {
                Data = originalPath.Data.Clone(),
                Name = originalPath.Name,
                Style = originalPath.Style
            };
            return copy;
        }

        private void LoadBackground(Canvas canvas)
        {
            Image background = Application.Current.FindResource("StepByStepBackground") as Image;
            if (background == null) return;

            //Image has an existing Parent when this Page is opened a second time
            if (background.Parent is Canvas)
            {
                ((Canvas) background.Parent).Children.Remove(background);
            }

            background.SetValue(Panel.ZIndexProperty, 1);

            canvas.Children.Add(background);
        }

        private string WriteTitle(int stepNumber)
        {
            string title = GetTitle(stepNumber);
            if (title == null)
            {
                string oldTitle = GetTitle(_title);
                if (oldTitle == null || stepNumber != _title)
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
            if (Application.Current.Resources.Contains(titleName))
            {
                return Application.Current.FindResource(titleName) as string;
            }
            return null;
        }

        private string GetTitleNumber(int stepNumber)
        {
            string subtitle = "";
            int mainNumber = 0;
            if (SbsMin <= stepNumber && stepNumber <= SbsMax)
            {
                for (int listIndex = SbsMin; listIndex <= stepNumber; listIndex++)
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

        private void Button_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SetFocus(_step);
        }

        #endregion
    }
}