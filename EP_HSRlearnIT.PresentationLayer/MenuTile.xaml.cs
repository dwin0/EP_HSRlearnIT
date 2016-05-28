using System;
using System.Windows.Media.Imaging;

namespace EP_HSRlearnIT.PresentationLayer
{
    /// <summary>
    /// Template for MenuTiles within the MainPage
    /// </summary>
    public partial class MenuTile
    {
        #region Private Members

        private readonly string _text;
        private readonly string _imageSource;
        #endregion

        #region Constructors

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
        #endregion

        #region Private Methods
        private void LoadTile()
        {
            TileImage.Source = new BitmapImage(new Uri(_imageSource, UriKind.RelativeOrAbsolute));
            TileText.Text = _text;
        }
        #endregion
    }
}
