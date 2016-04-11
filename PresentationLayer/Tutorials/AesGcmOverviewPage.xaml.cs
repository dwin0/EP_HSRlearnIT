using System.Windows.Controls;

namespace EP_HSRlearnIT.PresentationLayer.Tutorials
{
    /// <summary>
    /// Interaction logic for AesGcmOverviewPage.xaml
    /// </summary>
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
        private void Vorbereitung_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _toolTip.Content = "Erklärung zu Vorbereitung";
            _toolTip.IsOpen = true;
            _toolTip.StaysOpen = true;
        }

        private void Vorbereitung_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _toolTip.IsOpen = false;
        }


        private void Verschlüsselung_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _toolTip.Content = "Erklärung zu Verschlüsselung";
            _toolTip.IsOpen = true;
            _toolTip.StaysOpen = true;
        }

        private void Verschlüsselung_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _toolTip.IsOpen = false;
        }

        private void AdditionalAuthenticatedData_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _toolTip.Content = "Erklärung zu Additional Authenticated Data";
            _toolTip.IsOpen = true;
            _toolTip.StaysOpen = true;
        }

        private void AdditionalAuthenticatedData_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _toolTip.IsOpen = false;
        }

        private void CiphertextAuthentifizierung_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _toolTip.Content = "Erklärung zu Ciphertext Authentifizierung";
            _toolTip.IsOpen = true;
            _toolTip.StaysOpen = true;
        }

        private void CiphertextAuthentifizierung_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _toolTip.IsOpen = false;
        }

        private void Wiederholung_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _toolTip.Content = "Erklärung zu Wiederholung";
            _toolTip.IsOpen = true;
            _toolTip.StaysOpen = true;
        }

        private void Wiederholung_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _toolTip.IsOpen = false;
        }

        private void AbschlussUndRückgabe_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _toolTip.Content = "Erklärung zu Abschluss und Rückgabe";
            _toolTip.IsOpen = true;
            _toolTip.StaysOpen = true;
        }

        private void AbschlussUndRückgabe_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _toolTip.IsOpen = false;
        }

        #endregion Private Methods
    }
}
