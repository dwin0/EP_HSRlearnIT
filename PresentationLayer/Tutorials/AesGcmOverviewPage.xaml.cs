using System;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EP_HSRlearnIT.PresentationLayer.Tutorials
{
    /// <summary>
    /// Page to briefly inform about AES-GCM
    /// </summary>
    public partial class AesGcmOverviewPage
    {
        #region Constructors
        /// <summary>
        /// Method to initialize the XAML and the Content
        /// </summary>
        public AesGcmOverviewPage()
        {
            InitializeComponent();
            InitCanvas(OverviewCanvas);
        }
        #endregion

        #region Private Members
        private readonly ToolTip _toolTip = new ToolTip();

        private readonly Dictionary<string, Path> _backPaths = new Dictionary<string, Path>();
        private readonly Path[] _areaPaths = new Path[NumOfAreas];
        private Path _lastEnteredAreaPath;

        private const int NumOfAreas = 6;
        private const int NumOfStepPaths = 25;

        #endregion Private Members

        #region Private Methods
        private void InitCanvas(Canvas canvas)
        {
            LoadAreaPaths(canvas);
            LoadStepPaths(canvas);
            LoadBackground(canvas);
        }

        private void LoadAreaPaths(Canvas canvas)
        {
            for (int i = 1; i <= NumOfAreas; i++)
            {
                Path ressourcePath = Application.Current.FindResource("AreaPath" + i) as Path;
                if (ressourcePath == null) continue;

                //Create a copy of the Ressource Path to prevent multiple Event Listener on MouseEnter / MouseLeave
                Path areaPath = CopyPath(ressourcePath);

                areaPath.SetValue(Panel.ZIndexProperty, 2);
                areaPath.MouseEnter += AreaPathOnMouseEnter;
                areaPath.MouseLeave += AreaPathOnMouseLeave;

                canvas.Children.Add(areaPath);
                _areaPaths[i - 1] = areaPath;
            }
        }

        private void LoadStepPaths(Canvas canvas)
        {
            for (int i = 1; i <= NumOfStepPaths; i++)
            {
                Path ressourcePath = Application.Current.FindResource("StepPath" + i) as Path;
                if (ressourcePath == null || !ressourcePath.Name.Contains("_overview")) continue;

                //Create a copy of the Ressource Path to prevent multiple Event Listener on MouseEnter / MouseLeave
                Path stepPath = CopyPath(ressourcePath);

                stepPath.SetValue(Panel.ZIndexProperty, 3);
                stepPath.MouseEnter += StepPathOnMouseEnter;
                stepPath.MouseLeave += StepPathOnMouseLeave;
                stepPath.MouseDown += StepPathOnClick;

                canvas.Children.Add(stepPath);
            }
        }

        private void LoadBackground(Canvas canvas)
        {
            Image background = Application.Current.FindResource("BackgroundImage") as Image;
            if (background == null) return;

            //Image has an existing Parent when this Page is opened a second time
            if (background.Parent is Canvas)
            {
                ((Canvas) background.Parent).Children.Remove(background);
            }
            else
            {
                background.SetValue(Panel.ZIndexProperty, 1);
            }
            
            canvas.Children.Add(background);
        }

        private Path CopyPath(Path originalPath)
        {
            Path copy = new Path()
            {
                Data = originalPath.Data.Clone(),
                Name = originalPath.Name,
                Style = originalPath.Style
            };
            return copy;
        }

        private Path AddBackPath(Path frontPath)
        {
            Path backPath = CopyPath(frontPath);
            backPath.SetValue(Panel.ZIndexProperty, 0);
            _backPaths.Add(frontPath.Name, backPath);

            //Add backPath to the OverviewCanvas
            (frontPath.Parent as Canvas)?.Children.Add(backPath);

            return backPath;
        }

        private void StepPathOnMouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;

            Path frontPath = sender as Path;
            if (frontPath == null) return;
            Path backPath;

            //When MouseOver the second time, the backPath already exists
            if (!_backPaths.ContainsKey(frontPath.Name))
            {
                backPath = AddBackPath(frontPath);
            }
            else
            {
                backPath = _backPaths[frontPath.Name];
            }

            backPath.Fill = Application.Current.FindResource("BackAreaBrush") as SolidColorBrush;

            AddTooltip(frontPath, e);
        }

        private void AddTooltip(Path frontPath, MouseEventArgs e)
        {
            Geometry stepInfo = frontPath.Data;

            foreach (Path area in _areaPaths)
            {
                Geometry areaInfo = area.Data;
                IntersectionDetail detail = stepInfo.FillContainsWithDetail(areaInfo);

                if (detail == IntersectionDetail.FullyContains
                    || detail == IntersectionDetail.FullyInside
                    || detail == IntersectionDetail.Intersects)
                {
                    AreaPathOnMouseEnter(area, e);
                    _lastEnteredAreaPath = area;
                }
            }
        }

        private void StepPathOnMouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;

            Path frontPath = sender as Path;
            if (frontPath == null) return;

            Path backPath = _backPaths[frontPath.Name];
            backPath.Fill = Application.Current.FindResource("NoBackAreaBrush") as SolidColorBrush;

            AreaPathOnMouseLeave(_lastEnteredAreaPath, e);
        }

        private void AreaPathOnMouseEnter(object sender, MouseEventArgs e)
        {
            Path path = sender as Path;
            string text = Application.Current.FindResource(path?.Name + "Text") as string;
            ChangeToolTip(text);
        }

        private void AreaPathOnMouseLeave(object sender, MouseEventArgs e)
        {
            _toolTip.IsOpen = false;
        }

        private void ChangeToolTip(string tooltipText)
        {
            _toolTip.Content = tooltipText;
            _toolTip.IsOpen = true;
            _toolTip.StaysOpen = true;
            _toolTip.HorizontalOffset = 100;
            _toolTip.VerticalOffset = -100;
        }

        private void StepPathOnClick(object sender, MouseEventArgs e)
        {
            string pathName = (sender as Path)?.Name;

            //Example Path-Name: Step11_overview
            if (pathName != null)
            {
                int last = pathName.IndexOf("_", StringComparison.Ordinal);
                string stepNumber = pathName.Substring(4, last - 4);
                int step = int.Parse(stepNumber);

                //TODO Call Constructor of StepByStep
                Progress.SaveProgress("StepPage_CurrentStep", step);
            }
            NavigationService?.Navigate(new StepByStepPage());
        }

        #endregion Private Methods
    }
}
