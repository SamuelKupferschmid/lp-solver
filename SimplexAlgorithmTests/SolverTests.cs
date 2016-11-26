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

        [TestMethod]
        public void CalculatePreprocessedTargetCoefficient()
        {
            var z = Variable.Target();
            var x0 = Variable.Problem(0);
            var x1 = Variable.Problem(1);
            var x2 = Variable.Problem(2);

            var y1 = Variable.Slack(1);
            var y2 = Variable.Slack(2);
            var y3 = Variable.Slack(3);
            var y4 = Variable.Slack(4);

            var initTarget = new Equation(z,new []
            {
                new VariableFactor(x1,1),
                new VariableFactor(x2, 3),  
            }, -229);

            var tableau = new Tableau(new []
            {
                new Equation(y1,new []
                {
                    new VariableFactor(y4,-1),
                    new VariableFactor(x0,2),
                    new VariableFactor(x2,0)   
                }, 9),
                new Equation(y2,new []
                {
                    new VariableFactor(y4,-1),
                    new VariableFactor(x0,2),
                    new VariableFactor(x2,1)
                }, 2),
                new Equation(y3,new []
                {
                    new VariableFactor(y4,0),
                    new VariableFactor(x0,1),
                    new VariableFactor(x2,-1)
                }, 10),
                new Equation(x1,new []
                {
                    new VariableFactor(y4,1),
                    new VariableFactor(x0,-1),
                    new VariableFactor(x2,-1)
                }, 9),
                new Equation(z,new []
                {
                    new VariableFactor(y4,0),
                    new VariableFactor(x0,-1),
                    new VariableFactor(x2,0)
                }, 0)
            });

            var main = Solver.CalculatePreprocessedMainTarget(initTarget, tableau);

            Assert.AreEqual(new Equation(z,new []
            {
                new VariableFactor(y4, 1),
                new VariableFactor(x2, 2),  
            }, -220),main);


        }
    }
}