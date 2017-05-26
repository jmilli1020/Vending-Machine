using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTests
{
    [TestClass]
    public class UserInterfaceTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            MainMenu()
                DisplayItems()
                MakePurchase()
        }
    }
}
