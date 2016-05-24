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
                ExceptionLogger.WriteToLogfile("This Exception was fired by the unit - test 'TestToWriteIntoLogFileCatchedException", nre.Message, nre.StackTrace);
                //hier könnte der Test aufgrund eines Sekundenwechsels fehlschlagen!
                var date = DateTime.Now.ToString(CultureInfo.CurrentCulture);
                expectedStr =
                    $"Exception: {date}: This Exception was fired by the unit - test \'TestToWriteIntoLogFileCatchedException\' {nre.Message} {nre.StackTrace}";
            }

            using (StreamReader reader = new StreamReader(@"c:\logs\ExceptionLog.log"))
            {
                string strToCompare = reader.ReadLine();

                while (reader.EndOfStream == false)
                {
                    string temp = reader.ReadLine();
                    strToCompare = temp + reader.ReadLine();
                }
                Assert.AreEqual(expectedStr, strToCompare);
            }
        }
    }
}
