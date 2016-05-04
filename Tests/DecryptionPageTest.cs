using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;

namespace EP_HSRlearnITTests
{
    //[TestClass]
    public class DecryptionPageTest
    {
        public string BaseDir => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public string SutPath => Path.Combine(BaseDir, $"{nameof(EP_HSRlearnIT)}.exe");

        [TestMethod()]
        public void DecryptionTest()
        {
            //aufstarten
            var app = Application.Launch(SutPath);
            var window = app.GetWindow("MainWindow", InitializeOption.NoCache);
            window.WaitWhileBusy();

            //zu DecryptionPage navigieren
            var imgEncrDecr = window.Get<Image>("EncryptionDecryption");
            imgEncrDecr.Click();

            var buttonDecr = window.Get<Button>("Decryption");
            buttonDecr.Click();

            //input eingeben, es handelt sich um die Hex Werte des Test Case 14 aus der offiziellen GCM Spezifikation.
            var ciphertextInput = window.Get<TextBox>("HexCiphertextBox");
            ciphertextInput.Text = "cea7403d4d606b6e074ec5d3baf39d18";
            var ivInput = window.Get<TextBox>("HexIvBox");
            ivInput.Text = "000000000000000000000000";
            var tagInput = window.Get<TextBox>("HexTagBox");
            tagInput.Text = "d0d1c8a799996bf0265b98b5d48ab919";
            var keyInput = window.Get<TextBox>("HexDecryptionPasswortBox");
            keyInput.Text = "0000000000000000000000000000000000000000000000000000000000000000";

            //entschlüsseln lassen und dann auswerten, ob erwartetes Ergebnis erhalten
            var buttenEncrypt = window.Get<Button>("DecryptionButton");
            buttenEncrypt.Click();
            var plaintextField = window.Get<TextBox>("HexPlaintextBox");
            string plaintextOutput = plaintextField.Text;

            Assert.AreEqual("00000000000000000000000000000000", plaintextOutput);

            app.Close();
        }
    }
}
