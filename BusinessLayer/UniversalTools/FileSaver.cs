using System;
using System.IO;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public static class FileSaver
    {
        #region Private Member
        private static String _folderPath;
        private static String _filePath;
        private static String _fileName;
        
        #endregion

        #region Public Methods
        public static void UpdateFileContent(String file, String currentContent)
        {
            WriteFile(file, currentContent, false);
        }

        public static void ContentAddToFile(String file, String currentContent)
        {
            WriteFile(file, currentContent, true);
        }

        public static string ReadFile(String filePath)
        {
            
                StreamReader sr = new StreamReader(filePath);
                string output = sr.ReadToEnd();
                sr.Close();

            return output;
        }

        public static long GetSize(String filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }

        public static String SaveFile(String path, String filename)
        {

                CreateDirectory(path);
                CreateFile(filename);

            return _filePath;
        }

        #endregion


        #region Private Methods
        private static void CreateDirectory(String path)
        {
            _folderPath = path;

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

        }

        private static void CreateFile(String fileName)
        {
            _fileName = fileName;
            _filePath = Path.Combine(_folderPath, _fileName);

            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
            }
        }

        private static void WriteFile(String filePath, String currentContent, Boolean addToFile)
        {
                StreamWriter file = new StreamWriter(filePath, addToFile);
                file.Write(currentContent);
                file.Close();
        }

        #endregion

    }
}
