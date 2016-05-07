using Microsoft.VisualStudio.TestTools.UnitTesting;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using System.IO;

namespace EP_HSRlearnIT.BusinessLayer.Testing.UniversalToolsTest
{
    [TestClass]
    public class FileManagerTest
    {

        [TestMethod]
        public void CreateFileTest()
        {
            FileManager.SaveFile(@"c:\temp\HSRlearnIT\Test", "AES-GCM.txt");
            Assert.IsTrue(File.Exists(@"c:\temp\HSRlearnIT\Test\AES-GCM.txt"));
        }

        [TestMethod]
        public void UpdateFileTest()
        {
            string file = FileManager.SaveFile(@"c:\temp\HSRlearnIT\Test", "UpdateTest.txt");
            FileManager.UpdateContent(file, "10 Hello World!");
            FileManager.UpdateContent(file ,"30 Hello Member!");
            Assert.AreEqual("30 Hello Member!", FileManager.ReadFullContent(file));
        }

        [TestMethod]
        public void AppendToFileTest()
        {
            string file = FileManager.SaveFile(@"c:\temp\HSRlearnIT\Test", "AppendTest.txt");
            FileManager.AppendContent(file, "10 Hello World! ");
            FileManager.AppendContent(file, "30 Hello Member!");
            Assert.AreEqual("10 Hello World! 30 Hello Member!", FileManager.ReadFullContent(file));
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            Directory.Delete(@"c:\temp\HSRlearnIT\Test", true);
        }
    }
}