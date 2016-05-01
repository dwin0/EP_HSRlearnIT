using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.BusinessLayerTests.UniversalToolsTest
{
    [TestClass]
    public class ProgressTest
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            Progress.CleanProgress();
        }

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
            Progress.SaveProgress("testProgress1", 42);

            Assert.AreEqual(42, Progress.GetProgress("testProgress1"));
        }

        [TestMethod]
        public void GetAllProgressesTest()
        {
            Progress.SaveProgress(0, 10);
            Progress.SaveProgress(1, 11);
            Progress.SaveProgress(2, 12);

            Dictionary<object, object> allProgesses = Progress.GetProgress();

            for (int i = 0; i <= 2; i++)
            {
                int progress = Convert.ToInt32(allProgesses[i]);
                Assert.AreEqual(i + 10, progress);
            }
        }

        [TestMethod]
        public void GetProgressTest()
        {
            Progress.SaveProgress("Input", 123);
            Progress.SaveProgress("Key", 456);
            Progress.SaveProgress("Output", 789);
            Dictionary<object, object> allProgress = Progress.GetProgress();
            Assert.AreEqual(3, allProgress.Count);
        }


        [TestMethod]
        public void CleanProgressTest()
        {
            Dictionary<object, object> allProgesses1 = Progress.GetProgress();
            Assert.AreEqual(0, allProgesses1.Count);

            Progress.SaveProgress("Progress42", 42);
            Assert.AreEqual(1, allProgesses1.Count);

            Progress.CleanProgress();
            Assert.AreEqual(0, allProgesses1.Count);
        }

        [TestCleanup]
        public void CleanUp()
        {
            Progress.CleanProgress();
        }
    }
}
