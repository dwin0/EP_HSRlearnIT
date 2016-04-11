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
        public void CleanProgressTest()
        {
            Progress progress = new Progress();
            Dictionary<object, object> savedProgress = progress.GetProgress();

            Assert.AreEqual(0, savedProgress.Count);
        }

        [TestMethod]
        public void CleanTryGetProgressTest()
        {
            Progress progress = new Progress();

            Assert.IsNull(progress.GetProgress("empty"));
        }

        [TestMethod]
        public void SaveSingleProgressTest()
        {
            Progress progress = new Progress();
            progress.SaveProgress("progress1", 1);

            Assert.AreEqual(1, progress.GetProgress("progress1"));
        }

        [TestMethod]
        public void SaveDictonaryProgressTest()
        {
            Progress progress = new Progress();
            Dictionary<object, object> progToSave = new Dictionary<object, object>
            {
                {"progressA", "a"},
                {"progressB", "b"}
            };

            progress.SaveProgress(progToSave);

            Assert.AreEqual(progToSave, progress.GetProgress());
        }

        [TestMethod]
        public void UpdateProgressTest()
        {
            Progress progress = new Progress();
            progress.SaveProgress("testProgress", 9);
            progress.SaveProgress("testProgress", 42);

            Assert.AreEqual(42, progress.GetProgress("testProgress"));
        }

    }
}
