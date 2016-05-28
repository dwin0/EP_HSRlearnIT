using System;
using System.Globalization;

namespace EP_HSRlearnIT.BusinessLayer.Persistence
{
    /// <summary>
    /// Class to log all exceptions of the program.
    /// </summary>
    public static class ExceptionLogger
    {
        #region Private Member
        private const string Path = @"c:\logs";
        private const string FileName = "ExceptionLog.log";
        //Size in Byte
        private const long MaxSizeLogfile = 5*1024*1024;
        private const int RowsToDelete = 200;

        #endregion


        #region Public Methods

        /// <summary>
        /// This method handles the creation of the log file and saves the content of an exception.
        /// </summary>
        /// <param name="sourceMethod">Name of the method which threw the exception</param>
        /// <param name="exceptionMessage">Message which describes the exception</param>
        /// <param name="stackTrace">shows all method calls which where involved in this exception</param>
        public static void WriteToLogfile(string sourceMethod, string exceptionMessage, string stackTrace )
        {
            string filePath = FileManager.SaveFile(Path, FileName);
            string entry = $"Exception: {DateTime.Now.ToString(CultureInfo.CurrentCulture)}: {sourceMethod}{Environment.NewLine}{exceptionMessage}{Environment.NewLine}{stackTrace}{Environment.NewLine}";
            FileManager.AppendContent(filePath, entry);
            FileManager.AvoidOverflow(filePath, MaxSizeLogfile, RowsToDelete);
        }

        #endregion       
    }
}
