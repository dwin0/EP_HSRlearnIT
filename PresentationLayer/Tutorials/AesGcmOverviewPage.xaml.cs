using System.Windows.Controls;

namespace EP_HSRlearnIT.PresentationLayer.Tutorials
{
    /// <summary>
    /// Interaction logic for AesGcmOverviewPage.xaml
    /// </summary>
    public partial class AesGcmOverviewPage : Page
    {
        #region Constructors

        public AesGcmOverviewPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Methods
        private void Vorbereitung_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ToolTip = "Erklärung zu Vorbereitung";
        }

        private void Vorbereitung_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }


        private void Verschlüsselung_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ToolTip = "Erklärung zu Verschlüsselung";
        }

        private void Verschlüsselung_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void AdditionalAuthenticatedData_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ToolTip = "Erklärung zu Additional Authenticated Data";
        }

        private void AdditionalAuthenticatedData_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void CiphertextAuthentifizierung_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ToolTip = "Erklärung zu Ciphertext Authentifizierung";
        }

        private void CiphertextAuthentifizierung_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void Wiederholung_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ToolTip = "Erklärung zu Wiederholung";
        }

        private void Wiederholung_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void AbschlussUndRückgabe_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ToolTip = "Erklärung zu Abschluss und Rückgabe";
        }

        private void AbschlussUndRückgabe_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        #endregion Public Methods
    }
}
