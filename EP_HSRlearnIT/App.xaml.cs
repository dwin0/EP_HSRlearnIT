using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EP_HSRlearnIT
{
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //Noch definieren, welcher Text ausgegeben werden soll
            MessageBox.Show("Unhandled exception occured [global exception handler] :" + e.Exception.Message);
            ExceptionLogger loghandle = new ExceptionLogger();
            loghandle.writeToLogFile(e.Exception.StackTrace, "unhandled exception");
        }
    }
}
