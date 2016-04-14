using System.Windows;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.PresentationLayer
{
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //Noch definieren, welcher Text ausgegeben werden soll
            MessageBox.Show("Unhandled exception occured [global exception handler] :" + e.Exception.Message);
            ExceptionLogger.WriteToLogfile(e.Exception.StackTrace, "unhandled exception");
        }
    }
}
