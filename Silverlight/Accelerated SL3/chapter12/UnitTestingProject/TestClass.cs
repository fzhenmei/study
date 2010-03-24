using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
//added
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestingProject
{
    [TestClass]
    public class TestClass
    {
        [TestMethod]
        public void TestRangeTooLow()
        {
            Assert.IsFalse(Validators.validateRange(0, 10, 20));
        }

        [TestMethod]
        public void TestRangeAtUpperBound()
        {
            Assert.IsTrue(Validators.validateRange(20, 10, 20));
        }
    }
}
