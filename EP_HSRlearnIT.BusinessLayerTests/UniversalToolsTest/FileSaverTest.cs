using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;

namespace EP_HSRlearnIT.BusinessLayer.Testing.UniversalToolsTest
{
    [TestClass()]
    public class FileSaverTests
    {
        FileSaver newFile = new FileSaver();

        [TestMethod()]
        public void CreateFileTest()
        {        
            newFile.CreateFile("AES-GCM.txt", 20, " You are starting with the learntool! ");
            Assert.IsTrue(System.IO.File.Exists(@"c:\temp\HSRlearnIT\Test\AES-GCM.txt"));
        }

        [TestMethod()]
        public void ReadWriteFileTest()
        {
            newFile.CreateFile("RWTest.txt",20 , "");
            newFile.WriteFile(30, "Hello Member!");
            Assert.AreEqual("2030Hello Member!", newFile.ReadFile());
        }

        [TestMethod()]
        public void RemoveSaveFilesTest()
        {
            newFile.CreateFile("RemoveTest.txt", 20, " You are starting with the learntool! ");
            newFile.RemoveSaveFiles();
            Assert.IsFalse(File.Exists(@"c:\temp\HSRlearnIT\Test\RemoveTest.txt"));
        }
    }
}