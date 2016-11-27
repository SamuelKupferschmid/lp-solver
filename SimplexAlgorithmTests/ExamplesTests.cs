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
        public void FarmerExample()
        {
            var reader = new CsvReader("Examples\\FarmerExample.csv");
            var result = Solver.Solve(reader);

            Assert.AreEqual(Solver.ResultType.OneResult, result.Type);
        }

        [TestMethod]
        public void BasicExample()
        {
            var reader = new CsvReader("Examples\\BasicExample.csv");
            var result = Solver.Solve(reader);

            Assert.AreEqual(Solver.ResultType.OneResult, result.Type);
        }

        [TestMethod]
        public void InfiniteSolutions()
        {
            var reader = new CsvReader("Examples\\InfiniteSolutions.csv");

            var result = Solver.Solve(reader);
            Assert.AreEqual(Solver.ResultType.InfinitResults, result.Type);
        }

        [TestMethod]
        public void NegSchlupf()
        {
            var reader = new CsvReader("Examples\\NegSchlupf.csv");
            var result = Solver.Solve(reader);
            Assert.AreEqual(Solver.ResultType.OneResult, result.Type);
        }

        [TestMethod]
        public void NichtNegativitaet_1()
        {
            var reader = new CsvReader("Examples\\NichtNegativitaet_1.csv");
            var result = Solver.Solve(reader);
            Assert.AreEqual(Solver.ResultType.OneResult, result.Type);
        }

        [TestMethod]
        public void NichtNegativitaet_2()
        {
            var reader = new CsvReader("Examples\\NichtNegativitaet_2.csv");
            var result = Solver.Solve(reader);
            Assert.AreEqual(Solver.ResultType.OneResult, result.Type);
        }

        [TestMethod]
        public void ZweiSaefte()
        {
            var reader = new CsvReader("Examples\\ZweiSaefte.csv");
            var result = Solver.Solve(reader);
            Assert.AreEqual(Solver.ResultType.OneResult, result.Type);
        }

        [TestMethod]
        public void TrainExample()
        {
            var csv = new CsvReader("Examples\\TrainExample.csv");
            var result = Solver.Solve(csv.HeadVariables, csv.RowVariables, csv.TargetVariable, csv.Matrix, csv.Optimization);

            var zVal = result.Values.Single(v => v.Key.Type == VariableType.Target).Value;

            Assert.AreEqual(191, zVal,0.1);
        }
    }
}
