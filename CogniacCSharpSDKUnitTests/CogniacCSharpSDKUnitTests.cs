using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cogniac;
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
            Connection cc = new Connection("al@ieee.org", "SomePassword9999");
            Assert.IsNotNull(cc);
        }
    }
}
