using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EP_HSRlearnIT
{
    public class LoggingHandler
    {
        static string path = @"c:\logs\log.txt";
        StreamWriter writer;
        

        private void createIfMissing(string path)
        {
            LoggingHandler.path = path; 
            bool folderExists = Directory.Exists(path);
            DirectoryInfo dinf = new DirectoryInfo(path);
            if (!folderExists)
            {
                Directory.CreateDirectory(dinf.Parent.FullName);
            }
        }

        public void writeToLogFile(string exmsg, string sourceMethod)
        {
            createIfMissing(path);
            writer = new StreamWriter(path, true);
            writer.WriteLine("Exception :" + DateTime.Now.ToString() + ": " + exmsg +  " " + sourceMethod);
            writer.Close();
        }

       public static void unhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            StreamWriter writer2 = new StreamWriter(path, true);
            writer2.WriteLine("Exception: " + DateTime.Now.ToString() + ": " + e.ExceptionObject.ToString());
            writer2.Close();
            Environment.Exit(1);
        }
        

    }
}
