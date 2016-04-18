using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public static class ExceptionLogger
    {
        #region Private Member
        private static string _path = @"c:\logs";
        private static string _fileName = "ExceptionLog.log";
        //Size in Byte
        private static long _maxSize = 5 * 1024 * 1024;
        private static int _DeleteRows = 10;

        #endregion

        #region Public Methods
        public static void WriteToLogfile(string exeptionMessage, string sourceMethod)
        {
            string filePath = FileSaver.SaveFile(_path, _fileName);
            string entry = $"{Environment.NewLine}Exception: {DateTime.Now.ToString(CultureInfo.CurrentCulture)}: {exeptionMessage} {sourceMethod}";
            FileSaver.AppendContentToFile(filePath, entry);
            AvoidOverflow(filePath);
        }

        #endregion

        
        #region Private Methods
        private static void AvoidOverflow(String filePath)
        {
            if (FileSaver.GetSize(filePath) >= _maxSize)
            {
                List<string> lines = File.ReadAllLines(filePath).ToList<string>();
                lines.RemoveRange(0, _DeleteRows);
                File.WriteAllLines(filePath, lines);
                FileSaver.AppendContentToFile(filePath, "\n The oldest " + _DeleteRows + " lines are removed. \n");
            }
        }

        #endregion        
    }
}
