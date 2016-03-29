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
        string path = @"c:\logs\log.txt";
        StreamWriter writer;

        public void writeToLogFile(string exmsg, string sourceMethod)
        {
            writer = new StreamWriter(path);
            writer.WriteLine("Exception :" + DateTime.Now.ToString() + " " + exmsg +  " " + sourceMethod);
            writer.Close();
        }


    }
}
