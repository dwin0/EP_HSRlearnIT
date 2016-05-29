using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EP_HSRlearnIT.BusinessLayer.Persistence;
using EP_HSRlearnIT.BusinessLayer.Extensions;

namespace EP_HSRlearnIT.PresentationLayer.Games
{
    public partial class DragDropPage
    {
        #region Private Attributes
        private const string SettingsName = "DragDropPage_Settings";
        private const string RecycleBinStatus = "dragDropPage_RecycleBinFull";
        private const int ImageRecyclingBinEmpty = 17;
        private readonly int _originalNumberOfChildren;

        private static int NumOfDroppableRectanglePlaces = 17;
        private List<SavedDataForProgress> _addedSavedData;
        private readonly List<Rectangle> _dropLocationsRectangles = new List<Rectangle>(); 
        
        private SavedDataForProgress _currentlyAddedData;
        private Rectangle _currentlyMovedRectangle;
        private Rectangle _recycleBinRectangle;
        private readonly ImageBrush _brushRecycleBinEmpty = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/recycling.png", UriKind.RelativeOrAbsolute)));
        private readonly ImageBrush _brushRecycleBinFull = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/recyclebin.png", UriKind.RelativeOrAbsolute)));
        private bool _rectangleHasBeenMovedBefore;
        private Point _startPoint = new Point(0, 0);
        private bool _checkingFirstTime = true;
        #endregion

        public static readonly Dictionary<int, List<string>> CorrectAnswers = new Dictionary<int, List<string>>();

        #region Constructor
        /// <summary>
        /// Constructor Method to initialize the XAML, to generate the side Images, to initialise the drop places and to load the progress
        /// </summary>
        public DragDropPage()
        {
            InitializeComponent();
            GenerateSideImages();
            LoadDroppablePlaces(ElementCanvas);

            _originalNumberOfChildren = ElementCanvas.Children.Count;

            LoadSavedData();
            ShowTutorial();
            //Focus is needed in order to make the F1 und ECS Button work after a menu change
            Focus();
        }

        #endregion

        /// <summary>
        /// This method loads all Rectangles where a solution can be dropped.</summary>
        /// <param name="canvas">Is the Parent where the Rectangles have to be placed</param>
        private void LoadDroppablePlaces(Canvas canvas)
        {
            for (int i = 1; i <= NumOfDroppableRectanglePlaces; i++)
            {
                Rectangle droppablePlaces = Application.Current.FindResource("DroppablePlace" + i) as Rectangle;
                if (droppablePlaces == null) { continue; }

                Rectangle dropPlaceCopy = droppablePlaces.Clone() as Rectangle;

                if (dropPlaceCopy != null)
                {
                    if (i == ImageRecyclingBinEmpty)
                    {
                        _recycleBinRectangle = dropPlaceCopy;
                    }
                    else
                    {
                        dropPlaceCopy.Fill = Brushes.Transparent;
                        dropPlaceCopy.Width = 90;
                        dropPlaceCopy.Height = 40;
                    }
                        
                    canvas.Children.Add(dropPlaceCopy);

                    if (dropPlaceCopy.Name.Contains("Rect"))
                    {
                        _dropLocationsRectangles.Add(dropPlaceCopy);
                    }                    
                }
            }
        }

        /// <summary>
        /// This method loads the the data saved in SavedDataForProgress to get back the same state as before a menu change.
        /// </summary>
        private void LoadSavedData()
        {
            _addedSavedData = (List<SavedDataForProgress>)Progress.GetProgress(SettingsName) ?? new List<SavedDataForProgress>();
            int counter = 0;

            foreach (var rectangleData in _addedSavedData)
            {
                if(rectangleData == null) { continue; }
                var origImage = (Rectangle)ElementCanvas.Children[rectangleData.OriginalImageChildIndex];
                var newImage = CreateNewRectangle(origImage.Fill, 95, 45, "tmp" + counter, rectangleData.ImageMargin, rectangleData.Brush, rectangleData.StrokeDashArray, true, true, false);
                counter++;

                // here we have refreshed the original child reference and DropRectangle 
                rectangleData.ChildReference = newImage;
                rectangleData.DropRectangle = _dropLocationsRectangles[rectangleData.DropRectangleIndex];

                ElementCanvas.Children.Add(newImage);
            }
            bool? binFull = (bool?)Progress.GetProgress(RecycleBinStatus);
            if (binFull == true)
            {
                _recycleBinRectangle.Fill = _brushRecycleBinFull;
            }
        }

