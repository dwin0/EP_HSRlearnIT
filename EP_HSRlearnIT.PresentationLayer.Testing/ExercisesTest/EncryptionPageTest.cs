using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using Application = TestStack.White.Application;
using Button = TestStack.White.UIItems.Button;
using TextBox = TestStack.White.UIItems.TextBox;

namespace EP_HSRlearnIT.PresentationLayer.Testing.ExercisesTest
{
    //[TestClass]
    public class EncryptionPageTest
    {
        public string BaseDir => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public string SutPath => Path.Combine(BaseDir, "EP_HSRlearnIT.PresentationLayer.exe");

        [TestMethod]
        public void EncryptionTest()
        {
            //Startup
            var app = Application.Launch(SutPath);
            var window = app.GetWindow("HSRlearnIT", InitializeOption.NoCache);
            window.WaitWhileBusy();

            //navigate to EncryptionPage
            var imgEncrDecr = window.Get<Image>("VerandEntschluesselungsAnwendung");
            imgEncrDecr.Click();

            //input hex-values of test-case 16 of the official AES-GCM specification.
            var ivInput = window.Get<TextBox>("HexIvBox");
            ivInput.Text = "cafebabefacedbaddecaf888";
            var aadInput = window.Get<TextBox>("HexAadBox");
            aadInput.Text = "feedfacedeadbeeffeedfacedeadbeefabaddad2";
            var plaintextInput = window.Get<TextBox>("HexPlaintextBox");
            plaintextInput.Text = "d9313225f88406e5a55909c5aff5269a86a7a9531534f7da2e4c303d8a318a721c3c0c95956809532fcf0e2449a6b525b16aedf5aa0de657ba637b39";
            var keyInput = window.Get<TextBox>("HexPasswordBox");
            keyInput.Text = "feffe9928665731c6d6a8f9467308308feffe9928665731c6d6a8f9467308308";

            //Encrypt and check the result
            var buttenEncrypt = window.Get<Button>("EncryptionButton");
            buttenEncrypt.Click();
            var ciphertextField = window.Get<TextBox>("HexCiphertextBox");
            string ciphertextOutput = ciphertextField.Text;
            var tagField = window.Get<TextBox>("HexTagBox");
            string tagOutput = tagField.Text;

            const string expectedCiphertext = "522dc1f099567d07f47f37a32a84427d643a8cdcbfe5c0c97598a2bd2555d1aa8cb08e48590dbb3da7b08b1056828838c5f61e6393ba7a0abcc9f662";
            const string expectedTag = "76fc6ece0f4e1768cddf8853bb2d551b";

            Assert.AreEqual(expectedCiphertext, ciphertextOutput);
            Assert.AreEqual(expectedTag, tagOutput);

            app.Close();
        }
    }
}
