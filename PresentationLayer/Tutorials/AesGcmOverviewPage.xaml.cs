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
            InitCanvas(TestCanvas);
        }
        #endregion

        #region Private Members
        private readonly ToolTip _toolTip = new ToolTip();
        private readonly Dictionary<string, Rectangle> _backImages = new Dictionary<string, Rectangle>();
        private const int NumOfRectangles = 2;
        private const int NumOfPaths = 1;

        #endregion Private Members

        #region Private Methods
        private void InitCanvas(Canvas canvas)
        {
            LoadPaths(canvas);
            LoadRectangles(canvas);
            LoadBackground(canvas);
        }

        private void LoadPaths(Canvas canvas)
        {
            for (int i = 1; i <= NumOfPaths; i++)
            {
                Path path = Application.Current.FindResource("Path" + i) as Path;

                if (path == null) continue;

                //path has an existing Parent when this Page is opened a second time
                (path.Parent as Canvas)?.Children.Remove(path);

                path.SetValue(Panel.ZIndexProperty, 2);
                path.MouseEnter += OnMouseEnter;
                path.MouseLeave += OnMouseLeave;

                canvas.Children.Add(path);
            }
        }

        private void LoadRectangles(Canvas canvas)
        {
            for (int i = 1; i <= NumOfRectangles; i++)
            {
                Rectangle rectangle = Application.Current.FindResource("Rect" + i) as Rectangle;

                if (rectangle == null) continue;

                //rectangle has an existing Parent when this Page is opened a second time
                (rectangle.Parent as Canvas)?.Children.Remove(rectangle);

                rectangle.SetValue(Panel.ZIndexProperty, 2);
                rectangle.MouseEnter += RectOnMouseEnter;
                rectangle.MouseLeave += RectOnMouseLeave;

                canvas.Children.Add(rectangle);
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

        private void RectOnMouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle frontRectangle = sender as Rectangle;
            Rectangle backRectangle;

            if (frontRectangle == null) return;

            //When MouseOver the second time, the backRectangle already exists
            if (!_backImages.ContainsKey(frontRectangle.Name))
            {
                backRectangle = new Rectangle
                {
                    Margin = frontRectangle.Margin,
                    Height = frontRectangle.Height,
                    Width = frontRectangle.Width
                };

                backRectangle.SetValue(Panel.ZIndexProperty, 0);
                _backImages.Add(frontRectangle.Name, backRectangle);
                TestCanvas.Children.Add(backRectangle);
            } else
            {
                backRectangle = _backImages[frontRectangle.Name];
            }

            backRectangle.Fill = Application.Current.FindResource("BackAreaBrush") as SolidColorBrush;
        }

        private void RectOnMouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle frontImage = sender as Rectangle;
            if (frontImage == null) return;

            Rectangle backImage = _backImages[frontImage.Name];
            backImage.Fill = Application.Current.FindResource("NoBackAreaBrush") as SolidColorBrush;
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            Path pathName = sender as Path;
            string text = Application.Current.FindResource(pathName?.Name + "Text") as string;
            ChangeToolTip(text);
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            _toolTip.IsOpen = false;
        }

        private void ChangeToolTip(string tooltipText)
        {
            _toolTip.Content = tooltipText;
            _toolTip.IsOpen = true;
            _toolTip.StaysOpen = true;
        }
        #endregion Private Methods
    }
}
