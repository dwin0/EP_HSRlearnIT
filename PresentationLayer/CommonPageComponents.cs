using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;

namespace EP_HSRlearnIT.PresentationLayer
{
    /// <summary>
    /// Base-Class which contains all common Page-Methods
    /// </summary>
    public abstract class CommonPageComponents : Page
    {
        #region Public Methods

        /// <summary>
        /// Method to clone any Framework Element
        /// </summary>
        /// <param name="e">FrameworkElement to clone</param>
        /// <returns>Returns a clone of the given parameter</returns>
        public FrameworkElement Clone(FrameworkElement e)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(XamlWriter.Save(e));

            return (FrameworkElement)XamlReader.Load(new XmlNodeReader(document));
        }
        #endregion
    }
}