        /// <summary>
        /// This method restricts the tutorial to be shown after a menu change
        /// </summary>
        private void ShowTutorial()
        {
            var showGameInstruction = (bool?)Progress.GetProgress("ShowGameInstruction");

            if (showGameInstruction == null)
            {
                Progress.SaveProgress("ShowGameInstruction", true);
            }
            else
            {
                BorderGameInstruction.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// This method loads the images from the image-folder. These are the rectangle images to be moved. 
        /// </summary>
        private BitmapImage[] GetImages()
        {
            var images = new BitmapImage[9];

            for (var i = 1; i <= 9; i++)
            {
                try
                {
                    var image =
                        new BitmapImage(new Uri(@"pack://application:,,,/Images/dragdrop" + i + ".png", UriKind.RelativeOrAbsolute));
                    images[i - 1] = image;
                }
                catch(Exception ex)
                {
                    ExceptionLogger.WriteToLogfile("GetImage()", ex.Message, ex.StackTrace);
                    images[i - 1] = null;
                }
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
        /// <param name="brush">Brush (Border) of Rectangle</param>
        /// <param name="leftButtonDown">Rectangle should/should not be associated with the Mouse-Eventhandler leftButtonDown</param>
        /// <param name="strokeDashArray">Rectangle should/should not get a dotted line in the solution </param>
        /// <param name="leftButtonUp">Rectange should/should not be associated with the Mouse-Eventhandler LeftbuttonUp</param>
        /// <param name="moveMouse">Rectangle should/should not be associated with the Mouse-Eventhandler moveMouse</param>
        /// <returns>Return value is the newly created Rectangle</returns>
        private Rectangle CreateNewRectangle(Brush fill, double width, double height, string name, Thickness margin, Brush brush, double strokeDashArray, bool leftButtonDown, bool leftButtonUp, bool moveMouse)
        {
            Rectangle resRect = new Rectangle
            {
                Fill = fill,
                Width = width,
                Height = height,
                Name = name,
                Margin = margin,
                Stroke = brush,
                StrokeThickness = 4.0,
            };

            if(strokeDashArray > 0.0)
                resRect.StrokeDashArray = new DoubleCollection{ strokeDashArray }; 

            SetMouseEvents(resRect, leftButtonDown, leftButtonUp, moveMouse);
            return resRect;
        }
        private void SetMouseEvents(Rectangle resRect, bool leftButtonDown, bool leftButtonUp, bool moveMouse)
        {
            if (leftButtonDown) resRect.PreviewMouseLeftButtonDown += rectangle_PreviewMouseLeftButtonDown;
            if (leftButtonUp) resRect.MouseLeftButtonUp += rectangle_PreviewMouseLeftButtonUp;
            if (moveMouse) resRect.MouseMove += ElementCanvas_OnMouseMove;
        }

        /// <summary>
        /// This method iterates through the images and places them in the canvas. In order to associate these images to a droppable rectangle the list localCorrectAnswers is used. /// </summary>
        private void GenerateSideImages()
        {
            var images = GetImages();
            CorrectAnswers.Clear();
            List<List<string>> localCorrectAnswers = new List<List<string>>
            {
                new List<string> {"Rect13Hash"},
                new List<string> {"Rect1Iv"},
                new List<string> {"Rect7Counter", "Rect15Counter", "Rect17Counter"},
                new List<string> {"Rect2MultH", "Rect5MultH", "Rect9MultH", "Rect10MultH"},
                new List<string> {"Rect4Aad"},
                new List<string> {"Rect16Tag"},
                new List<string> {"Rect3Len"},
                new List<string> {"Rect6Ciphertext", "Rect11Ciphertext"},
                new List<string> {"Rect8Plaintext", "Rect12Plaintext"}
            };

            for (var i = 0; i < images.Length; i++)
            {
                if (images[i] == null) { continue; }
                var index = ElementCanvas.Children.Add(CreateNewRectangle(new ImageBrush(images[i]), 95, 45, "dragdrop" + i, new Thickness(800, 100 + i * 55, 0, 0), null, 0.0, true, true, false));
                CorrectAnswers.Add(index, localCorrectAnswers[i]);
            }
        }

        /// <summary>
        /// It checks if the mouse was was pressed down on an original rectangle image or an already copied image. 
        /// The moved rectangle is also saved into the progress and added as a Child to the ElementCanvas</summary>
        private void rectangle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                        "tmp" + _currentlyMovedRectangle.Name, _currentlyMovedRectangle.Margin, null, 0.0, true, true, true);

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

                    try
                    {
                        _currentlyAddedData = _addedSavedData[addedSavedDataIndex];
                    }
                    catch (Exception ex)
                    {
                        ExceptionLogger.WriteToLogfile("image_PreviewMouseLeftButtonDown", ex.Message, ex.StackTrace);
                    }
                }
                _currentlyMovedRectangle.Stroke = null;
                _currentlyMovedRectangle.StrokeDashArray = null;
            }
        }

