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
        public void SaveDictonaryProgressTest()
        {
            Dictionary<object, object> progToSave = new Dictionary<object, object>
            {
                {"progressA", "a"},
                {"progressB", "b"}
            };

            Progress.SaveProgress(progToSave);

            Assert.AreEqual(progToSave, Progress.GetProgress());
        }

        [TestMethod]
        public void UpdateProgressTest()
        {
            
            Progress.SaveProgress("testProgress", 9);
            Progress.SaveProgress("testProgress", 42);

            Assert.AreEqual(42, Progress.GetProgress("testProgress"));
        }

    }
}
