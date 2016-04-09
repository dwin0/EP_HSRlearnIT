using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;

namespace EP_HSRlearnIT.PresentationLayer.Testing
{
    [TestClass()]
    public class MainPageTests
    {
        public string BaseDir => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public string SutPath => Path.Combine(BaseDir, $"{nameof(EP_HSRlearnIT)}.exe");
        
        [TestMethod()]
        public void MainPageTest()
        {
            var app = Application.Launch(SutPath);
            var window = app.GetWindow("MainWindow", InitializeOption.NoCache);
            window.WaitWhileBusy();
            var button = window.Get<Button>("OverviewScreen");

            Assert.AreEqual("Übersicht AES GCM", button.Text);
            button.Click();

            app.Close();
        }

        [TestMethod()]
        public void OKTest()
        {
            Assert.IsTrue(true);
        }
        
    }
}