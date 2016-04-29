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
        private static int _deleteRows = 10;

        #endregion

        #region Public Methods
        public static void WriteToLogfile(string exeptionMessage, string sourceMethod)
        {
            string filePath = FileManager.SaveFile(_path, _fileName);
            string entry = $"{Environment.NewLine}Exception: {DateTime.Now.ToString(CultureInfo.CurrentCulture)}: {exeptionMessage} {sourceMethod}";
            FileManager.AppendContentToFile(filePath, entry);
            AvoidOverflow(filePath);
        }

        #endregion

        
        #region Private Methods
        private static void AvoidOverflow(String filePath)
        {
            if (FileManager.GetSize(filePath) >= _maxSize)
            {
                List<string> lines = File.ReadAllLines(filePath).ToList<string>();
                lines.RemoveRange(0, _deleteRows);
                File.WriteAllLines(filePath, lines);
                FileManager.AppendContentToFile(filePath, "\n The oldest " + _deleteRows + " lines are removed. \n");
            }
        }

        #endregion        
    }
}