        /// <summary>
        /// This method checks if there is an intersection with a dropLocationsRectangle</summary>
        /// <return>If intersection from DropLocation was found then the dropRectangle is returned, otherwise null</return>
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
        /// <returns>If intersection then true otherwise false</returns>
        private bool CheckCollisionWithRecycleBin(Rect imageRect)
        {
            var recycleRect = new Rect(Canvas.GetLeft(_recycleBinRectangle), Canvas.GetTop(_recycleBinRectangle), _recycleBinRectangle.Width, _recycleBinRectangle.Height);
            return imageRect.IntersectsWith(recycleRect);
        }

        /// <summary>
        /// Method checks if a droplocation is already filled with an image rectangle</summary>
        /// <param name="rect">rectangle field for which it is checked if its filled with an image rectangle or not</param>
        /// <returns>index -1 is returned if an rectangle field is still empty otherwise the index of the rectangle is returned </returns>
        private int IsRectangleFilled(Rectangle rect)
        {
            foreach (var data in _addedSavedData)
            {
                if (Equals(data.DropRectangle, rect))
                {
                    return _addedSavedData.IndexOf(data);
                }
            }
            return -1;
        }

        /// <summary>
        /// If there is an intersection between a dropRectangle, respectively recycle bin
        /// and the copied/moved image rectangle the image rectangle is dropped and 
        /// the Margin of the currentlyMovedRectangle is saved into the SaveDataForProgress.  
        /// </summary>
        private void rectangle_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_currentlyMovedRectangle == null)
            {
                return;
            }

            Rect rectangleInDraggedStatus = new Rect();
            try
            {
                rectangleInDraggedStatus = new Rect(_currentlyMovedRectangle.Margin.Left,
                    _currentlyMovedRectangle.Margin.Top, _currentlyMovedRectangle.Width,
                    _currentlyMovedRectangle.Height);
            }
            catch (ArgumentException ex)
            {
                ExceptionLogger.WriteToLogfile("DragDropPage_MouseLeftButtonUp", ex.Message, ex.StackTrace);
            }

