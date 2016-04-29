using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.PresentationLayer.Games
{
    public partial class DragDrop3
    {
        private const string SettingsName = "DragDropPage_Settings";
        private static readonly Dictionary<int, List<string>> CorrectAnswers = new Dictionary<int, List<string>>();
        private List<SavedDataForProgress> _addedSavedData;
        private readonly Rectangle[] _dropLocationsRectangles = new Rectangle[16];
        private readonly int _originalNumberOfChildren;
        private SavedDataForProgress _addedDataForProgress;
        private bool _copyRectangle;
        private bool _isMoving;
        private Rectangle _rectangleMoved;
        private Point _startPoint = new Point(0, 0);

        public DragDrop3()
        {
            InitializeComponent();
            GenerateSideImages();

            _originalNumberOfChildren = ElementCanvas.Children.Count;

            var index = 0;
            foreach (var child in ElementCanvas.Children)
            {
                if (child is Rectangle)
                {
                    var childRect = (Rectangle) child;
                    if (!childRect.Name.Contains("Rect")) continue;
                    _dropLocationsRectangles[index] = childRect;
                    index++;
                }
            }

            LoadSettings();
        }
        /// <summary>
        /// This method loads the the data saved in SavedDataForProgress to get back the same state as before a menu change./// </summary>
        private void LoadSettings()
        {
            _addedSavedData = (List<SavedDataForProgress>)Progress.GetProgress(SettingsName) ?? new List<SavedDataForProgress>();

            for (var i = 0; i < _addedSavedData.Count; i++)
            {
                var j = _addedSavedData[i].OriginalImageChildIndex;
                var origImage = (Rectangle)ElementCanvas.Children[j];
                var newImage = new Rectangle
                {
                    Width = 92,
                    Height = 43,
                    Margin = _addedSavedData[i].ImageMargin,
                    Fill = origImage.Fill,
                    Name = "tmp" + i
                };

                _addedSavedData[i].ChildReference = newImage;
                _addedSavedData[i].DropPosition = _dropLocationsRectangles[_addedSavedData[i].DropRectangleIndex];

                newImage.PreviewMouseLeftButtonDown += rectangle_PreviewMouseLeftButtonDown;
                newImage.PreviewMouseLeftButtonUp += image_PreviewMouseLeftButtonUp;
                ElementCanvas.Children.Add(newImage);
            }
            var bShowGameInstruction = (bool?)Progress.GetProgress("ShowGameInstruction");

            if (bShowGameInstruction == null)
            {
                Progress.SaveProgress("ShowGameInstruction", true);
            }
            else
            {
                GameInstruction.Visibility = Visibility.Hidden;
                ButtonCloseGameInstruction.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        /// This method loads the images from the image-folder. These are the rectangle images to be moved. /// </summary>
        private BitmapImage[] GetImages()
        {
            var images = new BitmapImage[9];

            for (var i = 0; i < 9; i++)
            {
                var image =
                    new BitmapImage(new Uri(@"pack://application:,,,/Images/dragdrop" + (i+1).ToString() + ".png",
                        UriKind.RelativeOrAbsolute));
                images[i] = image;
            }

            return images;
        }
        /// <summary>
       /// This method iterates through the images and places them in the canvas. In order to map these images to a droppable rectangle the list localCorrectAnswers is used. /// </summary>
        private void GenerateSideImages()
        {
            var images = GetImages();
            CorrectAnswers.Clear();
            List<List<string>> localCorrectAnswers = new List<List<string>>();
            localCorrectAnswers.Add(new List<string> {"Rect13Hash"});
            localCorrectAnswers.Add(new List<string> {"Rect1Iv"});
            localCorrectAnswers.Add(new List<string> {"Rect7Counter", "Rect15Counter", "Rect17Counter"});
            localCorrectAnswers.Add(new List<string> {"Rect2MultH", "Rect5MultH", "Rect9MultH", "Rect10MultH"});
            localCorrectAnswers.Add(new List<string> {"Rect4AD"});
            localCorrectAnswers.Add(new List<string> {"Rect16Tag"});
            localCorrectAnswers.Add(new List<string> {"Rect3len"});
            localCorrectAnswers.Add(new List<string> {"Rect6Ciphertext", "Rect11Ciphertext"});
            localCorrectAnswers.Add(new List<string> {"Rect8Plaintext", "Rect12Plaintext"});

            for (var i = 0; i < images.Length; i++)
            {
                var img = images[i];
                var rect = new Rectangle
                {
                    Fill = new ImageBrush(img),
                    Height = 43,
                    Width = 92,
                    Name = "dragdrop" + i,
                    Margin = new Thickness(800, 100 + i*55, 0, 0)
                };

                rect.PreviewMouseLeftButtonDown += rectangle_PreviewMouseLeftButtonDown;
                rect.MouseLeftButtonUp += image_PreviewMouseLeftButtonUp;

                var index = ElementCanvas.Children.Add(rect);
                CorrectAnswers.Add(index, localCorrectAnswers[i]);
            }
        }
        /// <summary>
        /// This method defines the functions for the PreviewMouseLeftButtonDown Event. It checks if the mouse was was pressed down on an original rectangle image
        /// or an already copied image. The moved rectangle is also saved into the progress and added as a Child to the ElementCanvas</summary>
        /// <param name="sender">This is the rectangle element which raised the event. In this method this is the _rectangleMoved</param>
        /// <param name="e">This object contains useful information in order to get the position of the mouse cursor</param>
        private void rectangle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                _rectangleMoved = sender as Rectangle;

                //Check if the element has been already moved or not. If it has been moved it would contain tmp in its name.
                if (_rectangleMoved != null)
                {
                    _startPoint = e.GetPosition(_rectangleMoved);
                    _copyRectangle = !_rectangleMoved.Name.Contains("tmp");

                    if (_copyRectangle)
                    {
                        _addedDataForProgress = new SavedDataForProgress();

                        var copiedRectangle = new Rectangle
                        {
                            Width = _rectangleMoved.Width,
                            Height = _rectangleMoved.Height,
                            Margin = _rectangleMoved.Margin,
                            Fill = _rectangleMoved.Fill,
                            Name = "tmp" + _rectangleMoved.Name
                        };

                        ElementCanvas.Children.Add(copiedRectangle);

                        //Index and ChildReference of copied rectangle is added to SavaData. 
                        _addedDataForProgress.OriginalImageChildIndex = ElementCanvas.Children.IndexOf(_rectangleMoved);
                        _addedDataForProgress.ChildReference = copiedRectangle;

                        //Set Eventhandler for new rectangle, so the copyied rectangle behaves as original
                        copiedRectangle.PreviewMouseLeftButtonDown += rectangle_PreviewMouseLeftButtonDown;
                        copiedRectangle.PreviewMouseLeftButtonUp += image_PreviewMouseLeftButtonUp;
                        copiedRectangle.MouseMove += ElementCanvas_MouseMove;

                        _rectangleMoved = copiedRectangle;
                    }
                    else
                    {
                        //refers to a rectangle which is moved within the fields of the algorithm. Index is saved to SaveData
                        var childIndex = ElementCanvas.Children.IndexOf(_rectangleMoved);
                        int addedSavedDataIndex = childIndex - _originalNumberOfChildren;
                        _addedDataForProgress = _addedSavedData[addedSavedDataIndex];
                    }
                }
                _isMoving = true;
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "image_PreviewMouseLeftButtonDown");
            }
        }
        /// <summary>
        /// This method checks if there is an intersection with a dropRectangle</summary>
        private Rectangle CheckCollisionWithDropRectangles(Rect imageRect)
        {
            foreach (var dropRect in _dropLocationsRectangles)
            {
                Rect r = new Rect(Canvas.GetLeft(dropRect), Canvas.GetTop(dropRect), dropRect.Width, dropRect.Height);
                if (imageRect.IntersectsWith(r))
                    return dropRect;
            }

            return null;
        }
        /// <summary>
        /// This method checks if there is an intersection with the RecycleBin</summary>
        /// <returns>if intersection then true otherwise false</returns>
        private bool CheckCollisionWithRecycleBin(Rect imageRect)
        {
            var recycleRect = new Rect(Canvas.GetLeft(Recycle), Canvas.GetTop(Recycle), Recycle.Width, Recycle.Height);
            return imageRect.IntersectsWith(recycleRect);
        }
        /// <summary>
        /// This method defines the functions for the PreviewMouseLeftButtonup Event. If there is an intersection between an dropRectangle,respectively recycle bin
        /// and the copied/moved image rectangle the image rectangle is dropped and the Margin of the _rectangleMoved is saved into the SaveDataForProgress.  </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void image_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                _isMoving = false;
                var imageRect = new Rect(_rectangleMoved.Margin.Left, _rectangleMoved.Margin.Top, _rectangleMoved.Width, _rectangleMoved.Height);
                // bIntersection = false;

                var dropRectangle = CheckCollisionWithDropRectangles(imageRect);
                if (dropRectangle != null) // The collision happened
                {
                    Rect r = new Rect(Canvas.GetLeft(dropRectangle), Canvas.GetTop(dropRectangle), dropRectangle.Width, dropRectangle.Height);
                    _rectangleMoved.Margin = new Thickness(r.Left, r.Top, 0, 0);
                    _addedDataForProgress.ImageMargin = _rectangleMoved.Margin;

                    var indexFound = -1;
                    for (var i = 0; i < _addedSavedData.Count; i++)
                    {
                        if (_addedSavedData[i].DropPosition != dropRectangle) continue;
                        indexFound = i;
                        break;
                    }
                    _addedDataForProgress.DropPosition = dropRectangle;
                    if (indexFound != -1) // If we are replacing already existing dropped image
                    {
                        if (_addedSavedData[indexFound].ChildReference != _rectangleMoved) 
                        {
                            ElementCanvas.Children.Remove(_addedSavedData[indexFound].ChildReference);
                            _addedSavedData.RemoveAt(indexFound);
                        }
                    }
                    if (_copyRectangle) 
                    {
                        _addedSavedData.Add(_addedDataForProgress);
                        if (_addedSavedData.Count == 16)
                            Check.IsEnabled = true;
                    }
                    for (int i = 0; i < _dropLocationsRectangles.Length; i++)
                    {
                        if (_addedDataForProgress.DropPosition == _dropLocationsRectangles[i])
                        {
                            _addedDataForProgress.DropRectangleIndex = i;
                            break;
                        }
                    }
                   
                    Progress.SaveProgress(SettingsName, _addedSavedData);
                }
                else
                {
                    if (_copyRectangle)
                        ElementCanvas.Children.Remove(_rectangleMoved);
                    else
                    {
                        _rectangleMoved.Margin = _addedDataForProgress.ImageMargin;
                    }
                }


                if (CheckCollisionWithRecycleBin(imageRect))
                {
                    ElementCanvas.Children.Remove(_rectangleMoved);
                    _addedSavedData.Remove(_addedDataForProgress);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "DragDropPage_MouseLeftButtonUp");
                MessageBox.Show("Exception :" + ex.Message); //for debugging
            }
        }
        /// <summary>
        /// This method defined all the actions during the MouseMove, as change of mouse curser and setting boundries. </summary>  
        private void ElementCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_isMoving)
                {
                    var currMousePoint = e.GetPosition(ElementCanvas);

                    _rectangleMoved.Margin = new Thickness(currMousePoint.X - _startPoint.X,
                        currMousePoint.Y - _startPoint.Y, 0, 0);

                    #region Calculate Offset

                    if (_rectangleMoved.Margin.Left < -42) // Left
                        _rectangleMoved.Margin = new Thickness(-42, _rectangleMoved.Margin.Top, 0, 0);

                    if (_rectangleMoved.Margin.Left + _rectangleMoved.Width > ElementCanvas.Width) // Right
                        _rectangleMoved.Margin = new Thickness(ElementCanvas.Width - _rectangleMoved.Width,
                            _rectangleMoved.Margin.Top, 0, 0);

                    if (_rectangleMoved.Margin.Top < 0) // Top
                        _rectangleMoved.Margin = new Thickness(0, _rectangleMoved.Margin.Top, 0, 0);

                    if (_rectangleMoved.Margin.Top + _rectangleMoved.Height > ElementCanvas.Height)
                        _rectangleMoved.Margin = new Thickness(ElementCanvas.Height - _rectangleMoved.Height,
                            _rectangleMoved.Margin.Top, 0, 0);

                    #endregion Calculate Offset

                    var imageRect = new Rect(_rectangleMoved.Margin.Left, _rectangleMoved.Margin.Top,
                        _rectangleMoved.Width, _rectangleMoved.Height);
                    bool bIntersection = CheckCollisionWithDropRectangles(imageRect) != null || CheckCollisionWithRecycleBin(imageRect);

                    _rectangleMoved.Cursor = bIntersection ? Cursors.Arrow : Cursors.No;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "testMethod");
                MessageBox.Show("Exception :" + ex.Message, "MOUSUP"); // For Debugging

            }
        }
        /// <summary>
        /// This method gets back the intial state of the game </summary> 
        private void OnResetClick(object sender, RoutedEventArgs e)
        {
            try
            {
                for (var i = 0; i < _addedSavedData.Count; i++)
                {
                    ElementCanvas.Children.RemoveAt(ElementCanvas.Children.Count - 1);
                }

                _addedSavedData.Clear();
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "OnResetClick");
            }
        }
        /// <summary>
        /// This methods checks the dropped elements for their correctness and gives back the amount of correct / wrong placements. Wrong dropped elements will be removed. </summary>
        private void Check_OnClick(object sender, RoutedEventArgs e)
        {
           var correctAnswers = 0;
           var wrongAnswers = 0;

            List<int> indicesToRemove = new List<int>();
            for(int i = _addedSavedData.Count-1; i>= 0; i--)
           // (int i = 0; i < _addedSavedData.Count; i++)
           {
                var data = _addedSavedData[i];
                if (data.AnswerCorrect())
                    correctAnswers++;
                else
                {
                    indicesToRemove.Add(i);
                    wrongAnswers++;
                }
            }

            foreach (int i in indicesToRemove)
            {
                _addedSavedData.RemoveAt(i);
                ElementCanvas.Children.RemoveAt(_originalNumberOfChildren + i);
            }

            MessageBox.Show(
                $"Spiel ist nun beendet. Korrekte Antwort: {correctAnswers}, Falsche Anworten: {wrongAnswers}");
        }

        private void CloseGameInstruction(object sender, RoutedEventArgs e)
        {
            GameInstruction.Visibility = Visibility.Hidden;
            var lbl = (Button) LogicalTreeHelper.FindLogicalNode(ElementCanvas, "ButtonCloseGameInstruction");
            if (lbl != null) lbl.Visibility = Visibility.Hidden;
        }
        #region SavedDataForProgress
        /// <summary>
        /// This class is needed for the progress saving. In order to restore the progress, the index,maring and dropPosition are saved.
        /// </summary>
        private class SavedDataForProgress
        {
            public Rectangle DropPosition; // Drop rectangle reference, there should be unique drop rectangles in whole _addedSavedData
            public Rectangle ChildReference;
      
            public Thickness ImageMargin; // Position of copied data, we need it when switching pages
            public int OriginalImageChildIndex; // Index of original child we made copy of, it's index of imagebrush
            public int DropRectangleIndex; // Index of original child we made copy of, it's index of imagebrush

            public SavedDataForProgress()
            {
                OriginalImageChildIndex = -1;
                DropPosition = null;
            }

           /* public SavedDataForProgress(int originalImageChildIndex, Thickness ImageMargin, Rectangle dropPosition)
            {
                OriginalImageChildIndex = originalImageChildIndex;
                this.ImageMargin = ImageMargin;
                DropPosition = dropPosition;
            }*/

            public bool AnswerCorrect()
            {
                if (CorrectAnswers[OriginalImageChildIndex].Contains(DropPosition.Name))
                {
                    return true;
                }
                return false;
            }
        }

        #endregion SavedDataForProgress
    }
}