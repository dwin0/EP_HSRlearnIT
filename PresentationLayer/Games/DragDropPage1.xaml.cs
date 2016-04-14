using System;
using System.Collections.Generic;
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


namespace EP_HSRlearnIT.PresentationLayer.Games
{
    public partial class DragDropPage1 : Page
    {
        public DragDropPage1()
        {
            InitializeComponent();
            orderImages();
        }
        private void orderImages()
        {
            int i = 0;
            Image[] imgs = { hashsubkey, iv, counter, multH, auth_data, auth_tag, ciphertext, plaintext, len };
            foreach (var img in imgs)
            {
                img.Width = 80;
                img.Height = 35;
                img.Margin = new Thickness(600, -450 + i * 100, img.Width, img.Height);
                i++;
            }
        }
        List<UIElement> AddedImages = new List<UIElement>();

        bool isMoving = false;
        Image imgMoved = null;
        Thickness marginStart;
        // Position used for calculating mouse move
        Point previousMousePosition = new Point();

        Rect[] dropLocations =
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
            //First it is checked which image is moved, then a new image is created  and set with height, width and margin, so it appears on the same position as the imgMoved.
            imgMoved = (Image)sender;
            Image finalImage = new Image();
            finalImage.Width = 80;
            finalImage.Height = 35;
            finalImage.Margin = imgMoved.Margin;
            finalImage.Source = imgMoved.Source;
            finalImage.Name = "tmp" + imgMoved.Name;
            controlGrid.Children.Add(finalImage);

            //Set Eventhandler for new image so new image behaves as original 
            finalImage.PreviewMouseLeftButtonUp += image_PreviewMouseLeftButtonUp;
            imgMoved = finalImage;

            // Remember the initial mouse position
            previousMousePosition = e.GetPosition(this);
            marginStart = imgMoved.Margin;
            isMoving = true;
        }

        private void image_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMoving = false;

            //Check if the original image corresponds with one of the droplocations. If there is a intersection, then image can be dropped
            Rect hashsubkeyrect = new Rect(imgMoved.Margin.Left, imgMoved.Margin.Top, imgMoved.Width, imgMoved.Height);
            bool bIntersection = false;
            for (int i = 0; i < dropLocations.Length; i++)
            {
                Rect r = dropLocations[i];
                if (hashsubkeyrect.IntersectsWith(r))
                {
                    bIntersection = true;
                    imgMoved.Margin = new Thickness(r.Left - 80, r.Top -35, 0, 0);
                    AddedImages.Add(imgMoved);
                    break;
                }
            }
            if(!bIntersection) 
            {
                controlGrid.Children.Remove(imgMoved);
            }
        }
        private void image_MouseMove(object sender, MouseEventArgs e) 
        {
            if (isMoving)
            {
                Point currMousePoint = e.GetPosition(this);
                double dragHorizontal = currMousePoint.X - previousMousePosition.X; 
                double dragVertical = currMousePoint.Y - previousMousePosition.Y; 
                previousMousePosition = currMousePoint;
                Thickness oldMargin = imgMoved.Margin;
                oldMargin.Left += dragHorizontal*2;
                oldMargin.Top += dragVertical*2;
                imgMoved.Margin = oldMargin;

                //This part is only for debugging (so that the textbox gets showsn) and will be deleted for the final version
                Rect hashsubkeyrect = new Rect(imgMoved.Margin.Left, imgMoved.Margin.Top, imgMoved.Width, imgMoved.Height);
                bool bIntersection = false;
                for (int i = 0; i < dropLocations.Length; i++)
                {
                    Rect r = dropLocations[i];
                    if (hashsubkeyrect.IntersectsWith(r))

                        bIntersection = true;
                }
                textBox.Text = String.Format("X: {0}, Y:{1} INT: {2}", oldMargin.Left, oldMargin.Top, bIntersection ? "TRUE" : "FALSE");
            }
        }
        private void reset_onclick(object sender, RoutedEventArgs e)
        {
            var images = controlGrid.Children.OfType<Image>().ToList();
            foreach (var image in images)
            {
                if (image.Name.Contains("tmp"))
                    {
                    controlGrid.Children.Remove(image);
                    }
                }
            AddedImages.Clear();
        }
    }
}
