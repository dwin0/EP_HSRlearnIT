using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    /// <summary>
    /// Class helps for the handling with files.
    /// </summary>
    public static class FileManager
    {

        #region Public Methods
        /// <summary>
        /// Method to change all content in a file.
        /// </summary>
        /// <param name="file">File which is to override.</param>
        /// <param name="currentContent">This is the new content for the file.</param>
        public static void OverwriteContent(string file, string currentContent)
        {
            WriteFile(file, currentContent, false);
        }

        /// <summary>
        /// Method to append additional content to a file.
        /// </summary>
        /// <param name="file">File which becomes additional content.</param>
        /// <param name="currentContent">This is the additional content for the file.</param>
        public static void AppendContent(string file, string currentContent)
        {
            WriteFile(file, currentContent, true);
        }

        /// <summary>
        /// Method to exchange the content from a file with a list of new content.
        /// </summary>
        /// <param name="filePath">Path to the file which is change.</param>
        /// <param name="newContent">A list with the new content.</param>
        public static void SwapContents(string filePath, IEnumerable<string> newContent)
        {
            OverwriteContent(filePath, "");
            File.WriteAllLines(filePath, newContent);
        }

        /// <summary>
        /// Method reads the whole file content.
        /// </summary>
        /// <param name="filePath">Path to the file which is read.</param>
        /// <returns>String with the whole content from a file.</returns>
        public static string ReadFullContent(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Method create a list of lines from a file.
        /// </summary>
        /// <param name="filePath">Path to the file which is read.</param>
        /// <returns>A list of all lines from a file.</returns>
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

        /// <summary>
        /// Method constructes a file.
        /// </summary>
        /// <param name="filePath">Path who the file is save.</param>
        /// <returns>The path who the file is save.</returns>
        public static string SaveFile(string filePath)
        {
            int indexOfLast = filePath.LastIndexOf('\\');
            string folderPath = filePath.Substring(0, indexOfLast);
            string fileName = filePath.Substring(indexOfLast + 1);
            return SaveFile(folderPath, fileName);
        }

        /// <summary>
        /// Method check and create the path from the file including the folder.
        /// </summary>
        /// <param name="folderPath">Path from the folder who want to save the file.</param>
        /// <param name="fileName">Name of the file who is create.</param>
        /// <returns>The path who the file is save.</returns>
        public static string SaveFile(string folderPath, string fileName)
        {
            CreateDirectory(folderPath);
            return CreateFile(folderPath, fileName);
        }

        public static void AvoidOverflow(string filePath, long maxSizeLogfile, int rowsToDelete)
        {
            if (GetSize(filePath) >= maxSizeLogfile)
            {
                List<string> lines = ReadAllLines(filePath).ToList();
                lines.RemoveRange(0, rowsToDelete);
                SwapContents(filePath, lines);
                AppendContent(filePath, $"{Environment.NewLine}***" +
                                        $"{Environment.NewLine}The oldest { rowsToDelete} rows were removed." +
                                        $"{Environment.NewLine}***");
            }
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
