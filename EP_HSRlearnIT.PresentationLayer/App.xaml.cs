using System.Windows;
using EP_HSRlearnIT.BusinessLayer.Persistence;

namespace EP_HSRlearnIT.PresentationLayer
{
    public partial class App
    {
        #region Private Methods
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            const string message = "Ein unerwarteter Fehler ist aufgetreten";
            const string title = "Exception Handler";
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            ExceptionLogger.WriteToLogfile("Global Exception Handler", e.Exception.Message, e.Exception.StackTrace);
        }

        #endregion
    }
}
