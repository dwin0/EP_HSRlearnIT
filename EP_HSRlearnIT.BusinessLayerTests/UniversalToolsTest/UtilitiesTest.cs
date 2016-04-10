using Microsoft.VisualStudio.TestTools.UnitTesting;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.BusinessLayerTests.UniversalToolsTest
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void ProgessUtilTest()
        {
            Utilities utilities = new Utilities();
            utilities.progress.SaveProgress("Test", 42);

            Assert.AreEqual(42, utilities.progress.GetProgress("Test"));
        }

        [TestMethod]
        public void FileSaverUtilTest()
        {
            Utilities utilities = new Utilities();
            utilities.fileSaver.RemoveSaveFiles();
            utilities.fileSaver.CreateFile("TestFile", 42, "Works!");

            Assert.AreEqual("42Works!", utilities.fileSaver.ReadFile());
        }
    }
}
