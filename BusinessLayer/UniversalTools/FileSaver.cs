using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public static class FileSaver
    {
        /*
        #region Private Member
        private static String _folderPath;
        private static String _filePath;
        private static String _fileName;
        
        #endregion
        */

        #region Public Methods
        public static void UpdateFileContent(string file, string currentContent)
        {
            WriteFile(file, currentContent, false);
        }

        public static void AppendContentToFile(string file, string currentContent)
        {
            WriteFile(file, currentContent, true);
        }

        public static string ReadFile(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                return sr.ReadToEnd();
            }
        }
        /*
        public static string ReadFile(string filePath)
        {
            
                StreamReader sr = new StreamReader(filePath);
                string output = sr.ReadToEnd();
                sr.Close();

            return output;
        } */

        public static long GetSize(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }

        public static string CreateFile(string folderPath, string fileName)
        {
            CreateDirectory(folderPath);
            return _CreateFile(folderPath, fileName);
        }
        /*
        public static string SaveFile(string path, string filename)
        {

                CreateDirectory(path);
                CreateFile(filename);

            return _filePath;
        }*/

        public static void AvoidOverflow(string filePath, long maxFileSize, int numberOfRowsToDelete)
        {
            if (FileSaver.GetSize(filePath) >= maxFileSize)
            {
                List<string> lines = File.ReadAllLines(filePath).ToList();
                lines.RemoveRange(0, numberOfRowsToDelete);
                File.WriteAllLines(filePath, lines);
                FileSaver.AppendContentToFile(filePath, "The oldest " + numberOfRowsToDelete + " lines are removed. \n");
            }
        }

        #endregion


        #region Private Methods
        private static void CreateDirectory(string folderPath)
        {
            //_folderPath = path;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        private static string _CreateFile(string folderPath, string fileName)
        {
            string filePath = Path.Combine(folderPath, fileName);

            if (!File.Exists(filePath))
            {
                using (File.Create(filePath)) { }
            }

            return filePath;
        }
        /*
        private static vostid CreateFile(string fileName)
        {
            _fileName = fileName;
            _filePath = Path.Combine(_folderPath, _fileName);

            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
            }
        } */

        private static void WriteFile(string filePath, string currentContent, bool addToFile)
        {
            using (StreamWriter file = new StreamWriter(filePath, addToFile))
            {
                file.Write(currentContent);
            }
            /*
                StreamWriter file = new StreamWriter(filePath, addToFile);
                file.Write(currentContent);
                file.Close();
            */
        }

        #endregion

    }
}
