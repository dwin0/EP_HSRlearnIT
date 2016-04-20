using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EP_HSRlearnIT.Games
{
    /// <summary>
    /// Interaction logic for DragDrop2.xaml
    /// </summary>
    public partial class DragDrop2 : Page
    {
        #region Private Members

        private ObservableCollection<DragImage> _availableParts;
        private ObservableCollection<DragImage> _setParts;
        private Point? _dragStartPosition = null;
        #endregion


        #region Constructors
        public DragDrop2()
        {
            InitializeComponent();
            InitParts();
            DataContext = this;
        }
        #endregion

        #region Private Methods

        private void InitParts()
        {
            _availableParts = new ObservableCollection<DragImage>();
            _setParts = new ObservableCollection<DragImage>();
            new[] { "Part1", "Part2", "Part3" }
                .ToList()
                .ForEach(part => _availableParts.Add(new DragImage() { Name = part, Image = $"/Images/dragdrop1.png" }));

            AvailableListBox.ItemsSource = _availableParts;
            //SetListBox.ItemsSource = _setParts;
        }

        private void AvailableListBox_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPosition = e.GetPosition(this);
        }

        private void AvailableListBox_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragStartPosition = null;
        }

        private bool IsMovementFarEnough(Point origPos, Point curPos)
        {
            var minDistX = SystemParameters.MinimumHorizontalDragDistance;
            var minDistY = SystemParameters.MinimumHorizontalDragDistance;

            return (Math.Abs(curPos.X - origPos.X) >= minDistX
                || Math.Abs(curPos.Y - origPos.Y) >= minDistY);
        }

        private void AvailableListBox_OnMouseMove(object sender, MouseEventArgs e)
        {
            //Drag doesn't start if click wasn't withing available list
            if (_dragStartPosition == null)
            {
                return;
            }
            //check if movement is far enough
            var position = e.GetPosition(this);
            if (!IsMovementFarEnough((Point)_dragStartPosition, position))
            {
                return;
            }
            //everything ok -> start Drag
            _dragStartPosition = null;
            StartDrag(AvailableListBox.SelectedItem as DragImage);
        }

        private void StartDrag<T>(T obj)
        {
            var dragScope = Content as FrameworkElement;
            var dragData = new DataObject(typeof(T), obj);
            DragDrop.DoDragDrop(dragScope, dragData, DragDropEffects.Move);
        }

        private void SetListBox_OnDragOver(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(DragImage)) as DragImage;
            //move object if available
            if (data != null)
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void SetListBox_OnDrop(object sender, DragEventArgs e)
        {
            var aesGcmPart = e.Data.GetData(typeof(DragImage)) as DragImage;
            _setParts.Add(aesGcmPart);
        }

        private void OnResetButtonClick(object sender, RoutedEventArgs e)
        {
            _setParts.Clear();
        }
        #endregion
    }
}
