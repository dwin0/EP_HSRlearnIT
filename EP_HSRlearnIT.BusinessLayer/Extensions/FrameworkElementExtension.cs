using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace EP_HSRlearnIT.BusinessLayer.Extensions
{
    /// <summary>
    /// Contains all Extensions for FrameworkElement
    /// </summary>
    public static class FrameworkElementExtension
    {
        #region Public Methods
        /// <summary>
        /// Method to clone any Framework Element
        /// </summary>
        /// <param name="element">FrameworkElement to clone</param>
        /// <returns>Returns a clone of the given parameter 'element'</returns>
        public static FrameworkElement Clone(this FrameworkElement element)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(XamlWriter.Save(element));

            return (FrameworkElement)XamlReader.Load(new XmlNodeReader(document));
        }

        #endregion
    }
}
