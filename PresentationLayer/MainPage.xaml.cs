using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using EP_HSRlearnIT.PresentationLayer.Games;
using EP_HSRlearnIT.PresentationLayer.Exercises;
using EP_HSRlearnIT.PresentationLayer.Tutorials;

namespace EP_HSRlearnIT.PresentationLayer
{
    /// <summary>
    /// Page which contains the program-navigation
    /// </summary>
    public partial class MainPage
    {
        #region Private Members

        private const string Overview = "AES-GCM - Übersicht";
        private const string StepByStep = "Schritt für Schritt - Anleitung";
        private const string EncryptionDecryption = "Ver- & Entschlüsselungs - Anwendung";
        private const string DragDrop = "Drag & Drop - Spiel";

        private readonly Dictionary<string, string> _tileDictionary = new Dictionary<string, string>()
            {
                { Overview, @"Images/eye-icon.png"},
                { StepByStep, @"Images/step-icon.png"},
                { EncryptionDecryption, @"Images/key-icon.png"},
                { DragDrop, @"Images/drag-icon.png"}
            };

        private readonly SolidColorBrush _greenBackgroundBrush = Application.Current.FindResource("TileBackgroundBrush") as SolidColorBrush;
        private readonly SolidColorBrush _blackBorderBrush = Application.Current.FindResource("TileBorderBrush") as SolidColorBrush;
        private MenuTile _mouseDownMenuTile;
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
            //These chars have to be replaced to prevent a XAMLParseException
            //when the strings are set as Name-Property to the Menu-Tiles
            Dictionary<string, string> stringsToReplace = new Dictionary<string, string>
            {
                {" ", string.Empty},
                {"-", string.Empty},
                {"Ü", "Ue"},
                {"ü", "ue"},
                {"&", "and"}
            };

            foreach (var tileEntry in _tileDictionary)
            {
                MenuTile tile = new MenuTile(tileEntry.Key, tileEntry.Value);
                tile.TileImage.Name = tileEntry.Key.MultipleReplace(stringsToReplace);
                //Mouse Up and Down - Events to make it feel lika a click. There is no Click-Event for Tiles.
                tile.MouseDown += OnTileMouseDown;
                tile.MouseUp += OnTileMouseUp;
                tile.MouseEnter += MenuTile_OnMouseEnter;
                tile.MouseLeave += MenuTile_OnMouseLeave;
                MenuGrid.Children.Add(tile);
            }
        }

        private void OnTileMouseDown(object sender, RoutedEventArgs e)
        {
            _mouseDownMenuTile = sender as MenuTile;
        }

        private void OnTileMouseUp(object sender, RoutedEventArgs e)
        {
            MenuTile tile = sender as MenuTile;
            if (tile == null)
            {
                ExceptionLogger.WriteToLogfile("No MenuTile was found - tile was null", "MainPage: OnTileMouseUp");
            } else if (tile.Equals(_mouseDownMenuTile))
            {
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
                        toNavigatePage = new EncryptionDecryptionTabs();
                        break;
                    case DragDrop:
                        toNavigatePage = new DragDropPage();
                        break;
                }

                if (toNavigatePage != null)
                {
                    NavigationService?.Navigate(toNavigatePage);
                }
                else
                {
                    ExceptionLogger.WriteToLogfile("No matching MenuTile-Text", "MainPage: OnTileMouseUp");
                }
            }
        }

        private void MenuTile_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ChangeBorder(sender, _blackBorderBrush);
        }

        private void MenuTile_OnMouseLeave(object sender, MouseEventArgs e)
        {
            ChangeBorder(sender, _greenBackgroundBrush);
        }

        private void ChangeBorder(object sender, SolidColorBrush brush)
        {
            MenuTile tile = sender as MenuTile;

            if (tile != null)
            {
                tile.TileBorder.BorderBrush = brush;
            }
        }

        #endregion
    }


    #region Extension Methods

    /// <summary>
    /// Extension Class for strings
    /// </summary>
    public static class StringExtender
    {
        /// <summary>
        /// Extension Method to replace multiple strings within a text
        /// </summary>
        /// <param name="text">Text in which replacements are made</param>
        /// <param name="replacements">Dictionary which contains a string-string - Pair.
        /// First string: Pattern searched in text - Second string: Text to replace the pattern</param>
        /// <returns>Text in which all found patterns are replaced with a given string</returns>
        public static string MultipleReplace(this string text, Dictionary<string, string> replacements)
        {
            string returnValue = text;
            foreach (string textToReplace in replacements.Keys)
            {
                returnValue = returnValue.Replace(textToReplace, replacements[textToReplace]);
            }
            return returnValue;
        }
    }
    #endregion
}
