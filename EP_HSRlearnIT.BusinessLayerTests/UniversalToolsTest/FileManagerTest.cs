using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EP_HSRlearnIT.BusinessLayer.UniversalTools;
using System.IO;
using System.Linq;

namespace EP_HSRlearnIT.BusinessLayer.Testing.UniversalToolsTest
{
    [TestClass]
    public class FileManagerTest
    {

        [TestMethod]
        public void CreateFileTwoParametersTest()
        {
            FileManager.SaveFile(@"c:\temp\HSRlearnIT\Test", "AES-GCM.txt");
            Assert.IsTrue(FileManager.IsExist(@"c:\temp\HSRlearnIT\Test\AES-GCM.txt"));
        }

        [TestMethod]
        public void CreateFileWithFilepathTest()
        {
            FileManager.SaveFile(@"c:\temp\HSRlearnIT\Test\OneParameterTest.txt");
            Assert.IsTrue(FileManager.IsExist(@"c:\temp\HSRlearnIT\Test\OneParameterTest.txt"));
        }

        [TestMethod]
        public void UpdateContentTest()
        {
            string file = FileManager.SaveFile(@"c:\temp\HSRlearnIT\Test\UpdateTest.txt");
            FileManager.UpdateContent(file, "10 Hello World!");
            FileManager.UpdateContent(file ,"30 Hello Member!");
            Assert.AreEqual("30 Hello Member!", FileManager.ReadFullContent(file));
        }

        [TestMethod]
        public void AppendContentTest()
        {
            string file = FileManager.SaveFile(@"c:\temp\HSRlearnIT\Test\AppendTest.txt");
            FileManager.AppendContent(file, "10 Hello World! ");
            FileManager.AppendContent(file, "30 Hello Member!");
            Assert.AreEqual("10 Hello World! 30 Hello Member!", FileManager.ReadFullContent(file));
        }

        [TestMethod]
        public void SwapContentsTest()
        {
            string file = FileManager.SaveFile(@"c:\temp\HSRlearnIT\Test\SwapTest.txt");
            FileManager.AppendContent(file, $"Hello World!{Environment.NewLine}");
            FileManager.AppendContent(file, $"Hello Europe!{Environment.NewLine}");
            FileManager.AppendContent(file, $"Hello Switzerland!{Environment.NewLine}");
            string[] newList = { "Bye World!", "Bye Europe!", "Bye Switzerland!" };
            FileManager.SwapContents(file, newList);
            Assert.AreEqual($"Bye World!{Environment.NewLine}Bye Europe!{Environment.NewLine}Bye Switzerland!{Environment.NewLine}", FileManager.ReadFullContent(file));
        }

        [TestMethod]
        public void ReadAllLinesTest()
        {
            string file = FileManager.SaveFile(@"c:\temp\HSRlearnIT\Test\ReadAllLinesTest.txt");
            FileManager.AppendContent(file, $"Hello World!{Environment.NewLine}");
            FileManager.AppendContent(file, $"Hello Europe!{Environment.NewLine}");
            FileManager.AppendContent(file, $"Hello Switzerland!{Environment.NewLine}");
            string[] newList = { "Hello World!", "Hello Europe!", "Hello Switzerland!" };
            List<string> lines = FileManager.ReadAllLines(file).ToList();
            int i = 0;
            foreach (string s in newList)
            {
                Assert.AreEqual(s, lines[i]);
                i++;
            }
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            Directory.Delete(@"c:\temp\HSRlearnIT\Test", true);
        }
    }
}