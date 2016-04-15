using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using System.Collections.Generic;

namespace EP_HSRlearnIT.BusinessLayerTests.UniversalToolsTest
{
    [TestClass]
    public class ProgressTest
    {

        [TestMethod]
        public void SaveSingleProgressTest()
        {
            Progress.SaveProgress("progress1", 1);

            Assert.AreEqual(1, Progress.GetProgress("progress1"));
        }



        [TestMethod]
        public void SaveMultiProgressTest()
        {
            Progress.SaveProgress("progressX", "x");
            Progress.SaveProgress("progressY", "y");

            Assert.AreEqual("x", Convert.ToString(Progress.GetProgress("progressX")));
            Assert.AreEqual("y", Convert.ToString(Progress.GetProgress("progressY")));
        }

        [TestMethod]
        public void UpdateProgressTest()
        {
            
            Progress.SaveProgress("testProgress1", 9);
            Progress.SaveProgress("testProgress2", 42);

            Assert.AreEqual(42, Progress.GetProgress("testProgress2"));
            Assert.AreEqual(9, Progress.GetProgress("testProgress1"));
        }

        [TestCleanup]
        public void CleanUp()
        {
            Progress.CleanProgress();
        }
    }
}
