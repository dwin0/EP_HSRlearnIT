using System;
using System.Windows.Media.Imaging;

namespace EP_HSRlearnIT
{
    /// <summary>
    /// Template for MenuTiles within the MainPage
    /// </summary>
    public partial class MenuTile
    {
        private readonly string _text;
        private readonly string _imageSource;

        /// <summary>
        /// Create a Tile with a Text and an Image-Source
        /// </summary>
        /// <param name="text">Tile-title</param>
        /// <param name="image">Image within the tile</param>
        public MenuTile(string text, string image)
        {
            _text = text;
            _imageSource = image;
            InitializeComponent();
            LoadTile();
        }

        private void LoadTile()
        {
            TileImage.Source = new BitmapImage(new Uri(_imageSource, UriKind.RelativeOrAbsolute));
            TileText.Text = _text;
        }
    }
}
