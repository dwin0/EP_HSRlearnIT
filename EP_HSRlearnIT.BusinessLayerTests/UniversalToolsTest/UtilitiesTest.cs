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
            Progress.SaveProgress("Test", 42);

            Assert.AreEqual(42, Progress.GetProgress("Test"));
        }

    }
}
