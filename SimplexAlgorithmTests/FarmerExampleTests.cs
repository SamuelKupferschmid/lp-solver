using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimplexAlgorithm;

namespace SimplexAlgorithmTests
{
    [TestClass]
    public class FarmerExampleTests
    {
        private Equation _y1;
        private Equation _y2;
        private Equation _y3;
        private Equation _z;

        [TestInitialize]
        public void Setup()
        {
            _y1 = new Equation(Variable.Slack(1), new[]
               {
                new VariableFactor(Variable.Problem(1),-1),
                new VariableFactor(Variable.Problem(2),-1),
            }, 40);

            _y2 = new Equation(Variable.Slack(2), new[]
               {
                new VariableFactor(Variable.Problem(1),-40),
                new VariableFactor(Variable.Problem(2),-120),
            }, 2400);

            _y3 = new Equation(Variable.Slack(3), new[]
               {
                new VariableFactor(Variable.Problem(1),-7),
                new VariableFactor(Variable.Problem(2),-12),
            }, 312);

            _z = new Equation(Variable.Target(), new[]
               {
                new VariableFactor(Variable.Problem(1),100),
                new VariableFactor(Variable.Problem(2),250),
            }, 0);
        }

        [TestMethod]
        public void FindPivot()
        {
            var t = new Tableau(new []{_y1,_y2,_y3, _z});
            Variable head;
            Variable row;
            Assert.IsTrue(t.FindPivot(out head, out row));
        }

        [TestMethod]
        public void NextTableau()
        {
            Variable head;
            Variable row;

            var t1 = new Tableau(new[] { _y1, _y2, _y3, _z });
            t1.FindPivot(out head, out row);
            var t2 = Solver.NextTableau(t1, head, row);
            Assert.AreEqual(4000,t2.TargetEquation.Coefficient);
            t2.FindPivot(out head, out row);
            var t3 = Solver.NextTableau(t2, head, row);
            Assert.AreEqual(4960,t3.TargetEquation.Coefficient);
            t3.FindPivot(out head, out row);
            var t4 = Solver.NextTableau(t3, head, row);
            Assert.AreEqual(5400,t4.TargetEquation.Coefficient);
            Assert.IsFalse(t4.FindPivot(out head,out row));
        }

        [TestMethod]
        public void Solve()
        {
            var s = new Solver(new []{_y1, _y2, _y3, _z});
            Assert.AreEqual(Solver.ResultType.OneResult, s.Solve());

            Assert.AreEqual(24,s.ResultFactors.Where(f=>f.Variable == Variable.Problem(1)).First().Factor);
            Assert.AreEqual(12,s.ResultFactors.Where(f=>f.Variable == Variable.Problem(2)).First().Factor);
            Assert.AreEqual(5400,s.ResultFactors.Where(f=>f.Variable == Variable.Target()).First().Factor);
        }
    }
}
