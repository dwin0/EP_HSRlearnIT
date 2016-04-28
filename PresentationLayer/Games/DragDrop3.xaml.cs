using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EP_HSRlearnIT.PresentationLayer.Games
{
    public partial class DragDrop3 : Page
    {

        private const string settingsName = "DragDropPage_Settings";
        private readonly List<SavedData> _addedImages;
        private readonly int _originalNumberOfChildren;

        private bool _isMoving = false;
        private Rectangle _imgMoved;

        private SavedData _addedData;
        private bool _copyImage = false;

        // Position used for calculating mouse move
       private Point _previousMousePosition;

        private class SavedData
        {
            //Index and position of copied image are saved into SavedData
            public int ImageIndex;
            public Thickness ImageMargin;

            public SavedData() { ImageIndex = -1; }
            public SavedData(int ImageIndex, Thickness ImageMargin)
            {
                this.ImageIndex = ImageIndex;
                this.ImageMargin = ImageMargin;
            }
        };

        public DragDrop3()
        {
            InitializeComponent();
            SetImages(GetImages());

            _originalNumberOfChildren = ElementCanvas.Children.Count;
            int index= 0;

            foreach (var child in ElementCanvas.Children)
            {
                if (child is Rectangle)
                {
                    Rectangle childRect = (Rectangle) (child);
                    if (childRect.Name.Contains(("Rect")))
                    {
                        _dropLocationsRectangles[index] = childRect;
                        index++;
                        _dropLocationsRects.Add(new Rect(Canvas.GetLeft(childRect), Canvas.GetTop(childRect), childRect.Width, childRect.Height));
                    }
                }
            }

            _addedImages = (List<SavedData>)Progress.GetProgress(settingsName);
            if (_addedImages == null)
            {
                _addedImages = new List<SavedData>();
            }

            for (int i = 0; i < _addedImages.Count; i++)
            {
                int j = _addedImages[i].ImageIndex;
                Rectangle origImage = (Rectangle)ElementCanvas.Children[j];
                Rectangle newImage = new Rectangle
                {
                    Width = 92,
                    Height = 43,
                    Margin = _addedImages[i].ImageMargin,
                    Fill = origImage.Fill,
                    Name = "tmp" + i.ToString()
                };

                newImage.PreviewMouseLeftButtonDown += image_PreviewMouseLeftButtonDown;
                newImage.PreviewMouseLeftButtonUp += image_PreviewMouseLeftButtonUp;
              

                ElementCanvas.Children.Add(newImage);
            }
        }

        private BitmapImage[] GetImages()
        {
            BitmapImage[] images = new BitmapImage[9];

            for (int i = 1; i < 10; i++)
            {
                BitmapImage image = new BitmapImage(new Uri(@"pack://application:,,,/Images/dragdrop" + i + ".png", UriKind.RelativeOrAbsolute));
                images[i - 1] = image;
            }

            return images;
        }

        private void SetImages(BitmapImage[] images)
        {
            int i = 0;
            foreach (var img in images)
            {
                Rectangle rect = new Rectangle()
                {
                    Fill = new ImageBrush(img),
                    Height = 43,
                    Width = 92,
                    Name = "dragdrop" + i
                };

                rect.Margin = new Thickness(800, 100 + i * 55, 0, 0);
                rect.PreviewMouseLeftButtonDown += image_PreviewMouseLeftButtonDown;
                rect.MouseLeftButtonUp += image_PreviewMouseLeftButtonUp;

                ElementCanvas.Children.Add(rect);

                i++;
            }
        }

        private Point _startPoint = new Point(0, 0);

        private void image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {

                //First it is checked which image is moved. It is checked if the element has already been moved once or if Image is still on its initial placd. If it has been moved it would contain tmp in its name.
                _imgMoved = sender as Rectangle;

                if (_imgMoved != null)
                {
                    _startPoint = e.GetPosition(_imgMoved);
                    _copyImage = !_imgMoved.Name.Contains("tmp");

                    if (_copyImage)
                    {
                        _addedData = new SavedData();


                        Rectangle finalImage = new Rectangle
                        {
                            Width = _imgMoved.Width,
                            Height = _imgMoved.Height,
                            Margin = _imgMoved.Margin,
                            Fill = _imgMoved.Fill,
                            Name = "tmp" + _imgMoved.Name
                        };

                        ElementCanvas.Children.Add(finalImage);
                        _addedData.ImageIndex = ElementCanvas.Children.IndexOf(_imgMoved);

                        //Set Eventhandler for new image so new image behaves as original
                        finalImage.PreviewMouseLeftButtonDown += image_PreviewMouseLeftButtonDown;
                        finalImage.PreviewMouseLeftButtonUp += image_PreviewMouseLeftButtonUp;
                        finalImage.MouseMove += ElementCanvas_MouseMove;

                        _imgMoved = finalImage;
                    }
                    else
                    {
                        int childIndex = ElementCanvas.Children.IndexOf(_imgMoved);
                        _addedData = _addedImages[childIndex - _originalNumberOfChildren];
                    }
                }

                // Remember the initial mouse position
                _previousMousePosition = e.GetPosition(this);
                _isMoving = true;

            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "image_PreviewMouseLeftButtonDown");
            }
        }

        private readonly Rectangle[] _dropLocationsRectangles = new Rectangle[18];
        private readonly List<Rect> _dropLocationsRects = new List<Rect>();
      

        private void image_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {/*
            try
            {
                _isMoving = false;

                //Check if the original image corresponds with one of the droplocations. If there is a intersection, then image can be dropped
                Rect imageRect = new Rect(_imgMoved.Margin.Left, _imgMoved.Margin.Top, _imgMoved.Width, _imgMoved.Height);
                bool bIntersection = false;

                foreach (Rect r in _droppLocations)
                {
                    if (imageRect.IntersectsWith(r))
                    {
                        bIntersection = true;
                        _imgMoved.Margin = new Thickness(r.Left, r.Top, 0, 0);
                        _addedData.ImageMargin = _imgMoved.Margin;
                        if (_copyImage)
                            _addedImages.Add(_addedData);

                        Progress.SaveProgress(settingsName, _addedImages);
                        break;
                    }
                }
                if (!bIntersection)
                {
                    if (_copyImage)
                        ElementCanvas.Children.Remove(_imgMoved);
                    else
                    {
                        _imgMoved.Margin = _addedData.ImageMargin;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "image_PreviewMouseLeftButtonUp");
            }*/
        }


        private void OnResetClick(object sender, RoutedEventArgs e)
        {
            try
            {
                for(int i = 0; i < _addedImages.Count; i++)
                {
                    ElementCanvas.Children.RemoveAt(ElementCanvas.Children.Count - 1); 
                }

                _addedImages.Clear();
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "OnResetClick");
            }
        }

     

        private void ElementCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {

                if (_isMoving)
                {
                    Point currMousePoint = e.GetPosition(ElementCanvas);

                    _imgMoved.Margin = new Thickness(currMousePoint.X - _startPoint.X, currMousePoint.Y - _startPoint.Y, 0, 0);

                    //Set Boundries 
                    if (_imgMoved.Margin.Left < -42) // Left
                        _imgMoved.Margin = new Thickness(-42, _imgMoved.Margin.Top, 0, 0);

                    if (_imgMoved.Margin.Left + _imgMoved.Width > ElementCanvas.Width) // Right
                        _imgMoved.Margin = new Thickness(ElementCanvas.Width - _imgMoved.Width, _imgMoved.Margin.Top, 0, 0);

                    if (_imgMoved.Margin.Top < 0)  // Top
                        _imgMoved.Margin = new Thickness(0, _imgMoved.Margin.Top, 0, 0);

                    if (_imgMoved.Margin.Top + _imgMoved.Height > ElementCanvas.Height) 
                        _imgMoved.Margin = new Thickness(ElementCanvas.Height - _imgMoved.Height, _imgMoved.Margin.Top, 0, 0);

                 //This part is only for debugging (so that the textbox gets shown) and will be deleted for the final version

                    Rect imageRect = new Rect(_imgMoved.Margin.Left, _imgMoved.Margin.Top, _imgMoved.Width, _imgMoved.Height);
                    bool bIntersection = false;
                    foreach (var r in _dropLocationsRects)
                    {
                        if (imageRect.IntersectsWith(r))
                        {
                            this._imgMoved.Cursor = Cursors.Arrow;

                            bIntersection = true;
                        }

                        /*if(imageRect.IntersectsWith(new Rect(Canvas.GetLeft(Recycle), Canvas.GetTop(Recycle), imageRect.Width, imageRect.Height))
                        {
                            ElementCanvas.Children.Remove(imageRect);
                        }*/
                    }
                    if(!bIntersection)
                        this._imgMoved.Cursor = Cursors.No;
                    TextBox.Text = $"X: {(int)imageRect.Left}, Y:{(int)imageRect.Top}, W:{(int)imageRect.Width}, H:{imageRect.Height} INT: {(bIntersection ? "TRUE" : "FALSE")}";

                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "testMethod");
            }
        }


        private void DragDropPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                _isMoving = false;

                //Check if the original image corresponds with one of the droplocations. If there is a intersection, then image can be dropped
                Rect imageRect = new Rect(_imgMoved.Margin.Left, _imgMoved.Margin.Top, _imgMoved.Width, _imgMoved.Height);
                bool bIntersection = false;

                foreach (Rect r in _dropLocationsRects)
                {
                    if (imageRect.IntersectsWith(r))
                    {
                        bIntersection = true;
                        _imgMoved.Margin = new Thickness(r.Left, r.Top, 0, 0);
                        _addedData.ImageMargin = _imgMoved.Margin;
                        if (_copyImage)
                            _addedImages.Add(_addedData);

                        Progress.SaveProgress(settingsName, _addedImages);
                        break;
                    }
                }
                if (!bIntersection)
                {
                    if (_copyImage)
                        ElementCanvas.Children.Remove(_imgMoved);
                    else
                    {
                        _imgMoved.Margin = _addedData.ImageMargin;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "DragDropPage_MouseLeftButtonUp");
            }
        }
    }
}
