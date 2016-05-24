using System.Windows;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.PresentationLayer
{
    public partial class App
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //Noch definieren, welcher Text ausgegeben werden soll
            string message = $"Ein unerwarteter Fehler ist aufgetreten";
            string title = "Exception Handler";
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            ExceptionLogger.WriteToLogfile("Global Exception Handler", e.Exception.Message, e.Exception.StackTrace);
        }
    }
}
