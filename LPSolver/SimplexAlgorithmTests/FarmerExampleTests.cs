using System;
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

            _z = new Equation(Variable.Target(1), new[]
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
            var t1 = new Tableau(new[] { _y1, _y2, _y3, _z });
            t1.Print();
            var t2 = Solver.NextTableau(t1);
            t2.Print();
        }
    }
}
