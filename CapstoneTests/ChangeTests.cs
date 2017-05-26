using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.Classes;

namespace CapstoneTests
{
    [TestClass]
    public class ChangeTests
    {
        [TestMethod]
        public void GetChangeMethodTests()
        {
            Change change = new Change();
            Assert.AreEqual($"Your change is {30} quarters, {0} dimes, {0} nickels", change.GetChange((decimal)10.00, (decimal)2.50));
            Assert.AreEqual($"Your change is {2} quarters, {1} dimes, {1} nickels", change.GetChange((decimal)3.00, (decimal)2.35));
            Assert.AreEqual($"Your change is {0} quarters, {0} dimes, {0} nickels", change.GetChange((decimal)2.50, (decimal)2.50));
        }
    }
}
