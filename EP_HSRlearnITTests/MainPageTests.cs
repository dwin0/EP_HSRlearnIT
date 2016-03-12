using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using EP_HSRlearnIT.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using TestStack.White.Factory;
using TestStack.White.UIItems;

namespace EP_HSRlearnIT.Windows.Tests
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
            var button = window.Get<Button>("OverviewScreen");

            Assert.AreEqual("Übersicht AES GCM", button.Text);
            button.Click();

            app.Close();
        }

        [TestMethod()]
        public void FailTest()
        {
            Assert.Fail();
        }
    }
}