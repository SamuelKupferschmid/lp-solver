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
            var e = new Equation(Variable.Slack(1),new []
            {
                new Tuple<Variable, double>(Variable.Problem(1),9), 
                new Tuple<Variable, double>(Variable.Problem(5),-4) 
            }, 40);

            Assert.AreEqual(10, e.Ration(Variable.Problem(5)), double.Epsilon);
        }

        [TestMethod]
        public void SwitchTerm()
        {
            var e = new Equation(Variable.Slack(1), new []
            {
                new Tuple<Variable, double>(Variable.Problem(1),-1 ),
                new Tuple<Variable, double>(Variable.Problem(1), -1)  
            },40);

            var e2 = e.Switch(Variable.Problem(1));

            Assert.AreEqual(Variable.Problem(1),e2.LeftTerm);
            Assert.AreEqual(-1,e2.Value(Variable.Slack(1)));
        }
    }
}