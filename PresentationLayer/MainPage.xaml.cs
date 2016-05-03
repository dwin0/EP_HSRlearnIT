using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EP_HSRlearnIT.PresentationLayer.Games;
using EP_HSRlearnIT.PresentationLayer.Exercises;
using EP_HSRlearnIT.PresentationLayer.Tutorials;

namespace EP_HSRlearnIT.PresentationLayer
{
    /// <summary>
    /// Page containing the program-navigation
    /// </summary>
    public partial class MainPage
    {
        #region Private Members
        private readonly Dictionary<string, KeyValuePair<string, Page>> _tileDictionary = new Dictionary<string, KeyValuePair<string, Page>>()
            {
                { "Übersicht AES GCM", new KeyValuePair<string, Page>(@"Images/eye-icon.png", new AesGcmOverviewPage())},
                { "Schritt für Schritt Anleitung", new KeyValuePair<string, Page>(@"Images/step-icon.png", new StepByStepPage())},
                { "Ver- und Entschlüsselung", new KeyValuePair<string, Page>(@"Images/key-icon.png", new EncryptionDecyrptionPage())},
                { "Drag- und Drop - Spiel", new KeyValuePair<string, Page>("Images/drag-icon.png", new DragDropPage())}
            };

        private readonly SolidColorBrush _backgroundBrush = Application.Current.FindResource("TileBackgroundBrush") as SolidColorBrush;
        private readonly SolidColorBrush _borderBrush = Application.Current.FindResource("TileBorderBrush") as SolidColorBrush;
        #endregion

        #region Constructors

        /// <summary>
        /// Method to initialize the XAML and load all MenuTiles
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            LoadTiles();
        }
        #endregion

        #region Private Methods

        private void LoadTiles()
        {
            foreach (var tileEntry in _tileDictionary)
            {
                MenuTile tile = new MenuTile(tileEntry.Key, tileEntry.Value.Key);
                tile.PreviewMouseLeftButtonDown += OnTileClick;
                tile.MouseEnter += MenuTile_OnMouseEnter;
                tile.MouseLeave += MenuTile_OnMouseLeave;
                MenuGrid.Children.Add(tile);
            }
        }

        private void OnTileClick(object sender, RoutedEventArgs e)
        {
            MenuTile tile = sender as MenuTile;
            if (tile == null) return;

            KeyValuePair<string, Page> toNavigatePage;
            bool available = _tileDictionary.TryGetValue(tile.TileText.Text, out toNavigatePage);
            if (available)
            {
                NavigationService?.Navigate(toNavigatePage.Value);
            }
        }

        private void MenuTile_OnMouseEnter(object sender, MouseEventArgs e)
        {
            MenuTile tile = sender as MenuTile;

            if (tile != null)
            {
                tile.TileBorder.BorderBrush = _borderBrush;
            }
        }

        private void MenuTile_OnMouseLeave(object sender, MouseEventArgs e)
        {
            MenuTile tile = sender as MenuTile;

            if (tile != null)
            {
                tile.TileBorder.BorderBrush = _backgroundBrush;
            }
        }

        #endregion
    }
}
