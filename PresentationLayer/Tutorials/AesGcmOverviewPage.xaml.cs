using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

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

        #endregion


        #region Protected Methods

        protected override void AreaPath_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Path path = sender as Path;
            string text = Application.Current.FindResource(path?.Name + "Text") as string;
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
    }
}
