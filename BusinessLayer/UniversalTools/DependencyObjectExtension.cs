using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public static class DependencyObjectExtension
    {
        #region Public Methods
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