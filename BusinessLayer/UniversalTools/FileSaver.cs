using System;
using System.CodeDom;
using System.IO;
using System.Threading.Tasks;

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
            string output = "";
            Task readTask = Task.Factory.StartNew(() =>
            {
                StreamReader sr = new StreamReader(filePath);
                output = sr.ReadToEnd();
                sr.Close();
            });
            readTask.Wait();

            return output;
        }

        public static long GetSize(String filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }

        public static String SaveFile(String path, String filename)
        {
            Task saveTask = Task.Factory.StartNew(() =>
            {
                CreateDirectory(path);
                CreateFile(filename);
            });
            saveTask.Wait();
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
                File.Create(_filePath);
            }
        }

        private static void WriteFile(String filePath, String currentContent, Boolean addToFile)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                StreamWriter file = new StreamWriter(filePath, addToFile);
                file.Write(currentContent);
                file.Close();
            });
            task.Wait();
        }

        #endregion

    }
}