            var dropRectangle = CheckCollisionWithDropRectangles(rectangleInDraggedStatus);
            // The collision happened
            if (dropRectangle != null)
            {
                //dropRectangle = defined rectangle on algorithm (e.g. rectMulH). 
                _currentlyMovedRectangle.Margin = new Thickness(Canvas.GetLeft(dropRectangle),
                    Canvas.GetTop(dropRectangle), 0, 0);
                _currentlyAddedData.ImageMargin = _currentlyMovedRectangle.Margin;

                var addedSavedDataIndex = IsRectangleFilled(dropRectangle);

                _currentlyAddedData.DropRectangle = dropRectangle;
                _currentlyAddedData.DropRectangleIndex = _dropLocationsRectangles.IndexOf(dropRectangle);

                // If it is not minus 1, that means we are putting the image to the already filled rectangle
                // Do not remove rectangle itself
                if (addedSavedDataIndex != -1 &&
                    !_addedSavedData[addedSavedDataIndex].ChildReference.Equals(_currentlyMovedRectangle))
                {
                    ElementCanvas.Children.Remove(_addedSavedData[addedSavedDataIndex].ChildReference);
                    try
                    {
                        _addedSavedData.RemoveAt(addedSavedDataIndex);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        ExceptionLogger.WriteToLogfile("DragDropPage_MouseLeftButtonUp", ex.Message, ex.StackTrace);
                    }
                }
                //if we move it from the intial place to an rectangle field
                if (!_rectangleHasBeenMovedBefore)
                {
                    _addedSavedData.Add(_currentlyAddedData);
                    if (_addedSavedData.Count == _dropLocationsRectangles.Count)
                    {
                        OpenDialogBox_AskForCheck();
                    }
                }
                Progress.SaveProgress(SettingsName, _addedSavedData);
            }
            // The collision did not happen
            else
            {
                // Get rid of the fresh copy
                if (!_rectangleHasBeenMovedBefore)
                {
                    ElementCanvas.Children.Remove(_currentlyMovedRectangle);
                }
                // Return child back to its original drop rectangle
                else
                {
                    _currentlyMovedRectangle.Margin = _currentlyAddedData.ImageMargin;
                }
            }

