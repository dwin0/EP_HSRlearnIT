using System.IO;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public static class FileManager
    {

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

        public static long GetSize(string filePath)
        {
            return new FileInfo(filePath).Length;
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
