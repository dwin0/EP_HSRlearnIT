using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    /// <summary>
    /// Class log all global exceptions of the program.
    /// _path and _fileName are fix values.
    /// _maxSize is the favored max size of the logfile.
    /// _deleteRows is the amount of lines to delete into logfile when reached _maxSize.
    /// </summary>
    public static class ExceptionLogger
    {
        #region Private Member
        private static string _path = @"c:\logs";
        private static string _fileName = "ExceptionLog.log";
        //Size in Byte
        private static long _maxSize = 5 * 1024 * 1024;
        private static int _deleteRows = 10;

        #endregion


        #region Public Methods
        public static void WriteToLogfile(string exeptionMessage, string sourceMethod)
        {
            string filePath = FileManager.SaveFile(_path, _fileName);
            string entry = $"{Environment.NewLine}Exception: {DateTime.Now.ToString(CultureInfo.CurrentCulture)}: {exeptionMessage} {Environment.NewLine}{sourceMethod}";
            FileManager.AppendContent(filePath, entry);
            AvoidOverflow(filePath);
        }

        #endregion

        
        #region Private Methods
        private static void AvoidOverflow(String filePath)
        {
            if (FileManager.GetSize(filePath) >= _maxSize)
            {
                List<string> lines = FileManager.ReadAllLines(filePath).ToList();
                lines.RemoveRange(0, _deleteRows);
                FileManager.SwapContents(filePath, lines);
                FileManager.AppendContent(filePath, $"{Environment.NewLine}***" +
                                                    $"{Environment.NewLine}The oldest { _deleteRows} rows were removed." +
                                                    $"{Environment.NewLine}***");
            }
        }

        #endregion        
    }
}
