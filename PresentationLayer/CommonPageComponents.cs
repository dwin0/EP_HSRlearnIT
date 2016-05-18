using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;

namespace EP_HSRlearnIT.PresentationLayer
{
    public abstract class CommonPageComponents : Page
    {
        public FrameworkElement Clone(FrameworkElement e)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(XamlWriter.Save(e));

            return (FrameworkElement)XamlReader.Load(new XmlNodeReader(document));
        }
    }
}
