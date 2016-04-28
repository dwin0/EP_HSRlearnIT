﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EP_HSRlearnIT.PresentationLayer.Games;
using EP_HSRlearnIT.PresentationLayer.Exercises;
using EP_HSRlearnIT.PresentationLayer.Tutorials;

namespace EP_HSRlearnIT.PresentationLayer
{
    public partial class MainPage2
    {
        #region Private Members

        private readonly SolidColorBrush _backgroundColor = Application.Current.FindResource("BackAreaBrush") as SolidColorBrush;
        #endregion

        #region Constructors

        public MainPage2()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Methods

        private void OverviewScreen_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AesGcmOverviewPage());
        }

        private void StepByStep_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new StepPage());
        }

        private void EncryptionDecryption_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new EncryptionDecyrptionPage());
        }

        private void DragAndDrop_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new DragDrop3());
        }
        #endregion

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Border parent = (sender as Grid)?.Parent as Border;

            if (parent != null)
            {
                SolidColorBrush black = new SolidColorBrush()
                {
                    Color = Colors.Black
                };

                parent.BorderBrush = black;
            }
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            Border parent = (sender as Grid)?.Parent as Border;

            if (parent != null)
            {
                parent.BorderBrush = _backgroundColor;
            }
        }
    }
}
