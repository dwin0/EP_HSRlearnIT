using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EP_HSRlearnIT.Windows
{
    /// <summary>
    /// Interaction logic for DragDropPage.xaml
    /// </summary>
    public partial class DragDropPage : Page
    {
        public ObservableCollection<AesGcmPart> AvailableParts;
        public ObservableCollection<AesGcmPart> SetParts;

        public DragDropPage()
        {
            InitializeComponent();
            InitParts();
            DataContext = this;
        }

        private void InitParts()
        {
            AvailableParts = new ObservableCollection<AesGcmPart>();
            SetParts = new ObservableCollection<AesGcmPart>();
            new[] { "Part1", "Part2", "Part3" }
                .ToList()
                .ForEach(part => AvailableParts.Add(new AesGcmPart() { Name = part, Image = $"/images/{part.ToLower()}.png" }));

            AvailableListBox.ItemsSource = AvailableParts;
            SetListBox.ItemsSource = SetParts;
        }

        private Point? dragStartPosition = null;

        private void AvailableListBox_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragStartPosition = e.GetPosition(this);
        }

        private void AvailableListBox_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dragStartPosition = null;
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
            if(dragStartPosition == null)
            {
                return;
            }
            //check if movement is far enough
            var position = e.GetPosition(this);
            if(!IsMovementFarEnough((Point)dragStartPosition, position))
            {
                return;
            }
            //everything ok -> start Drag
            dragStartPosition = null;
            StartDrag(AvailableListBox.SelectedItem as AesGcmPart);
        }


        private void StartDrag<T>(T obj)
        {
            var dragScope = this.Content as FrameworkElement;
            var dragData = new DataObject(typeof(T), obj);
            DragDrop.DoDragDrop(dragScope, dragData, DragDropEffects.Move);
        }


        private void SetListBox_OnDragOver(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(AesGcmPart)) as AesGcmPart;
            //move object if available
            if(data != null)
            {
                e.Effects = DragDropEffects.Copy;
            } else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void SetListBox_OnDrop(object sender, DragEventArgs e)
        {
            var aesGcmPart = e.Data.GetData(typeof(AesGcmPart)) as AesGcmPart;
            SetParts.Add(aesGcmPart);
        }

        private void OnResetButtonClick(object sender, RoutedEventArgs e)
        {
            SetParts.Clear();
        }
    }
}
