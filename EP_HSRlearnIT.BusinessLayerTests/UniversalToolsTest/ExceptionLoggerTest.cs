using System;
using System.Globalization;
using System.IO;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EP_HSRlearnIT.BusinessLayer.Testing.UniversalToolsTest
{
    [TestClass]
    public class ExceptionLoggerTest
    {
        [TestMethod()]
        public void TestToWriteIntoLogFileCatchedException()
        {
            string expectedStr = null;

            try
            {
                string s = null;
                int i = s.Length;
            }
            catch (NullReferenceException nre)
            {
                ExceptionLogger.WriteToLogfile(nre.Message, "testMethod");
                //hier könnte der Test aufgrund eines Sekundenwechsels fehlschlagen!
                var date = DateTime.Now.ToString(CultureInfo.CurrentCulture);
                expectedStr = "Exception: " + date + ": " + nre.Message + " testMethod";
            }

            using (StreamReader reader = new StreamReader(@"c:\logs\ExceptionLog.log"))
            {
                string strToCompare = reader.ReadLine();

                while (reader.EndOfStream == false)
                {
                    strToCompare = reader.ReadLine();
                }
                Assert.AreEqual(strToCompare, expectedStr);
            }
        }
    }
}
