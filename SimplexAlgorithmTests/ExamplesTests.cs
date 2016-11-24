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

        [TestMethod]
        public void InfiniteSolutions()
        {
            var s = new Solver("Examples\\InfiniteSolutions.csv");
            Assert.AreEqual(Solver.ResultType.InfinitResults, s.Solve());
        }

        [TestMethod]
        public void NegSchlupf()
        {
            var s = new Solver("Examples\\NegSchlupf.csv");
            Assert.AreEqual(Solver.ResultType.OneResult, s.Solve());
        }

        [TestMethod]
        public void NichtNegativitaet_1()
        {
            var s = new Solver("Examples\\NichtNegativitaet_1.csv");
            Assert.AreEqual(Solver.ResultType.OneResult, s.Solve());
        }

        [TestMethod]
        public void NichtNegativitaet_2()
        {
            var s = new Solver("Examples\\NichtNegativitaet_2.csv");
            Assert.AreEqual(Solver.ResultType.OneResult, s.Solve());
        }

        [TestMethod]
        public void ZweiSaefte()
        {
            var s = new Solver("Examples\\ZweiSaefte.csv");
            Assert.AreEqual(Solver.ResultType.OneResult, s.Solve());
        }
    }
}
