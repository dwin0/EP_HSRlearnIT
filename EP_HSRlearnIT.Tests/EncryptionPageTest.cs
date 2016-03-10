// <copyright file="EncryptionPageTest.cs">Copyright ©  2016</copyright>
using System;
using EP_HSRlearnIT.Windows;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EP_HSRlearnIT.Windows.Tests
{
    /// <summary>This class contains parameterized unit tests for EncryptionPage</summary>
    [PexClass(typeof(EncryptionPage))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class EncryptionPageTest
    {
        /// <summary>Test stub for .ctor()</summary>
        [PexMethod]
        public EncryptionPage ConstructorTest()
        {
            EncryptionPage target = new EncryptionPage();
            return target;
            // TODO: add assertions to method EncryptionPageTest.ConstructorTest()
        }
    }
}
