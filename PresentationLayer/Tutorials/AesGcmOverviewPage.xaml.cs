﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.PresentationLayer.Tutorials
{
    /// <summary>
    /// Page to briefly inform about AES-GCM
    /// </summary>
    public partial class AesGcmOverviewPage
    {
        #region Private Members

        private Path _mouseDownPath;
        private readonly Path[] _areaPaths = new Path[NumOfAreas];
        private readonly Dictionary<string, Path> _backPaths = new Dictionary<string, Path>();
        private const int NumOfAreas = 6;
        private const int NumOfStepPaths = 24;
        #endregion

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
                if (ressourcePath == null)
                {
                    ExceptionLogger.WriteToLogfile("A ressourcePath was not found - ressourcePath was null", "AesGcmOverviewPage: LoadAreaPahts");
                    continue;
                }

                //Create a copy of the Ressource AreaPath to prevent multiple Event Listener on MouseEnter
                Path areaPath = Clone(ressourcePath) as Path;
                if (areaPath == null)
                {
                    ExceptionLogger.WriteToLogfile("Path could not be copied - Copy was null", "AesGcmOverviewPage: LoadAreaPaths");
                    return;
                }

                areaPath.SetValue(Panel.ZIndexProperty, 2);
                areaPath.MouseEnter += AreaPathOnMouseEnter;

                canvas.Children.Add(areaPath);
                _areaPaths[i - 1] = areaPath;
            }
        }

        private void LoadStepPaths(Canvas canvas)
        {
            for (int i = 1; i <= NumOfStepPaths; i++)
            {
                Path ressourcePath = Application.Current.FindResource("StepPath" + i) as Path;
                if (ressourcePath == null)
                {
                    ExceptionLogger.WriteToLogfile("A ressourcePath was not found - ressourcePath was null", "AesGcmOverviewPage: LoadStepPahts");
                    continue;
                }
                if (!ressourcePath.Name.Contains("_overview"))
                {
                    continue;
                }

                //Create a copy of the Ressource StepPath to prevent multiple Event Listener on MouseEnter / MouseLeave
                Path stepPath = Clone(ressourcePath) as Path;
                if (stepPath == null)
                {
                    ExceptionLogger.WriteToLogfile("Path could not be copied - Copy was null", "AesGcmOverviewPage: LoadStepPaths");
                    continue;
                }

                stepPath.SetValue(Panel.ZIndexProperty, 3);
                stepPath.MouseEnter += StepPathOnMouseEnter;
                stepPath.MouseLeave += StepPathOnMouseLeave;
                //Mouse Up and Down - Events to make it feel lika a click. There is no Click-Event for Paths.
                stepPath.MouseDown += StepPathOnMouseDown;
                stepPath.MouseUp += StepPathOnMouseUp;

                canvas.Children.Add(stepPath);
            }
        }

        private void LoadBackground(Canvas canvas)
        {
            Image background = Application.Current.FindResource("BackgroundImage") as Image;
            if (background == null)
            {
                ExceptionLogger.WriteToLogfile("Background-Image was not found - background was null", "AesGcmOverviewPage: LoadBackground");
                return;
            }

            //Image has an existing Parent when this Page is opened a second time
            if (background.Parent is Canvas)
            {
                ((Canvas) background.Parent).Children.Remove(background);
            }

            background.SetValue(Panel.ZIndexProperty, 1);
            
            canvas.Children.Add(background);
        }


        #region StepPath-Methods

        private void StepPathOnMouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;

            Path stepPath = sender as Path;
            if (stepPath == null)
            {
                ExceptionLogger.WriteToLogfile("No stepPath was found - stepPath was null", "AesGcmOverviewPage: StepPathOnMouseEnter");
                return;
            }

            Path backPath;
            _backPaths.TryGetValue(stepPath.Name, out backPath);
            //When the Mouse enters a second time, the backPath already exists
            if (backPath == null)
            {
                backPath = AddBackPath(stepPath);
                if (backPath == null)
                {
                    ExceptionLogger.WriteToLogfile("backPath could not be added - backPath was null", "AesGcmOverviewPage: StepPathOnMouseEnter");
                    return;
                }
            }

            backPath.Fill = Application.Current.FindResource("BackAreaBrush") as SolidColorBrush;

            //Find Area Path to show the explanation
            Path areaPath = FindAreaPath(stepPath);
            if (areaPath != null)
            {
                AreaPathOnMouseEnter(areaPath, e);
            }
        }

        /// <summary>
        /// Method to highlight the background of a step within the AES-GCM - Overview Image
        /// </summary>
        /// <param name="stepPath">StepPath to highlight</param>
        /// <returns></returns>
        private Path AddBackPath(Path stepPath)
        {
            Path backPath = Clone(stepPath) as Path;
            if (backPath == null)
            {
                ExceptionLogger.WriteToLogfile("Path could not be copied - Copy was null", "AesGcmOverviewPage: AddBackPath");
                return null;
            }

            backPath.SetValue(Panel.ZIndexProperty, 0);
            _backPaths.Add(stepPath.Name, backPath);

            //Add backPath to the OverviewCanvas
            (stepPath.Parent as Canvas)?.Children.Add(backPath);

            return backPath;
        }

        private void StepPathOnMouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;

            Path stepPath = sender as Path;
            if (stepPath == null)
            {
                ExceptionLogger.WriteToLogfile("No stepPath was found - stepPath was null", "AesGcmOverviewPage: StepPathOnMouseLeave");
                return;
            }

            Path backPath;
            _backPaths.TryGetValue(stepPath.Name, out backPath);
            if (backPath != null)
            {
                backPath.Fill = Application.Current.FindResource("NoBackAreaBrush") as SolidColorBrush;
            }
        }

        private void StepPathOnMouseDown(object sender, MouseEventArgs e)
        {
            _mouseDownPath = sender as Path;
        }

        private void StepPathOnMouseUp(object sender, MouseEventArgs e)
        {
            Path stepPath = sender as Path;

            if (stepPath == null)
            {
                ExceptionLogger.WriteToLogfile("No stepPath was found - stepPath was null", "AesGcmOverviewPage: StepPathOnMouseUp");
            }
            else if (stepPath.Equals(_mouseDownPath))
            {
                string pathName = stepPath.Name;

                //Example Path-Name: Step11_overview
                if (pathName != null)
                {
                    int lastCharToRemove = pathName.IndexOf("_", StringComparison.Ordinal);
                    string stepNumber = pathName.Substring(4, lastCharToRemove - 4);
                    int step = int.Parse(stepNumber);

                    NavigationService?.Navigate(new StepByStepPage(step));
                }
                NavigationService?.Navigate(new StepByStepPage());
            }
        }
        #endregion StepPath-Methods


        #region AreaPath-Methods

        private void AreaPathOnMouseEnter(object sender, MouseEventArgs e)
        {
            Path path = sender as Path;
            string text = Application.Current.FindResource(path?.Name + "Text") as string;
            if (text != null)
            {
                ShowAreaExplanation(text);
            }
            else
            {
                ExceptionLogger.WriteToLogfile("No areaPath - text was found - text was null", "AesGcmOverviewPage: AreaPathOnMouseEnter");
            }
        }

        private Path FindAreaPath(Path frontPath)
        {
            Geometry stepSurface = frontPath.Data;
            foreach (Path area in _areaPaths)
            {
                Geometry areaSurface = area.Data;
                IntersectionDetail detail = stepSurface.FillContainsWithDetail(areaSurface);

                if (detail == IntersectionDetail.FullyContains
                    || detail == IntersectionDetail.FullyInside
                    || detail == IntersectionDetail.Intersects)
                {
                    return area;
                }
            }
            return null;
        }

        private void ShowAreaExplanation(string text)
        {
            OverviewTextBlock.Text = text + "\n<<Klick auf das Feld für weitere Infos>>";
            if (!OverviewTextBlock.ClipToBounds)
            {
                TextScrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility)Visibility.Hidden;
            }
        }
        #endregion AreaPath-Method

        #endregion Private Methods
    }
}
