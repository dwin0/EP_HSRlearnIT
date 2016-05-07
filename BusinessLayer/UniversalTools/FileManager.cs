using System.Collections.Generic;
using System.IO;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public static class FileManager
    {

        #region Public Methods
        public static void UpdateContent(string file, string currentContent)
        {
            WriteFile(file, currentContent, false);
        }

        public static void AppendContent(string file, string currentContent)
        {
            WriteFile(file, currentContent, true);
        }

        public static void SwapContents(string filePath, IEnumerable<string> newContent)
        {
            UpdateContent(filePath, "");
            File.WriteAllLines(filePath, newContent);
        }

        public static string ReadFullContent(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                return sr.ReadToEnd();
            }
        }

        public static IEnumerable<string> ReadAllLines(string filePath)
        {
            return File.ReadLines(filePath);
        }

        public static long GetSize(string filePath)
        {
            return new FileInfo(filePath).Length;
        }

        public static bool IsExist(string filePath)
        {
            return File.Exists(filePath);
        }

        public static string SaveFile(string filePath)
        {
            int indexOfLast = filePath.LastIndexOf('\\');
            string folderPath = filePath.Substring(0, indexOfLast);
            string fileName = filePath.Substring(indexOfLast + 1);
            return SaveFile(folderPath, fileName);
        }

        public static string SaveFile(string folderPath, string fileName)
        {
            CreateDirectory(folderPath);
            return CreateFile(folderPath, fileName);
        }

        #endregion


        #region Private Methods
        private static void CreateDirectory(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        private static string CreateFile(string folderPath, string fileName)
        {
            string filePath = Path.Combine(folderPath, fileName);

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            return filePath;
        }

        private static void WriteFile(string filePath, string currentContent, bool addToFile)
        {
            using (StreamWriter file = new StreamWriter(filePath, addToFile))
            {
                file.Write(currentContent);
            }
        }

        #endregion

    }
}
