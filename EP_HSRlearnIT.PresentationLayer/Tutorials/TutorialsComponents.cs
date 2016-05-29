using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using EP_HSRlearnIT.BusinessLayer.Persistence;
using EP_HSRlearnIT.BusinessLayer.Extensions;

namespace EP_HSRlearnIT.PresentationLayer.Tutorials
{
    /// <summary>
    /// Contains all common Methods of Tutorial-Pages
    /// </summary>
    public class TutorialsComponents : Page
    {
        #region Private Members
        private readonly Dictionary<string, Path> _highlightedPaths = new Dictionary<string, Path>();
        private Path _mouseDownPath;

        #endregion

        #region Protected Members
        protected const int NumOfStepPaths = 24;

        #endregion

        #region Protected Methods
        protected void LoadBackground(Canvas canvas, string imageName)
        {
            Image background = Application.Current.TryFindResource(imageName) as Image;
            if (background == null)
            {
                ExceptionLogger.WriteToLogfile("LoadBackground", "Background was not found.", "Error from class TutorialsComponents");
                return;
            }

            //Image has an existing Parent when this Page is opened a second time
            if (background.Parent is Canvas)
            {
                ((Canvas)background.Parent).Children.Remove(background);
            }

            background.SetValue(Panel.ZIndexProperty, 1);

            canvas.Children.Add(background);
        }

        protected void LoadStepPaths(Canvas canvas)
        {
            for (int i = 1; i <= NumOfStepPaths; i++)
            {
                Path ressourcePath = Application.Current.TryFindResource("StepPath" + i) as Path;
                if (ressourcePath == null)
                {
                    ExceptionLogger.WriteToLogfile("LoadStepPahts", "ressourcePath was null", "");
                    continue;
                }
                if (!ressourcePath.Name.Contains("_overview"))
                {
                    continue;
                }

                //Create a copy of the Ressource StepPath to prevent multiple Event Listener on MouseEnter / MouseLeave
                Path stepPath = ressourcePath.Clone() as Path;
                if (stepPath == null)
                {
                    ExceptionLogger.WriteToLogfile("LoadStepPaths", "stepPath - Clone was null", "");
                    continue;
                }

                stepPath.SetValue(Panel.ZIndexProperty, 3);
                stepPath.MouseEnter += StepPath_OnMouseEnter;
                stepPath.MouseLeave += StepPath_OnMouseLeave;
                //Mouse Up and Down - Events to make it feel lika a click. There is no Click-Event for Paths.
                stepPath.MouseDown += StepPath_OnMouseDown;
                stepPath.MouseUp += StepPath_OnMouseUp;

                canvas.Children.Add(stepPath);
            }
        }

        protected virtual void ShowExplanation(Path stepPath, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods
        private void StepPath_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;

            Path stepPath = sender as Path;
            if (stepPath == null)
            {
                ExceptionLogger.WriteToLogfile("StepPath_OnMouseEnter", "stepPath was null", "");
                return;
            }

            Path highlightedPath;
            _highlightedPaths.TryGetValue(stepPath.Name, out highlightedPath);
            //When the Mouse enters a second time, the highlightedPath already exists
            if (highlightedPath == null)
            {
                highlightedPath = AddHighlightedPath(stepPath);
                if (highlightedPath == null)
                {
                    ExceptionLogger.WriteToLogfile("StepPath_OnMouseEnter", "highlightedPath was null", "");
                    return;
                }
            }

            highlightedPath.Fill = Application.Current.FindResource("BackAreaBrush") as SolidColorBrush;

            var parentPage = stepPath.GetParentPage() as Page;
            if (parentPage == null || parentPage.Title != "AesGcmOverviewPage") return;
            try
            {
                ShowExplanation(stepPath, e);
            }
            catch (NotImplementedException exception)
            {
                ExceptionLogger.WriteToLogfile("StepPath_OnMouseEnter", exception.Message, exception.StackTrace);
            }
        }

        private void StepPath_OnMouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;

            Path stepPath = sender as Path;
            if (stepPath == null)
            {
                ExceptionLogger.WriteToLogfile("StepPath_OnMouseLeave", "stepPath was null", "");
                return;
            }

            Path highlightedPath;
            _highlightedPaths.TryGetValue(stepPath.Name, out highlightedPath);
            if (highlightedPath != null)
            {
                highlightedPath.Fill = Application.Current.FindResource("NoBackAreaBrush") as SolidColorBrush;
            }
        }

        private void StepPath_OnMouseDown(object sender, MouseEventArgs e)
        {
            _mouseDownPath = sender as Path;
        }

        private void StepPath_OnMouseUp(object sender, MouseEventArgs e)
        {
            Path stepPath = sender as Path;

            if (stepPath == null)
            {
                ExceptionLogger.WriteToLogfile("StepPath_OnMouseUp", "stepPath was null", "");
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

        /// <summary>
        /// Method to highlight the background of a step within the AES-GCM - Overview Image
        /// </summary>
        /// <param name="stepPath">StepPath to highlight</param>
        /// <returns></returns>
        private Path AddHighlightedPath(Path stepPath)
        {
            Path highlightedPath = stepPath.Clone() as Path;
            if (highlightedPath == null)
            {
                ExceptionLogger.WriteToLogfile("AddHighlightedPath", "highlightedPath - Clone was null", "");
                return null;
            }

            highlightedPath.SetValue(Panel.ZIndexProperty, 0);
            _highlightedPaths.Add(stepPath.Name, highlightedPath);

            //Add highlightedPath to the OverviewCanvas
            (stepPath.Parent as Canvas)?.Children.Add(highlightedPath);

            return highlightedPath;
        }

        #endregion Private Methods
    }
}
