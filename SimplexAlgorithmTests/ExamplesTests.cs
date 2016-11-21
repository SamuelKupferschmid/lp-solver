using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimplexAlgorithm;

namespace SimplexAlgorithmTests
{
    [TestClass]
    public class ExamplesTests
    {
        [TestMethod]
        public void BasicExample()
        {
            var s = new Solver("Examples\\BasicExample.csv");
            Assert.AreEqual(Solver.ResultType.OneResult, s.Solve());
        }
    }
}
