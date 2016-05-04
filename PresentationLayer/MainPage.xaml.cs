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
        private readonly Dictionary<string, string> _tileDictionary = new Dictionary<string, string>()
            {
                { "Übersicht AES GCM", @"Images/eye-icon.png"},
                { "Schritt für Schritt Anleitung", @"Images/step-icon.png"},
                { "Ver- und Entschlüsselung", @"Images/key-icon.png"},
                { "Drag- und Drop - Spiel", "Images/drag-icon.png"}
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
                MenuTile tile = new MenuTile(tileEntry.Key, tileEntry.Value);
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

            Page toNavigatePage = null;

            switch (tile.TileText.Text)
            {
                case "Übersicht AES GCM":
                    toNavigatePage = new AesGcmOverviewPage();
                    break;
                case "Schritt für Schritt Anleitung":
                    toNavigatePage = new StepByStepPage();
                    break;
                case "Ver- und Entschlüsselung":
                    toNavigatePage = new EncryptionDecyrptionPage();
                    break;
                case "Drag- und Drop - Spiel":
                    toNavigatePage = new DragDropPage();
                    break;
            }

            if (toNavigatePage != null)
            {
                NavigationService?.Navigate(toNavigatePage);
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
