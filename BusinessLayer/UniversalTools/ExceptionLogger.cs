using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public static class ExceptionLogger
    {
        #region Private Member
        private static string _path = @"c:\logs";
        private static string _fileName = "ExceptionLog.log";
        private static string _filePath;
        //Size in Byte
        private static long _maxSize = 5 * 1024 * 1024;
        private static int _DeleteRows = 10;

        #endregion

        #region Public Methods
        public static void WriteToLogfile(string exmsg, string sourceMethod)
        {
            FileSaver.CreateFile(_path, _fileName);
            _filePath = Path.Combine(_path, _fileName);
            String entry = "\n" + "Exception :" + DateTime.Now.ToString() + ": " + exmsg + " " + sourceMethod + "\n";
            FileSaver.AppendContentToFile(_filePath, entry);
            AvoidOverflow(_filePath);
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
                FileSaver.AppendContentToFile(filePath, "The oldest " + _DeleteRows + " lines are removed. \n");
            }
        }

        #endregion

    }
}
