using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public static class DependencyObjectExtension
    {
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
    }
}