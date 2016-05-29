using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EP_HSRlearnIT.BusinessLayer.Persistence;

namespace EP_HSRlearnIT.BusinessLayer.Testing.PersistenceTest
{
    [TestClass]
    public class FileManagerTest
    {

        [TestMethod]
        public void SaveFileTwoParametersTest()
        {
            FileManager.SaveFile(@"c:\temp\HSRlearnIT\Test", "AES-GCM.txt");
            Assert.IsTrue(File.Exists(@"c:\temp\HSRlearnIT\Test\AES-GCM.txt"));
        }

        [TestMethod]
        public void SaveFileWithFilepathTest()
        {
            FileManager.SaveFile(@"c:\temp\HSRlearnIT\Test\OneParameterTest.txt");
            Assert.IsTrue(File.Exists(@"c:\temp\HSRlearnIT\Test\OneParameterTest.txt"));
        }

        [TestMethod]
        public void OverwriteContentTest()
        {
            string file = FileManager.SaveFile(@"c:\temp\HSRlearnIT\Test\UpdateTest.txt");
            FileManager.OverwriteContent(file, "10 Hello World!");
            FileManager.OverwriteContent(file ,"30 Hello Member!");
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

        [TestMethod]
        public void IsExistTest()
        {
            FileManager.SaveFile(@"c:\temp\HSRlearnIT\Test\ExistTest.txt");
            Assert.IsTrue(FileManager.IsExist(@"c:\temp\HSRlearnIT\Test\ExistTest.txt"));
        }

        [TestMethod]
        public void AvoidOverflowTest()
        {
            string fileName = @"c:\temp\HSRlearnIT\Test\OverflowTest.txt";
            var sizeInMb = 5;
            byte[] data = new byte[(sizeInMb+1) * 1024 * 1024];

            Random random = new Random();
            random.NextBytes(data);
            File.WriteAllBytes(fileName, data);

            FileManager.AvoidOverflow(fileName, sizeInMb, 20, "");
            List<string> fileContent = FileManager.ReadAllLines(fileName).ToList();
            string lastSecondLine = fileContent[fileContent.Count - 2];
            Assert.AreEqual("The oldest 20 rows were removed.", lastSecondLine);
        }

        [TestMethod]
        public void AvoidOverflowOwnMessageTest()
        {
            string fileName = @"c:\temp\HSRlearnIT\Test\OverflowOwnMessageTest.txt";
            int deleteRows = 200;
            string message = $"Your file is full.{Environment.NewLine}I delete {deleteRows} lines.";
            var sizeInMb = 5;

            byte[] data = new byte[(sizeInMb + 1) * 1024 * 1024];
            Random random = new Random();
            random.NextBytes(data);
            File.WriteAllBytes(fileName, data);

            FileManager.AvoidOverflow(fileName, sizeInMb, deleteRows, message);

            List<string> fileContent = FileManager.ReadAllLines(fileName).ToList();
            int numberOfLines = fileContent.Count;
            string lastLines = fileContent[numberOfLines - 2] + Environment.NewLine + fileContent[numberOfLines - 1];

            Assert.AreEqual(message, lastLines);
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            Directory.Delete(@"c:\temp\HSRlearnIT\Test", true);
        }
    }
}