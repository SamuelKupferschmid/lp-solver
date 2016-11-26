using System;
using System.Linq;
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

        [TestMethod]
        public void TrainExample()
        {
            var s = new Solver("Examples\\TrainExample.csv");
            s.Solve();
            //var target = s.Equations.First(e => e.LeftTerm.Type == VariableType.Target);

            //invert target coefficient to transform it into a maximization problem
            //Assert.AreEqual(-229, target.Coefficient);

            //Assert.AreEqual(Solver.ResultType.OneResult, s.Solve());
            //Assert.AreEqual(191,s.ResultFactors.Where(f=>f.Variable == Variable.Target()).First().Factor);
        }
    }
}
