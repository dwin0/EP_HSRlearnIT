using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace EP_HSRlearnIT.PresentationLayer.Games
{
    public partial class DragDropPage1
    {
        #region Private Members
        private int _step = 1;
      
        #endregion
        public DragDropPage1()
        {
            InitializeComponent();
            SetImages(GetImages());
            //SortImages();

            var progress = Progress.GetProgress("CurrentStep");
            if (progress != null)
            {
                _step = Convert.ToInt32(progress);
            }

          //  ReplaceText(_step);
        }

        private BitmapImage[] GetImages()
        {
            BitmapImage[] images = new BitmapImage[9];

            for (int i = 1; i < 10; i++)
            {
                BitmapImage image = new BitmapImage(new Uri(@"/Images/dragdrop" + i + ".png", UriKind.RelativeOrAbsolute));
                images[i-1] = image;
            }

            return images;
        }

        private void SetImages(BitmapImage[] images)
        {
            int i = 0;
            foreach (var img in images)
            {
                Image image = new Image
                {
                    Source = img,
                    Height = 35,
                    Width = 80,
                    AllowDrop = true,
                    Name = "dragdrop" + i
            };

                image.Margin = new Thickness(600, -450 + i*100, image.Width, image.Height);
                image.PreviewMouseLeftButtonDown += image_PreviewMouseLeftButtonDown;
                image.MouseLeftButtonUp += image_PreviewMouseLeftButtonUp;
                image.MouseMove += image_MouseMove;
                Panel.SetZIndex(image, 2);

                ControlGrid.Children.Add(image);
                i++;
            }
        }

        /*
        private void SortImages()
        {
            int i = 0;
            Image[] images = { Hashsubkey, Iv, Counter, MultH, AuthData, AuthTag, Ciphertext, Plaintext, Len };
            foreach (var img in images)
            {
                img.Width = 80;
                img.Height = 35;
                img.Margin = new Thickness(600, -450 + i * 100, img.Width, img.Height);
                i++;
            }
        }
        */

        private readonly List<UIElement> _addedImages = new List<UIElement>();

        private bool _isMoving = false;
        private Image _imgMoved;
        //private Thickness _marginStart; //is never used

        // Position used for calculating mouse move
        private Point _previousMousePosition;

        private readonly Rect[] _dropLocations =
        {
            new Rect(-734, -166, 0, 0),
            new Rect(-537, -346, 0, 0),
            new Rect(-537, -262, 0, 0),
            new Rect(-732, -166, 0, 0),
            new Rect(-386, 126, 0, 0),
            new Rect(-386, 220, 0, 0),
            new Rect(-150, -262, 0, 0),
            new Rect(-150, -169, 0, 0),
            new Rect(-150, -30, 0, 0),
            new Rect(-150, 126, 0, 0),
            new Rect(-331, -98, 0, 0),
            new Rect(96, -100, 0, 0),
            new Rect(96, 204, 0, 0),
            new Rect(96, 383, 0, 0),
            new Rect(279, -262, 0, 0),
            new Rect(279, -169, 0, 0),
            new Rect(279, -30, 0, 0),
            new Rect(279, 106, 0, 0),
            new Rect(279, 293, 0, 0),
            new Rect(279, 450, 0, 0),
        };
        private void image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //First it is checked which image is moved, then a new image is created  and set with height, width and margin, so it appears on the same position as the _imgMoved.
                _imgMoved = sender as Image;
                if (_imgMoved != null)
                {
                    Image finalImage = new Image
                    {
                        Width = 80,
                        Height = 35,
                        Margin = _imgMoved.Margin,
                        Source = _imgMoved.Source,
                        Name = "tmp" + _imgMoved.Name
                    };
                    ControlGrid.Children.Add(finalImage);

                    //Set Eventhandler for new image so new image behaves as original 
                    finalImage.PreviewMouseLeftButtonUp += image_PreviewMouseLeftButtonUp;
                    _imgMoved = finalImage;
                }

                // Remember the initial mouse position
                _previousMousePosition = e.GetPosition(this);
                //_marginStart = _imgMoved.Margin;
                _isMoving = true;
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "image_PreviewMouseLeftButtonDown");
                //Console.WriteLine("Exception occured " + ex.Message); //wird dies benötigt?
            }
        }

        private void image_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                _isMoving = false;

                //Check if the original image corresponds with one of the droplocations. If there is a intersection, then image can be dropped
                Rect imageRect = new Rect(_imgMoved.Margin.Left, _imgMoved.Margin.Top, _imgMoved.Width, _imgMoved.Height);
                bool bIntersection = false;
                foreach (Rect r in _dropLocations)
                {
                    if (imageRect.IntersectsWith(r))
                    {
                        bIntersection = true;
                        _imgMoved.Margin = new Thickness(r.Left - 80, r.Top - 35, 0, 0);
                        _addedImages.Add(_imgMoved);
                        //  Progress.SaveProgress( );
                        break;
                    }
                }
                if (!bIntersection)
                {
                    ControlGrid.Children.Remove(_imgMoved);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "testMethod");
                //Console.WriteLine("Das Element konnte nicht korrekt zugeordnet werden. Fehlermeldung: " + ex.Message); //wird dies benötigt?
            }
        }

        private void image_MouseMove(object sender, MouseEventArgs e) 
        {
            try
            {
                if (_isMoving)
                {
                    Point currMousePoint = e.GetPosition(this);
                    double dragHorizontal = currMousePoint.X - _previousMousePosition.X;
                    double dragVertical = currMousePoint.Y - _previousMousePosition.Y;
                    _previousMousePosition = currMousePoint;
                    Thickness oldMargin = _imgMoved.Margin;
                    oldMargin.Left += dragHorizontal * 2;
                    oldMargin.Top += dragVertical * 2;
                    _imgMoved.Margin = oldMargin;

                    //This part is only for debugging (so that the textbox gets shown) and will be deleted for the final version
                    Rect imageRect = new Rect(_imgMoved.Margin.Left, _imgMoved.Margin.Top, _imgMoved.Width, _imgMoved.Height);
                    bool bIntersection = false;
                    foreach (var r in _dropLocations)
                    {
                        if (imageRect.IntersectsWith(r))

                            bIntersection = true;
                    }
                    TextBox.Text = $"X: {oldMargin.Left}, Y:{oldMargin.Top} INT: {(bIntersection ? "TRUE" : "FALSE")}";
                    
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "testMethod");
            }
        }
        private void OnResetClick(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var image in _addedImages)
                {
                    ControlGrid.Children.Remove(image);
                }

                /*
                var images = ControlGrid.Children.OfType<Image>().ToList();
                
                foreach (var image in images)
                {
                    if (image.Name.Contains("tmp"))
                    {
                        ControlGrid.Children.Remove(image);
                    }
                }
                */
                _addedImages.Clear();
                
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "testMethod");
            }
        }
    }
}
