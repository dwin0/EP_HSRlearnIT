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
        private const string Overview = "Übersicht AES-GCM";
        private const string StepByStep = "Schritt für Schritt - Anleitung";
        private const string EncryptionDecryption = "Ver- und Entschlüsselung";
        private const string DragDrop = "Drag & Drop - Spiel";

        #region Private Members
        private readonly Dictionary<string, string> _tileDictionary = new Dictionary<string, string>()
            {
                { Overview, @"Images/eye-icon.png"},
                { StepByStep, @"Images/step-icon.png"},
                { EncryptionDecryption, @"Images/key-icon.png"},
                { DragDrop, "Images/drag-icon.png"}
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
                case Overview:
                    toNavigatePage = new AesGcmOverviewPage();
                    break;
                case StepByStep:
                    toNavigatePage = new StepByStepPage();
                    break;
                case EncryptionDecryption:
                    toNavigatePage = new EncryptionDecyrptionPage();
                    break;
                case DragDrop:
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