            if (CheckCollisionWithRecycleBin(rectangleInDraggedStatus))
            {
                ElementCanvas.Children.Remove(_currentlyMovedRectangle);
                _addedSavedData.Remove(_currentlyAddedData);
                _recycleBinRectangle.Fill = _brushRecycleBinFull;
                Progress.SaveProgress(RecycleBinStatus, true);
            }
            _currentlyMovedRectangle = null;
        }

        /// <summary>
        /// This method will open a dialag box, as soon as all drop rectangles are filled. User can check the solution.
        /// </summary>
        private void OpenDialogBox_AskForCheck()
        {
            if (!_checkingFirstTime)
            {
                return;
            }                
            _checkingFirstTime = false;
            string message = $"Alle Felder sind belegt. {Environment.NewLine}Klicke auf JA, um deine Lösung auszuwerten.";
            const string title = "Spiel beendet";
            if (MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.Yes) ==
                MessageBoxResult.Yes)
            {
                Check_OnClick(this, null);
            }
        }

        /// <summary>
        /// This method defines all the actions during the moveMouse, as change of mouse curser and setting boundries.
        /// </summary> 
        private void ElementCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_currentlyMovedRectangle != null)
            {
                //If LeftButton is released on the sideborder of the canvas/boundry, then we have to assign the respective PreviewMouseLeftButtonUp event
                if (e.LeftButton == MouseButtonState.Released)
                {
                    rectangle_PreviewMouseLeftButtonUp(sender, new MouseButtonEventArgs(InputManager.Current.PrimaryMouseDevice, 0, MouseButton.Left));
                    return;
                }
                    var currMousePoint = e.GetPosition(ElementCanvas);

                    _currentlyMovedRectangle.Margin = new Thickness(currMousePoint.X - _startPoint.X, currMousePoint.Y - _startPoint.Y, 0, 0);

                    CheckPageBoundaries();
                try
                {
                    var imageRect = new Rect(_currentlyMovedRectangle.Margin.Left, _currentlyMovedRectangle.Margin.Top, _currentlyMovedRectangle.Width, _currentlyMovedRectangle.Height);
                    bool intersection = CheckCollisionWithDropRectangles(imageRect) != null || CheckCollisionWithRecycleBin(imageRect);
                    _currentlyMovedRectangle.Cursor = intersection ? Cursors.Arrow : Cursors.No;
                }
                catch (Exception ex)
                {
                    ExceptionLogger.WriteToLogfile("ElementCanvas_OnMouseMove", ex.Message, ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// This method sets the boundries of the mouse for the game </summary>
        private void CheckPageBoundaries()
        {
            // Left
            if (_currentlyMovedRectangle.Margin.Left < -42)
            {
                _currentlyMovedRectangle.Margin = new Thickness(-42, _currentlyMovedRectangle.Margin.Top, 0, 0);
            }

            // Right
            if (_currentlyMovedRectangle.Margin.Left + _currentlyMovedRectangle.Width > ElementCanvas.Width)
            {
                _currentlyMovedRectangle.Margin = new Thickness(ElementCanvas.Width - _currentlyMovedRectangle.Width, _currentlyMovedRectangle.Margin.Top, 0, 0);
            }
            
            // Top
            if (_currentlyMovedRectangle.Margin.Top < 0)
            {
                _currentlyMovedRectangle.Margin = new Thickness(_currentlyMovedRectangle.Margin.Left, 0, 0, 0);
            }

            // Bottom
            if (_currentlyMovedRectangle.Margin.Top + _currentlyMovedRectangle.Height > ElementCanvas.Height)
            {
                _currentlyMovedRectangle.Margin = new Thickness(_currentlyMovedRectangle.Margin.Left, ElementCanvas.Height - _currentlyMovedRectangle.Height, 0, 0);
            }
        }

        /// <summary>
        /// This method makes a reset of the whole game. </summary> 
        private void Reset_OnClick(object sender, RoutedEventArgs e)
        {
            int addedChildrenCount = _addedSavedData.Count;
            ElementCanvas.Children.RemoveRange(ElementCanvas.Children.Count - addedChildrenCount, addedChildrenCount);

            _addedSavedData.Clear();
            _recycleBinRectangle.Fill = _brushRecycleBinEmpty;
            Progress.SaveProgress(RecycleBinStatus, false);
        }

        /// <summary>
        /// This methods checks the dropped elements for their correctness. If correct, rectangle gets a green border, if wrong, the rectangle gets a red border.</summary>
        private void Check_OnClick(object sender, RoutedEventArgs e)
        {
                int correctAnswers = 0;
                int wrongAnswers = 0;
            
                foreach (var data in _addedSavedData)
                {
                    if (data.IsAnswerCorrect())
                    {
                        correctAnswers++;
                        data.Brush = new SolidColorBrush(Colors.SeaGreen);
                        data.ChildReference.Stroke = data.Brush;
                        data.StrokeDashArray = 0.0;
                    }
                    else
                    {  
                        wrongAnswers++;
                        data.Brush = new SolidColorBrush(Colors.Red);
                        data.ChildReference.Stroke = data.Brush;
                        data.ChildReference.StrokeDashArray = new DoubleCollection{ 5 };
                        data.StrokeDashArray = 5.0;
                    }
                    data.ChildReference.StrokeThickness = 4;
                }
            if (correctAnswers == _addedSavedData.Count && correctAnswers == _dropLocationsRectangles.Count)
            {
                const string message = "Super, du hast das Spiel korrekt ausgefüllt. Gratuliere!";
                const string title = "Spielresultat";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string message =$"Korrekte Antwort(en): {correctAnswers} {Environment.NewLine}Falsche Antwort(en): {wrongAnswers}";
                const string title = "Spielresultat";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        /// <summary>
        /// This method is responsible for closing, respectively hiding the game instruction</summary>
        /// <param name="sender">Contains the control which raised the event</param>
        /// <param name="e"></param>
        private void CloseInstruction_OnClick(object sender, RoutedEventArgs e)
        {
            BorderGameInstruction.Visibility = Visibility.Hidden;
        }
        
        /// <summary>
        /// This method opens the game instructions. </summary>
        private void OpenInstruction_OnMouseUp(object sender, RoutedEventArgs e)
        {
            BorderGameInstruction.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// This methods defines the behaviour when pressend on F1 or ESC
        /// </summary>        
        private void DragDrop_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    OpenInstruction_OnMouseUp(this, e);
                    break;
                case Key.Escape:
                    CloseInstruction_OnClick(this, e);
                    break;
                default:
                    return;
            }
        }
    }
}