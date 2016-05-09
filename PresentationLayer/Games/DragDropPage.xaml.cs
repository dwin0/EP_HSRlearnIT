using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.PresentationLayer.Games
{
    public partial class DragDropPage
    {
        #region Private Attributes
        private const string SettingsName = "DragDropPage_Settings";
        private static readonly Dictionary<int, List<string>> CorrectAnswers = new Dictionary<int, List<string>>();
        private readonly int _originalNumberOfChildren;

        private static int NumOfDroppableRectanglePlaces = 17;
        private List<SavedDataForProgress> _addedSavedData;
        private List<Rectangle> _dropLocationsRectangles = new List<Rectangle>(); 
        
        private SavedDataForProgress _currentlyAddedData;
        private Rectangle _currentlyMovedRectangle;
        private bool _rectangleHasBeenMovedBefore;
        private bool _isMoving;
        private Point _startPoint = new Point(0, 0);

        #endregion

        public DragDropPage()
        {
            InitializeComponent();
            GenerateSideImages();
            LoadDroppablePlaces(ElementCanvas);

            _originalNumberOfChildren = ElementCanvas.Children.Count;

            var index = 0;
            foreach (var child in ElementCanvas.Children)
            {
                if (child is Rectangle)
                {
                    var childRect = (Rectangle) child;
                    if (!childRect.Name.Contains("Rect")) continue;
                    _dropLocationsRectangles.Add(childRect);
                    index++;
                }
            }

            LoadSettings();
        }

        /// <summary>
        /// This method loads all Rectangles where a solution can be dropped.</summary>
        /// <param name="canvas">Is the Parent where the Rectangles have to be placed</param>
        private void LoadDroppablePlaces(Canvas canvas)
        {
            for (int i = 1; i <= NumOfDroppableRectanglePlaces; i++)
            {
                Rectangle droppablePlaces = Application.Current.FindResource("DroppablePlace" + i) as Rectangle;
                if (droppablePlaces == null) continue;

                Rectangle dropPlaceCopy = Clone(droppablePlaces) as Rectangle;

                if (dropPlaceCopy != null)
                {
                    canvas.Children.Add(dropPlaceCopy);
                }
            }
        }

        private FrameworkElement Clone(FrameworkElement e)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(XamlWriter.Save(e));

            return (FrameworkElement)XamlReader.Load(new XmlNodeReader(document));
        }

        /// <summary>
        /// This method loads the the data saved in SavedDataForProgress to get back the same state as before a menu change./// </summary>
        private void LoadSettings()
        {
            try
            {
                _addedSavedData = (List<SavedDataForProgress>)Progress.GetProgress(SettingsName) ?? new List<SavedDataForProgress>();

                for (var i = 0; i < _addedSavedData.Count; i++)
                {
                    var j = _addedSavedData[i].OriginalImageChildIndex;
                    var origImage = (Rectangle)ElementCanvas.Children[j];
                    var newImage = CreateNewRectangle(origImage.Fill, 92, 43, "tmp" + i, _addedSavedData[i].ImageMargin,
                        true, true, false);

                    // here we have refreshed the original child reference and dropRectangle 
                    _addedSavedData[i].ChildReference = newImage;
                    _addedSavedData[i].DropRectangle = _dropLocationsRectangles[_addedSavedData[i].DropRectangleIndex];

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
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "LoadSettings");
            }
        }

        /// <summary>
        /// This method loads the images from the image-folder. These are the rectangle images to be moved. /// </summary>
        private BitmapImage[] GetImages()
        {
            var images = new BitmapImage[9];

            for (var i = 0; i < 9; i++)
            {
                var image = new BitmapImage(new Uri(@"pack://application:,,,/Images/dragdrop" + (i+1) + ".png", UriKind.RelativeOrAbsolute));
                images[i] = image;
            }

            return images;
        }

        /// <summary>
        /// This method is creating a new Rectangle, e.g. after a menu change or when an copy of the original retangle is created. </summary>
        /// <param name="fill"></param>
        /// <param name="width">Witdh of new Rectangle to be created</param>
        /// <param name="height">Height of the new Rectangle to be created</param>
        /// <param name="name">Name of the new Rectangle to be created</param>
        /// <param name="margin">Margin of the Rectangle to be created</param>
        /// <param name="bLeftButtonDown">Rectangle should/should not be associated with the Mouse-Eventhandler LeftButtonDown</param>
        /// <param name="bLeftButtonUp">Rectange should/should not be associated with the Mouse-Eventhandler LeftbuttonUp</param>
        /// <param name="bMouseMove">Rectangle should/should not be associated with the Mouse-Eventhandler MouseMove</param>
        /// <returns>Return value is the newly created Rectangle</returns>
        Rectangle CreateNewRectangle(Brush fill, double width, double height, string name, Thickness margin, bool bLeftButtonDown, bool bLeftButtonUp, bool bMouseMove)
        {
            Rectangle resRect = new Rectangle
            {
                Fill = fill,
                Width = width,
                Height = height,
                Name = name,
                Margin = margin
            };
            if (bLeftButtonDown) resRect.PreviewMouseLeftButtonDown += rectangle_PreviewMouseLeftButtonDown;
            if (bLeftButtonUp) resRect.MouseLeftButtonUp += rectangle_PreviewMouseLeftButtonUp;
            if (bMouseMove) resRect.MouseMove += ElementCanvas_MouseMove;
            return resRect;
        }

        /// <summary>
       /// This method iterates through the images and places them in the canvas. In order to associate these images to a droppable rectangle the list localCorrectAnswers is used. /// </summary>
        private void GenerateSideImages()
        {
            try
            {
                var images = GetImages();
                CorrectAnswers.Clear();
                List<List<string>> localCorrectAnswers = new List<List<string>>();
                localCorrectAnswers.Add(new List<string> { "Rect13Hash" });
                localCorrectAnswers.Add(new List<string> { "Rect1Iv" });
                localCorrectAnswers.Add(new List<string> { "Rect7Counter", "Rect15Counter", "Rect17Counter" });
                localCorrectAnswers.Add(new List<string> { "Rect2MultH", "Rect5MultH", "Rect9MultH", "Rect10MultH" });
                localCorrectAnswers.Add(new List<string> { "Rect4Aad" });
                localCorrectAnswers.Add(new List<string> { "Rect16Tag" });
                localCorrectAnswers.Add(new List<string> { "Rect3Len" });
                localCorrectAnswers.Add(new List<string> { "Rect6Ciphertext", "Rect11Ciphertext" });
                localCorrectAnswers.Add(new List<string> { "Rect8Plaintext", "Rect12Plaintext" });

                for (var i = 0; i < images.Length; i++)
                {
                    var index = ElementCanvas.Children.Add(CreateNewRectangle(new ImageBrush(images[i]), 92, 43, "dragdrop" + i, new Thickness(800, 100 + i * 55, 0, 0), true, true, false));
                    CorrectAnswers.Add(index, localCorrectAnswers[i]);
                }
            }
    
	        catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "GenerateSideImages()");
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
                _currentlyMovedRectangle = sender as Rectangle;

                //Check if the element has been already moved or not. If it has been moved it would contain tmp in its name.
                if (_currentlyMovedRectangle != null)
                {
                    _startPoint = e.GetPosition(_currentlyMovedRectangle);
                    _rectangleHasBeenMovedBefore = _currentlyMovedRectangle.Name.Contains("tmp");

                   if (!_rectangleHasBeenMovedBefore)
                    {
                        _currentlyAddedData = new SavedDataForProgress();

                        var copiedRectangle = CreateNewRectangle(_currentlyMovedRectangle.Fill,
                            _currentlyMovedRectangle.Width, _currentlyMovedRectangle.Height,
                            "tmp" + _currentlyMovedRectangle.Name, _currentlyMovedRectangle.Margin, true, true, true);

                        ElementCanvas.Children.Add(copiedRectangle);

                        //Index and ChildReference of copied rectangle is added to SavaData. 
                        _currentlyAddedData.OriginalImageChildIndex = ElementCanvas.Children.IndexOf(_currentlyMovedRectangle);
                        _currentlyAddedData.ChildReference = copiedRectangle;

                        _currentlyMovedRectangle = copiedRectangle;
                    }
                    else
                    {
                        //refers to a rectangle which is moved within the fields of the algorithm. Index is saved to SaveData
                        var childIndexOfMovedRectangle = ElementCanvas.Children.IndexOf(_currentlyMovedRectangle);
                        int addedSavedDataIndex = childIndexOfMovedRectangle - _originalNumberOfChildren;
                        _currentlyAddedData = _addedSavedData[addedSavedDataIndex];
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
        /// This method checks if there is an intersection with a dropLocationsRectangle</summary>
        /// <return>If intersection of an rectangle with a an rectangle from DropLocation was found then the drectangle from dropLocationsRectangles is returned, otherwise nothing</return>
        private Rectangle CheckCollisionWithDropRectangles(Rect imageRect)
        {
            foreach (var dropRect in _dropLocationsRectangles)
            {
                Rect r = new Rect(Canvas.GetLeft(dropRect), Canvas.GetTop(dropRect), dropRect.Width, dropRect.Height);
                if (imageRect.IntersectsWith(r))
                {
                    return dropRect;
                }   
            }

            return null;
        }

        /// <summary>
        /// This method checks if there is an intersection with the RecycleBin</summary>
        /// <returns>if intersection then true otherwise false</returns>
        private bool CheckCollisionWithRecycleBin(Rect imageRect)
        {
            Rectangle recycleBinRectangle = Application.Current.FindResource("DroppablePlace17") as Rectangle;
            if (recycleBinRectangle != null)
            {
                var recycleRect = new Rect(Canvas.GetLeft(recycleBinRectangle), Canvas.GetTop(recycleBinRectangle), recycleBinRectangle.Width, recycleBinRectangle.Height);
                return imageRect.IntersectsWith(recycleRect);
            }
            return false;
        }

        /// <summary>
        /// Method checks if a droplocation is already filled with an image rectangle/// </summary>
        /// <param name="rect">rectangle field for which it is checked if its filled with an image rectangle or not</param>
        /// <returns>index -1 is returned if an rectangle field is still empty otherwise the index of the rectangle is returned </returns>
        private int IsRectangleFilled(Rectangle rect)
        {
            try
            {
                for (var index = 0; index < _addedSavedData.Count; index++)
                {
                    if (_addedSavedData[index].DropRectangle == rect) //intended
                        return index;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "IsRectangleFilled");
            }
                return -1;
        }

        /// <summary>
        /// This method defines the functions for the PreviewMouseLeftButtonup Event. If there is an intersection between a dropRectangle, respectively recycle bin
        /// and the copied/moved image rectangle the image rectangle is dropped and the Margin of the currentlyMovedRectangle is saved into the SaveDataForProgress.  </summary>
      
        private void rectangle_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_currentlyMovedRectangle == null)
                return;
            try
            {
                _isMoving = false;
                var rectangleInDraggedStatus = new Rect(_currentlyMovedRectangle.Margin.Left, _currentlyMovedRectangle.Margin.Top, _currentlyMovedRectangle.Width, _currentlyMovedRectangle.Height);

                var dropRectangle = CheckCollisionWithDropRectangles(rectangleInDraggedStatus); 
                if (dropRectangle != null) // The collision happened
                {
                      //TODO: Check if r is necessary
                    //dropRectangle = defined rectangle on algorithm (e.g. rectMulH). 
                    Rect r = new Rect(Canvas.GetLeft(dropRectangle), Canvas.GetTop(dropRectangle), dropRectangle.Width, dropRectangle.Height);
                    _currentlyMovedRectangle.Margin = new Thickness(r.Left, r.Top, 0, 0);
                    _currentlyAddedData.ImageMargin = _currentlyMovedRectangle.Margin;

                    var addedSavedDataIndex = IsRectangleFilled(dropRectangle);

                    _currentlyAddedData.DropRectangle = dropRectangle;
                    _currentlyAddedData.DropRectangleIndex = _dropLocationsRectangles.IndexOf(dropRectangle);

                    //TODO: Extract Method
                    if (addedSavedDataIndex != -1) // If it is not minus 1, that means we are putting the image to the already filled rectangle
                    {
                        if (_addedSavedData[addedSavedDataIndex].ChildReference != _currentlyMovedRectangle) // Do not remove rectangle itself 
                        {
                            ElementCanvas.Children.Remove(_addedSavedData[addedSavedDataIndex].ChildReference);
                            _addedSavedData.RemoveAt(addedSavedDataIndex);
                        }
                    }

                    if (!_rectangleHasBeenMovedBefore) //if we move it from the intial place to an rectangle field
                    {
                        _addedSavedData.Add(_currentlyAddedData);
                        if (_addedSavedData.Count == 16) //TODO: const 
                            Check.IsEnabled = true;
                    }

                    Progress.SaveProgress(SettingsName, _addedSavedData);
                }
                else // The collision did not happen
                {
                    if (!_rectangleHasBeenMovedBefore) // Either we get rid of the fresh copy
                        ElementCanvas.Children.Remove(_currentlyMovedRectangle);
                    else // Or we return child back to its original drop rectangle
                        _currentlyMovedRectangle.Margin = _currentlyAddedData.ImageMargin;
                }

                if (CheckCollisionWithRecycleBin(rectangleInDraggedStatus))
                {
                    ElementCanvas.Children.Remove(_currentlyMovedRectangle);
                    _addedSavedData.Remove(_currentlyAddedData);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "DragDropPage_MouseLeftButtonUp");
                MessageBox.Show("Exception :" + ex.Message); //for debugging
            }
        }

        /// <summary>
        /// This method defines all the actions during the MouseMove, as change of mouse curser and setting boundries. </summary>  
        private void ElementCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_isMoving)
                {
                    var currMousePoint = e.GetPosition(ElementCanvas);

                    _currentlyMovedRectangle.Margin = new Thickness(currMousePoint.X - _startPoint.X, currMousePoint.Y - _startPoint.Y, 0, 0);
              
                
                    #region Check Page Boundaries

                    if (_currentlyMovedRectangle.Margin.Left < -42) // Left
                    {
                        _currentlyMovedRectangle.Margin = new Thickness(-42, _currentlyMovedRectangle.Margin.Top, 0, 0);
                    }


                    if (_currentlyMovedRectangle.Margin.Left + _currentlyMovedRectangle.Width > ElementCanvas.Width) // Right
                    {
                        _currentlyMovedRectangle.Margin = new Thickness(ElementCanvas.Width - _currentlyMovedRectangle.Width,
                           _currentlyMovedRectangle.Margin.Top, 0, 0);
                    }

                    if (_currentlyMovedRectangle.Margin.Top < 0) // Top
                    {
                        _currentlyMovedRectangle.Margin = new Thickness(0, _currentlyMovedRectangle.Margin.Top, 0, 0);
                    }


                    if (_currentlyMovedRectangle.Margin.Top + _currentlyMovedRectangle.Height > ElementCanvas.Height)
                    {
                        _currentlyMovedRectangle.Margin = new Thickness(ElementCanvas.Height - _currentlyMovedRectangle.Height,
                            _currentlyMovedRectangle.Margin.Top, 0, 0);
                    }
                        
                    #endregion Calculate Offset
                    
                   var imageRect = new Rect(_currentlyMovedRectangle.Margin.Left, _currentlyMovedRectangle.Margin.Top,
                        _currentlyMovedRectangle.Width, _currentlyMovedRectangle.Height);

                    bool bIntersection = CheckCollisionWithDropRectangles(imageRect) != null || CheckCollisionWithRecycleBin(imageRect);

                    _currentlyMovedRectangle.Cursor = bIntersection ? Cursors.Arrow : Cursors.No;
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
                foreach (var i in _addedSavedData)
               //To remove the last added element from Canvas we use ElementCanvas.Children.Count -1
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
            try
            {
                var correctAnswers = 0;
                var wrongAnswers = 0;

                List<int> indicesToRemove = new List<int>();
                for (int i = _addedSavedData.Count - 1; i >= 0; i--)
                {
                    var data = _addedSavedData[i];
                    if (data.IsAnswerCorrect())
                    {
                        correctAnswers++;
                    }
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
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "Check_OnClick");
            }
        }

        /// <summary>
        /// This method is responsible for closing, respectively hiding the game instruction</summary>
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
            public Rectangle DropRectangle; // Drop rectangle reference, there should be unique drop rectangles in whole _addedSavedData
            public Rectangle ChildReference;
      
            public Thickness ImageMargin; // Position of copied data, we need it when switching pages
            public int OriginalImageChildIndex; // Index of original child we made copy of, it's index of imagebrush
            public int DropRectangleIndex; // Index of original child we made copy of, it's index of imagebrush

            public SavedDataForProgress() // TODO : Refactor with parameters
            {
                OriginalImageChildIndex = -1;
                DropRectangle = null;
            }

           /* public SavedDataForProgress(int originalImageChildIndex, Thickness ImageMargin, Rectangle dropPosition)
            {
                OriginalImageChildIndex = originalImageChildIndex;
                this.ImageMargin = ImageMargin;
                DropPosition = dropPosition;
            }*/

            public bool IsAnswerCorrect()
            {
                if (CorrectAnswers[OriginalImageChildIndex].Contains(DropRectangle.Name))
                {
                    return true;
                }
                return false;
            }
        }

        #endregion SavedDataForProgress

        private void OpenInstruction(object sender, RoutedEventArgs e)
        {
            GameInstruction.Visibility = Visibility.Visible;
            ButtonCloseGameInstruction.Visibility = Visibility.Visible;
            
            
        }
    }
}