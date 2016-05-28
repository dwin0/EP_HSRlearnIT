using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using TestStack.White.Factory;
using Application = TestStack.White.Application;
using Button = TestStack.White.UIItems.Button;

namespace EP_HSRlearnIT.PresentationLayer.Testing
{
    //[TestClass]
    public class MainPageTests
    {
        public string BaseDir => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public string SutPath => Path.Combine(BaseDir, "EP_HSRlearnIT.PresentationLayer.exe");
        
        [TestMethod]
        public void MainPageTest()
        {
            //Load MainPage in MainWindow
            var app = Application.Launch(SutPath);
            var window = app.GetWindow("HSRlearnIT", InitializeOption.NoCache);
            window.WaitWhileBusy();

            //OpenMenu
            var menuButton = window.Get<Button>("MenuButton");
            menuButton.Click();

            window.Click();

            Assert.AreEqual("HSRlearnIT", window.Name);

            app.Close();
        }
    }
}