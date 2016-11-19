using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimplexAlgorithm;

namespace SimplexAlgorithmTests
{
    [TestClass]
    public class EquationTests
    {
        [TestMethod]
        public void RationByVariable()
        {
            var e = new Equation(Variable.Slack(1), new []
            {
                new VariableFactor(Variable.Problem(1),9), 
                new VariableFactor(Variable.Problem(5),-4)
            }, 40);

            Assert.AreEqual(10, e.Ration(Variable.Problem(5)), double.Epsilon);
        }

        [TestMethod]
        public void SwitchTerm()
        {
            var e = new Equation(Variable.Slack(1), new[]
            {
                new VariableFactor(Variable.Problem(1),-1 ), 
                new VariableFactor(Variable.Problem(2), -1)
            }, 40);

            var e2 = e.Switch(Variable.Problem(1));

            Assert.AreEqual(Variable.Problem(1), e2.LeftTerm);
            Assert.AreEqual(-1, e2[Variable.Slack(1)]);
        }

        [TestMethod]
        public void EqualsIgnoresVarOrdinal()
        {
            var eq1 = new Equation(Variable.Slack(1), new []
            {
                new VariableFactor(Variable.Problem(1), 5),
                new VariableFactor(Variable.Problem(2), 64 ) 
            }, 200);

            //var eq2 = new Equation(eq1.LeftTerm,eq1.);
        }

        [TestMethod]
        public void Resolve()
        {
            var e1 = new Equation(Variable.Slack(2), new[]
            {
                new VariableFactor(Variable.Problem(1),40 ),
                new VariableFactor(Variable.Problem(2),-80)
            }, 80);
            var e2 = new Equation(Variable.Problem(1), new[]
            {
                new VariableFactor(Variable.Slack(1),-1 ),
                new VariableFactor(Variable.Problem(2),-1)
            }, 40);

            var r = e1.Resolve(e2);

            Assert.AreEqual(e1.LeftTerm, r.LeftTerm);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void CheckVariableDuplicatesInRightTerm()
        {
            var e = new Equation(Variable.Problem(0), new []
            {
                new VariableFactor(Variable.Slack(3),34), 
                new VariableFactor(Variable.Slack(3),3) 
            }, 200 );
        }
    }
}