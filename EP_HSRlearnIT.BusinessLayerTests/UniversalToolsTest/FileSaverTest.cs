using Microsoft.VisualStudio.TestTools.UnitTesting;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using System.IO;

namespace EP_HSRlearnIT.BusinessLayer.Testing.UniversalToolsTest
{
    [TestClass]
    public class FileSaverTests
    {

        [TestMethod]
        public void CreateFileTest()
        {
            FileSaver.SaveFile(@"c:\temp\HSRlearnIT\Test", "AES-GCM.txt");
            Assert.IsTrue(File.Exists(@"c:\temp\HSRlearnIT\Test\AES-GCM.txt"));
        }

        [TestMethod]
        public void UpdateFileTest()
        {
            string file = FileSaver.SaveFile(@"c:\temp\HSRlearnIT\Test", "UpdateTest.txt");
            //string file = Path.Combine(@"c:\temp\HSRlearnIT\Test", "UpdateTest.txt");
            FileSaver.UpdateFileContent(file, "10 Hello World!");
            FileSaver.UpdateFileContent(file ,"30 Hello Member!");
            Assert.AreEqual("30 Hello Member!", FileSaver.ReadFile(file));
        }

        [TestMethod]
        public void AppendToFileTest()
        {
            string file = FileSaver.SaveFile(@"c:\temp\HSRlearnIT\Test", "AddTest.txt");
            //string file = Path.Combine(@"c:\temp\HSRlearnIT\Test", "AddTest.txt");
            FileSaver.AppendContentToFile(file, "10 Hello World! ");
            FileSaver.AppendContentToFile(file, "30 Hello Member!");
            Assert.AreEqual("10 Hello World! 30 Hello Member!", FileSaver.ReadFile(file));
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            Directory.Delete(@"c:\temp\HSRlearnIT\Test", true);
        }
    }
}