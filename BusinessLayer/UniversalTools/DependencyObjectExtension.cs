using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    /// <summary>
    /// Class to service methods to create list of objects from a xaml file.
    /// </summary>
    public static class DependencyObjectExtension
    {

        #region Public Methods
        /// <summary>
        /// Method return a list of all xmal objects.
        /// </summary>
        /// <param name="root"></param>is the reference to the page.
        /// <returns></returns>
        public static IEnumerable<DependencyObject> GetAllChildren(DependencyObject root)
        {
            if (root == null)
            {
                yield break;
            }

            yield return root;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);

                foreach (var subChild in GetAllChildren(child))
                {
                    yield return subChild;
                }
            }
        }

        /// <summary>
        /// Method returns a list with the xamal objects from a explizit type
        /// </summary>
        /// <typeparam name="T"></typeparam>type to search.
        /// <param name="root"></param>reference of page.
        /// <returns></returns>
        public static IEnumerable<T> GetAllChildren<T>(DependencyObject root) where T : class
        {
            foreach (var element in GetAllChildren(root))
            {
                if (element is T)
                {
                    yield return element as T;
                }
            }
        }

        #endregion
    }
}