using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using Application = TestStack.White.Application;
using Button = TestStack.White.UIItems.Button;
using TextBox = TestStack.White.UIItems.TextBox;

namespace EP_HSRlearnITTests
{
    //[TestClass]
    public class EncryptionPageTest
    {
        public string BaseDir => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public string SutPath => Path.Combine(BaseDir, $"{nameof(EP_HSRlearnIT)}.exe");

        [TestMethod()]
        public void EncryptionTest()
        {
            //aufstarten
            var app = Application.Launch(SutPath);
            var window = app.GetWindow("MainWindow", InitializeOption.NoCache);
            window.WaitWhileBusy();

            //zu EncryptionPage navigieren
            var imgEncrDecr = window.Get<Image>("EncryptionDecryption");
            imgEncrDecr.Click();

            var buttonEncr = window.Get<Button>("Encryption");
            buttonEncr.Click();

            //input eingeben, es handelt sich um die Hex Werte des Test Cases 16 aus der offiziellen GCM Spezifikation.
            var ivInput = window.Get<TextBox>("HexIvBox");
            ivInput.Text = "cafebabefacedbaddecaf888";
            var aadInput = window.Get<TextBox>("HexAadBox");
            aadInput.Text = "feedfacedeadbeeffeedfacedeadbeefabaddad2";
            var plaintextInput = window.Get<TextBox>("HexPlaintextBox");
            plaintextInput.Text = "d9313225f88406e5a55909c5aff5269a86a7a9531534f7da2e4c303d8a318a721c3c0c95956809532fcf0e2449a6b525b16aedf5aa0de657ba637b39";
            var keyInput = window.Get<TextBox>("HexEncryptionPasswordBox");
            keyInput.Text = "feffe9928665731c6d6a8f9467308308feffe9928665731c6d6a8f9467308308";

            //verschlüsseln lassen und dann auswerte, ob erwartetes Ergebnis erhalten
            var buttenEncrypt = window.Get<Button>("EncryptionButton");
            buttenEncrypt.Click();
            var ciphertextField = window.Get<TextBox>("HexCiphertextBox");
            string ciphertextOutput = ciphertextField.Text;
            var tagField = window.Get<TextBox>("HexTagBox");
            string tagOutput = tagField.Text;

            Assert.AreEqual("522dc1f099567d07f47f37a32a84427d643a8cdcbfe5c0c97598a2bd2555d1aa8cb08e48590dbb3da7b08b1056828838c5f61e6393ba7a0abcc9f662", ciphertextOutput);
            Assert.AreEqual("76fc6ece0f4e1768cddf8853bb2d551b", tagOutput);

            app.Close();
        }
    }
}
