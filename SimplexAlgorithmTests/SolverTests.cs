using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimplexAlgorithm;

namespace SimplexAlgorithmTests
{
    [TestClass]
    public class SolverTests
    {
        [TestMethod]
        public void FindInfiniteSolutions()
        {
            var cond = new Equation(Variable.Slack(1), new []
            {
                new VariableFactor(Variable.Problem(1), 2),
                new VariableFactor(Variable.Problem(2), 1)  
            },40);
            var target = new Equation(Variable.Target(), new[]
            {
                new VariableFactor(Variable.Problem(1), 2),
                new VariableFactor(Variable.Problem(2), 1)
            }, 40);

            var s = new Solver(new Equation[]{ cond, target });

            Assert.AreEqual(Solver.ResultType.InfinitResults, s.Solve());
        }
    }
}