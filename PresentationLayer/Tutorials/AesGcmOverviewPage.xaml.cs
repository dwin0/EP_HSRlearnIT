using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace EP_HSRlearnIT.PresentationLayer.Tutorials
{
    public partial class AesGcmOverviewPage
    {
        #region Constructors
        public AesGcmOverviewPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Members
        private readonly ToolTip _toolTip = new ToolTip();
        #endregion Private Members

        #region Private Methods
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            Path pathName = sender as Path;
            string text = Application.Current.FindResource(pathName?.Name + "Text") as string;
            ChangeToolTip(text);
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            _toolTip.IsOpen = false;
        }

        private void ChangeToolTip(string tooltipText)
        {
            _toolTip.Content = tooltipText;
            _toolTip.IsOpen = true;
            _toolTip.StaysOpen = true;
        }
        #endregion Private Methods
    }
}
