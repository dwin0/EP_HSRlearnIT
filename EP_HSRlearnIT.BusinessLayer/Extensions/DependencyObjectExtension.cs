using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EP_HSRlearnIT.BusinessLayer.Extensions
{
    /// <summary>
    /// Class with methods to create a list of objects from a xaml file.
    /// </summary>
    public static class DependencyObjectExtension
    {

        #region Public Methods
        /// <summary>
        /// Method which creates a list of all xmal objects of a page.
        /// </summary>
        /// <param name="root">Root is the reference to the page.</param>
        /// <returns>Returns a list of all xmal objects from root.</returns>
        public static IEnumerable<DependencyObject> GetAllChildren(this DependencyObject root)
        {
            if (root == null)
            {
                yield break;
            }

            yield return root;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);

                foreach (var subChild in child.GetAllChildren())
                {
                    yield return subChild;
                }
            }
        }

        /// <summary>
        /// Method which creates a list with the xamal objects of an explizit type.
        /// </summary>
        /// <typeparam name="T">Type which is searched in the object list.</typeparam>
        /// <param name="root">Is the reference to the page.</param>
        /// <returns>Returns a list of all xmal objects of the type T from root.</returns>
        public static IEnumerable<T> GetAllChildren<T>(this DependencyObject root) where T : class
        {
            foreach (var element in root.GetAllChildren())
            {
                if (element is T)
                {
                    yield return element as T;
                }
            }
        }

        /// <summary>
        /// Method which returns the parent Page of an element.
        /// </summary>
        /// <param name="element">The element for which the parent Page is searched.</param>
        /// <returns>The Page in which the element is existing.</returns>
        public static DependencyObject GetParentPage(this DependencyObject element)
        {
            var parent = VisualTreeHelper.GetParent(element);
            while (!(parent is Page))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent;
        }

        #endregion
    }
}