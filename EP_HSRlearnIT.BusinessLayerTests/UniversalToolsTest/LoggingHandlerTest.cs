// <copyright file="LoggingHandlerTest.cs">Copyright ©  2016</copyright>
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.BusinessLayer.Testing.UniversalToolsTest//.LoggingHandler.Tests
{
   
    [TestClass()]
    public class LoggingHandlerTest
    {
        #region Test1
        [TestMethod()]
        public void TestToWriteIntoLogFileCatchedException()
        {
            String _nreMessage = null;
            String _date = null;
            String _exception = null;
            String _test = null;

            try
            {
                string s = null;
                if (s.Length == 0)
                {
                    Console.WriteLine(s);
                }
            }
            catch (NullReferenceException nre)
            {
                ExceptionLogger.WriteToLogfile(nre.Message, "testMethod");
                _date = DateTime.Now.ToString();
                _nreMessage = nre.Message;
                _exception = "Exception :";
                _test = _exception + _date + ": " + _nreMessage + " " + "testMethod";
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToLogfile(ex.Message, "testMethod");
                Console.WriteLine("Exception occured " + ex.Message);

            }
            finally
            {
                
            }
            Console.ReadLine();

            StreamReader reader = new StreamReader(@"c:\logs\ExceptionLog.log");

            string currentLine = reader.ReadLine();

            while (reader.EndOfStream == false)
            {
                currentLine = reader.ReadLine();
            } 
            
      
            string strToCompare = currentLine;
            string expectedstr = _test;
            Assert.AreEqual(strToCompare, expectedstr);
        }
        #endregion Test1



      

    }

}
