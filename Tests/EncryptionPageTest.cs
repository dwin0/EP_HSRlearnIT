using System.IO;
using System.Reflection;
using System.Windows.Forms;
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

            //input eingeben, es handelt sich um die Hex Werte des Test Case 14 aus der offiziellen GCM Spezifikation.
            var ivInput = window.Get<TextBox>("HexIvBox");
            ivInput.Text = "000000000000000000000000";
            var plaintextInput = window.Get<TextBox>("HexPlainTextBox");
            plaintextInput.Text = "00000000000000000000000000000000";
            var keyInput = window.Get<TextBox>("HexEncryptionPasswordBox");
            keyInput.Text = "0000000000000000000000000000000000000000000000000000000000000000";

            //verschlüsseln lassen und dann auswerte, ob erwartetes Ergebnis erhalten
            var buttenEncrypt = window.Get<Button>("EncryptionButton");
            buttenEncrypt.Click();
            var ciphertextField = window.Get<TextBox>("HexCipherTextBox");
            string ciphertextOutput = ciphertextField.Text;
            var tagField = window.Get<TextBox>("HexTagBox");
            string tagOutput = tagField.Text;

            Assert.AreEqual("cea7403d4d606b6e074ec5d3baf39d18", ciphertextOutput);
            Assert.AreEqual("d0d1c8a799996bf0265b98b5d48ab919", tagOutput);

            app.Close();
        }
    }
}
