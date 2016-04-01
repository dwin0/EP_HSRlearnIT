using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EP_HSRlearnIT
{
    public class FileSaver
    {
        private String topLevelFolder = @"C:\temp\HSRlearnIT";
        private String folderName = "Test";
        private String pathString;
        private String fileName;
        #region Public Methods

        internal bool SaveFile(String text)
        {
            return false;
        }
        #endregion


        public void CreateFile(String fileName, int status, String currentContent)
        {
            this.fileName = fileName;
            pathString = Path.Combine(topLevelFolder, folderName, fileName);
            if(!File.Exists(pathString))
            {

                pathString = Path.Combine(topLevelFolder, folderName);
                Directory.CreateDirectory(pathString);
                pathString = Path.Combine(pathString, fileName);

                Console.WriteLine("Filepath: {0}\n", pathString);
                FileStream file = File.Create(pathString);
                file.Close();
            }
            WriteFile(status, currentContent);
            String output = ReadFile();
            //MessageBox.Show("The Content of the file is " + output);
        }

        public void WriteFile(int status, String currentContent)
        {
            StreamWriter file = new StreamWriter(pathString, true);
            file.Write(status);
            file.Write(currentContent);
            file.Close();
        }

        public String ReadFile()
        {
            FileInfo info = new FileInfo(pathString);
            StreamReader sr = new StreamReader(pathString);
            String output = sr.ReadToEnd();
            sr.Close();
            return output;
        }

        public void RemoveSaveFiles()
        {
            Directory.Delete(topLevelFolder, true);
        }
    }
}
