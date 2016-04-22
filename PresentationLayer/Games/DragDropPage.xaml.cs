using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;



namespace EP_HSRlearnIT.PresentationLayer.Games
{
    public partial class DragDropPage
    {
        private const string settingsName = "DragDropPage_Settings";
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

        private int originalNumberOfChildren;

        public DragDropPage()
        {
            InitializeComponent();
            SetImages(GetImages());

            originalNumberOfChildren = ControlGrid.Children.Count;

            _addedImages = (List<SavedData>)Progress.GetProgress(settingsName);
            if (_addedImages == null)
            {
                _addedImages = new List<SavedData>();
            }

            for (int i = 0; i < _addedImages.Count; i++)
            {
                int j = _addedImages[i].ImageIndex;
                Image origImage = (Image)ControlGrid.Children[j];
                Image newImage = new Image
                {
                    Width = 80,
                    Height = 35,
                    Margin = _addedImages[i].ImageMargin,
                    Source = origImage.Source,
                    Name = "tmp" + i.ToString()
                };

                newImage.PreviewMouseLeftButtonDown += image_PreviewMouseLeftButtonDown;
                newImage.PreviewMouseLeftButtonUp += image_PreviewMouseLeftButtonUp;
                newImage.MouseMove += image_MouseMove;

                ControlGrid.Children.Add(newImage);
            }
        }


        private readonly List<SavedData> _addedImages;

        private BitmapImage[] GetImages()
        {
            BitmapImage[] images = new BitmapImage[9];

            for (int i = 1; i < 10; i++)
            {
                BitmapImage image = new BitmapImage(new Uri(@"/Images/dragdrop" + i + ".png", UriKind.RelativeOrAbsolute));
                images[i - 1] = image;
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

                image.Margin = new Thickness(600, -450 + i * 100, image.Width, image.Height);
                image.PreviewMouseLeftButtonDown += image_PreviewMouseLeftButtonDown;
                image.MouseLeftButtonUp += image_PreviewMouseLeftButtonUp;
                image.MouseMove += image_MouseMove;
                Panel.SetZIndex(image, 2);

                ControlGrid.Children.Add(image);
                i++;
            }
        }

        private bool _isMoving = false;
        private Image _imgMoved;
        private SavedData _addedData;
        private bool copyImage = false;

        // Position used for calculating mouse move
        private Point _previousMousePosition;

        private readonly Rect[] _dropLocations =
        {new Rect(-728, -164, 0, 0),
            new Rect(-531, -348, 60, 35),
            new Rect(-531, -261,60, 35),
            new Rect(-732, -166, 60, 35),
            new Rect(-346, -95, 60, 35),
            new Rect(-379, 131, 60, 35),
            new Rect(-379, 220, 60, 35),
            new Rect(-142, -262,60, 35),
            new Rect(-142, -166, 60, 35),
            new Rect(-140, 129, 60, 35),
            new Rect(-137, -14,60, 35),
            new Rect(-150, 126, 60, 35),
            new Rect(-331, -98, 60, 35),
            new Rect(104, -106, 60, 35),
            new Rect(104, 212, 60, 35),
            new Rect(96, 383, 60, 35),
            new Rect(292, -261, 60, 35),
            new Rect(292, -166, 60, 35),
            new Rect(289, -29, 60, 35),
            new Rect(289, 107,60, 35),
            new Rect(289, 298, 60, 35),
            new Rect(289, 458, 60, 35),
        };



        private void image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //First it is checked which image is moved, then a new image is created  and set with height, width and margin, so it appears on the same position as the _imgMoved.
                _imgMoved = sender as Image;
                if (_imgMoved != null)
                {
                    copyImage = !_imgMoved.Name.Contains("tmp"); // If it doesn't contain tmp, that means it's already copied, so I need to negate it



                    if (copyImage)
                    {
                        _addedData = new SavedData();

                        Image finalImage = new Image
                        {
                            Width = 80,
                            Height = 35,
                            Margin = _imgMoved.Margin,
                            Source = _imgMoved.Source,
                            Name = "tmp" + _imgMoved.Name
                        };

                        ControlGrid.Children.Add(finalImage);
                        _addedData.ImageIndex = ControlGrid.Children.IndexOf(_imgMoved);

                        //Set Eventhandler for new image so new image behaves as original
                        finalImage.PreviewMouseLeftButtonDown += image_PreviewMouseLeftButtonDown;
                        finalImage.PreviewMouseLeftButtonUp += image_PreviewMouseLeftButtonUp;
                        finalImage.MouseMove += image_MouseMove;

                        _imgMoved = finalImage;
                    }
                    else
                    {
                        int childIndex = ControlGrid.Children.IndexOf(_imgMoved);
                        _addedData = _addedImages[childIndex - originalNumberOfChildren];
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
                        _addedData.ImageMargin = _imgMoved.Margin;
                        if (copyImage)
                            _addedImages.Add(_addedData);

                        Progress.SaveProgress(settingsName, _addedImages);
                        break;
                    }
                }
                if (!bIntersection)
                {
                    if (copyImage)
                        ControlGrid.Children.Remove(_imgMoved);
                    else
                    {
                        _imgMoved.Margin = _addedData.ImageMargin;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "image_PreviewMouseLeftButtonUp");
            }
        }

        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_isMoving)
                {
                    Point currMousePoint = e.GetPosition(this);

                    this._imgMoved.Cursor = Cursors.No;

                    //Point topLeft = ControlGrid.TranslatePoint(new Point(0, 0), ControlGrid);
                    //Point rightBottom = new Point(topLeft.X + ControlGrid.ActualWidth, topLeft.Y + ControlGrid.ActualHeight);
                    //if (currMousePoint.X > ControlGrid.ActualWidth - 120 || currMousePoint.Y > ControlGrid.ActualHeight - 100) { return; }
                    //if (currMousePoint.X < topLeft.X + 50 || currMousePoint.Y < topLeft.Y) { return;}


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
                        {
                            this._imgMoved.Cursor = Cursors.Arrow;

                            bIntersection = true;
                        }

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
                for (int i = 0; i < _addedImages.Count; i++)
                {
                    ControlGrid.Children.RemoveAt(ControlGrid.Children.Count - 1);
                }

                _addedImages.Clear();
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "OnResetClick");
            }
        }
    }
}
