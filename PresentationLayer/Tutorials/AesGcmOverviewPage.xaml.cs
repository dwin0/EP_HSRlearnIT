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
    public partial class AesGcmOverviewPage
    {
        #region Constructors
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
                Path path = Application.Current.FindResource("AreaPath" + i) as Path;
                if (path == null) continue;

                //path has an existing Parent when this Page is opened a second time
                if(path.Parent is Canvas)
                {
                    (path.Parent as Canvas)?.Children.Remove(path);
                } else
                {
                    path.SetValue(Panel.ZIndexProperty, 2);
                    path.MouseEnter += AreaPathOnMouseEnter;
                    path.MouseLeave += AreaPathOnMouseLeave;
                }


                /*
                (path.Parent as Canvas)?.Children.Remove(path);

                path.SetValue(Panel.ZIndexProperty, 2);
                path.MouseEnter += AreaPathOnMouseEnter;
                path.MouseLeave += AreaPathOnMouseLeave;
                */

                canvas.Children.Add(path);
                _areaPaths[i - 1] = path;
            }
        }

        private void LoadStepPaths(Canvas canvas)
        {
            for (int i = 1; i <= NumOfStepPaths; i++)
            {
                Path path = Application.Current.FindResource("StepPath" + i) as Path;

                if (path == null || !path.Name.Contains("_overview")) continue;

                //path has an existing Parent when this Page is opened a second time
                (path.Parent as Canvas)?.Children.Remove(path);

                path.SetValue(Panel.ZIndexProperty, 3);
                path.MouseEnter += StepPathOnMouseEnter;
                path.MouseLeave += StepPathOnMouseLeave;
                path.MouseDown += StepPathOnClick;

                canvas.Children.Add(path);

                /*
                if(path.Parent is Canvas)
                {
                    (path.Parent as Canvas)?.Children.Remove(path);
                } else
                {
                    path.SetValue(Panel.ZIndexProperty, 3);
                    path.MouseEnter += StepPathOnMouseEnter;
                    path.MouseLeave += StepPathOnMouseLeave;
                    path.MouseDown += StepPathOnClick;
                }

                canvas.Children.Add(path);
                */

            }
        }

        private void LoadBackground(Canvas canvas)
        {
            Image background = Application.Current.FindResource("BackgroundImage") as Image;
            if (background == null) return;

            //image has an existing Parent when this Page is opened a second time
            (background.Parent as Canvas)?.Children.Remove(background);
            background.SetValue(Panel.ZIndexProperty, 1);

            canvas.Children.Add(background);
        }

        private void StepPathOnMouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;

            Path frontPath = sender as Path;
            if (frontPath == null) return;

            Path backPath;

            //When MouseOver the second time, the backRectangle already exists
            if (!_backPaths.ContainsKey(frontPath.Name))
            {
                backPath = new Path
                {
                    Data = frontPath.Data
                };

                backPath.SetValue(Panel.ZIndexProperty, 0);
                _backPaths.Add(frontPath.Name, backPath);

                (frontPath.Parent as Canvas)?.Children.Add(backPath);
            }
            else
            {
                backPath = _backPaths[frontPath.Name];
            }

            backPath.Fill = Application.Current.FindResource("BackAreaBrush") as SolidColorBrush;

            //Show Tooltip
            Geometry stepInfo = frontPath.Data;
            Geometry areaInfo;

            foreach(Path area in _areaPaths)
            {
                areaInfo = area.Data;

                IntersectionDetail detail = stepInfo.FillContainsWithDetail(areaInfo);

                if(detail == IntersectionDetail.FullyContains
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
            NavigationService?.Navigate(new StepPage());
        }
        #endregion Private Methods
    }
}
