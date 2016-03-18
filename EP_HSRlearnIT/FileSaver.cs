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
        private Boolean existFile = false;
        private String topLevelFolder = @"c:\temp\HSRlearnIT";
        private String fileName;
        private String pathString;
        //private int status = 0;
        //private String currentContent;
        #region Public Methods

        internal bool SaveFile(String text)
        {
            return false;
        }
        #endregion

        public FileSaver()
        {
        }

        public void CreateFile(String name, int status, String currentContent)
        {
            if(existFile == false)
            {
                pathString = System.IO.Path.Combine(topLevelFolder, name);
                System.IO.Directory.CreateDirectory(pathString);
                fileName = System.IO.Path.GetRandomFileName();
                pathString = System.IO.Path.Combine(pathString, fileName);
                Console.WriteLine("Filepath: {0}\n", pathString);
                System.IO.FileStream file = System.IO.File.Create(pathString);
                file.Close();
                existFile = true;
                WriteFile(status, currentContent);
                ReadFile();
            }
        }

        public void WriteFile(int status, String currentContent)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(pathString, true);
            file.Write(status);
            file.Write(currentContent);
            file.Close();
        }

        public void ReadFile()
        {
            FileInfo info = new FileInfo(pathString);
            StreamReader sr = new StreamReader(pathString);
            String output = sr.ReadToEnd();
            MessageBox.Show("The Content of the file " + info.Name + " is " + output);
            sr.Close();
        }

        public void RemoveSaveFiles()
        {
            System.IO.Directory.Delete(topLevelFolder);
        }
    }
}
