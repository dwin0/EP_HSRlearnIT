using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using EP_HSRlearnIT.BusinessLayer.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EP_HSRlearnIT.BusinessLayer.Testing.PersistenceTest
{
    [TestClass]
    public class ExceptionLoggerTest
    {
        [TestMethod]
        public void WriteExceptionIntoLogFile()
        {
            string expectedStr = null;

            try
            {
                string s = null;
                int i = s.Length;
                Console.WriteLine(i);
            }
            catch (NullReferenceException nre)
            {
                ExceptionLogger.WriteToLogfile("This Exception was fired by the unit - test 'WriteExceptionIntoLogFile'", nre.Message, nre.StackTrace);
                //hier könnte der Test aufgrund eines Sekundenwechsels fehlschlagen!
                var date = DateTime.Now.ToString(CultureInfo.CurrentCulture);
                expectedStr =
                    $"Exception: {date}: This Exception was fired by the unit - test \'WriteExceptionIntoLogFile\'{Environment.NewLine}{nre.Message}{Environment.NewLine}{nre.StackTrace}";
            }

            List<string> fileContent = FileManager.ReadAllLines(@"c:\logs\ExceptionLog.log").ToList();
            int fileSize = fileContent.Count;
            string strToCompare = fileContent[fileSize - 3] + Environment.NewLine + fileContent[fileSize - 2] + Environment.NewLine + fileContent[fileSize - 1];
            Assert.AreEqual(expectedStr, strToCompare);
        }
    }
}
