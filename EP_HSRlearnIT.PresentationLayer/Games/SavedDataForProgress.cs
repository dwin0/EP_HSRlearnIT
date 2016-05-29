using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EP_HSRlearnIT.PresentationLayer.Games
{
    /// <summary>
    /// This class is needed for the progress saving. In order to restore the progress, the ChildReference, Droprectangle Reference, ImageMargin, Brush,
    /// OriginalImageChildIndex and DropRectangleIndex is needed. 
    /// </summary>>
    internal class SavedDataForProgress
    {
        #region Public Member

        public Rectangle DropRectangle; // Drop rectangle reference, there should be unique drop rectangles in whole _addedSavedData
        public Rectangle ChildReference;

        public Thickness ImageMargin; // Position of copied data, we need it when switching pages
        public Brush Brush;
        public double StrokeDashArray;
        public int OriginalImageChildIndex; // Index of original child we made copy of, it's index of imagebrush
        public int DropRectangleIndex; // Index of original child we made copy of, it's index of imagebrush
        #endregion

        #region Internal Methods

        internal SavedDataForProgress()
        {
            OriginalImageChildIndex = -1;
            DropRectangle = null;
            Brush = null;
            StrokeDashArray = 0.0;
        }

        /// <summary>
        /// Method which is responsible for checking an solution for its correctness
        /// </summary>
        /// <returns>all correct drop rectangle locations from an dropped element</returns>
        internal bool IsAnswerCorrect()
        {
            List<string> isCorrect;
            DragDropPage.CorrectAnswers.TryGetValue(OriginalImageChildIndex, out isCorrect);
            return isCorrect != null && isCorrect.Contains(DropRectangle.Name);
        }
        #endregion
    }
}
