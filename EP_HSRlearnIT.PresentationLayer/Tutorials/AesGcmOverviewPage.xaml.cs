using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using EP_HSRlearnIT.BusinessLayer.Extensions;
using EP_HSRlearnIT.BusinessLayer.Persistence;

namespace EP_HSRlearnIT.PresentationLayer.Tutorials
{
    /// <summary>
    /// Page to briefly inform about AES-GCM.
    /// </summary>
    public partial class AesGcmOverviewPage
    {
        #region Private Members
        private readonly Path[] _areaPaths = new Path[NumOfAreas];
        protected const int NumOfAreas = 6;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor to initialize the XAML and the Content.
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
            LoadBackground(canvas, "BackgroundImage");
        }

        private void ShowAreaExplanation(string text)
        {
            OverviewTextBlock.Text = text + $"{Environment.NewLine}<<Klick auf das Feld für weitere Infos>>";
            if (!OverviewTextBlock.ClipToBounds)
            {
                TextScrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility)Visibility.Hidden;
            }
        }

        private Path FindAreaPath(Path stePath)
        {
            Geometry stepSurface = stePath.Data;
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

        private void LoadAreaPaths(Canvas canvas)
        {
            for (int i = 1; i <= NumOfAreas; i++)
            {
                Path ressourcePath = Application.Current.TryFindResource("AreaPath" + i) as Path;
                if (ressourcePath == null)
                {
                    ExceptionLogger.WriteToLogfile("LoadAreaPahts", "ressourcePath was null", "");
                    continue;
                }

                //Create a copy of the Ressource AreaPath to prevent multiple Event Listener on MouseEnter
                Path areaPath = ressourcePath.Clone() as Path;
                if (areaPath == null)
                {
                    ExceptionLogger.WriteToLogfile("LoadAreaPaths", "areaPath - Clone was null", "");
                    return;
                }

                areaPath.SetValue(Panel.ZIndexProperty, 2);
                if (canvas.FindName("Overview") != null)
                {
                    try
                    {
                        areaPath.MouseEnter += AreaPath_OnMouseEnter;
                    }
                    catch (Exception ex)
                    {
                        ExceptionLogger.WriteToLogfile("AreaPath_OnMouseEnter", ex.Message, ex.StackTrace);
                    }
                }

                canvas.Children.Add(areaPath);
                _areaPaths[i - 1] = areaPath;
            }
        }

        private void AreaPath_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Path path = sender as Path;
            string text = Application.Current.TryFindResource(path?.Name + "Text") as string;
            if (text != null)
            {
                ShowAreaExplanation(text);
            }
            else
            {
                ExceptionLogger.WriteToLogfile("AreaPathOnMouseEnter", "Text was not found.", "");
            }
        }

        #endregion


        #region Protected Methods
        protected override void ShowExplanation(Path stepPath, MouseEventArgs e)
        {
            //Find Area Path to show the explanation
            Path areaPath = FindAreaPath(stepPath);
            if (areaPath != null)
            {
                AreaPath_OnMouseEnter(areaPath, e);
            }
        }

        #endregion
    }
}
