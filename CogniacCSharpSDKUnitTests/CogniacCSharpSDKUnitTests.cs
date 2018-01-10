using Microsoft.VisualStudio.TestTools.UnitTesting;
using CogniacCSharpSDK;
using System;

namespace CogniacCSharpSDKUnitTests
{
    [TestClass]
    public class CogniacCSharpSDKUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CogConnUserPassTest()
        {
            // Supplied user name and password only are correct
            CogniacConnection cc = new CogniacConnection("al@ieee.org", "SomePassword9999");
            Assert.IsNotNull(cc);
        }
    }
}
