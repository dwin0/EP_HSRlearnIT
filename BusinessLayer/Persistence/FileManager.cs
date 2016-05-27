using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EP_HSRlearnIT.BusinessLayer.Persistence
{
    /// <summary>
    /// Class which helps with the handling of files.
    /// </summary>
    public static class FileManager
    {

        #region Public Methods
        /// <summary>
        /// Method to change all content in a file.
        /// </summary>
        /// <param name="filePath">Path to the file which is to override.</param>
        /// <param name="currentContent">This is the new content for the file.</param>
        public static void OverwriteContent(string filePath, string currentContent)
        {
            WriteFile(filePath, currentContent, false);
        }

        /// <summary>
        /// Method to append additional content to a file.
        /// </summary>
        /// <param name="filePath">Path to the file which will get additional content.</param>
        /// <param name="currentContent">This is the additional content for the file.</param>
        public static void AppendContent(string filePath, string currentContent)
        {
            WriteFile(filePath, currentContent, true);
        }

        /// <summary>
        /// Method to read the whole file content.
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
        /// Method which creates a list of lines from a file.
        /// </summary>
        /// <param name="filePath">Path to the file which is read.</param>
        /// <returns>A list of all lines from a file.</returns>
        public static IEnumerable<string> ReadAllLines(string filePath)
        {
            return File.ReadLines(filePath);
        }

        public static bool IsExisting(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// Method which constructs a file.
        /// </summary>
        /// <param name="filePath">Path where the file will be saved.</param>
        /// <returns>The path where the file is saved.</returns>
        public static string SaveFile(string filePath)
        {
            int indexOfLast = filePath.LastIndexOf('\\');
            string folderPath = filePath.Substring(0, indexOfLast);
            string fileName = filePath.Substring(indexOfLast + 1);
            return SaveFile(folderPath, fileName);
        }

        /// <summary>
        /// Method which checks and creates the folder and the file.
        /// </summary>
        /// <param name="folderPath">Path of the folder where the file will be saved.</param>
        /// <param name="fileName">Name of the file which is created.</param>
        /// <returns>The path where the file is saved.</returns>
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

        /// <summary>
        /// Method to exchange the content from a file with a list of new content.
        /// </summary>
        /// <param name="filePath">Path to the file which is to change.</param>
        /// <param name="newContent">A list with the new content.</param>
        public static void SwapContents(string filePath, IEnumerable<string> newContent)
        {
            OverwriteContent(filePath, "");
            File.WriteAllLines(filePath, newContent);
        }

        #endregion


        #region Private Methods

        private static long GetSize(string filePath)
        {
            return new FileInfo(filePath).Length;
        }

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
