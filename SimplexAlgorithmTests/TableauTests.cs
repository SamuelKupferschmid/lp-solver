using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimplexAlgorithm;

namespace SimplexAlgorithmTests
{
    [TestClass()]
    public class TableauTests
    {
        [TestMethod()]
        public void HandleInequalityOperator()
        {
            var smeq = new Solver(GetStreamReader(@"1;1;
max;3;0
;
<=;2;100")).Equations.First();

            Assert.AreEqual(new Equation(Variable.Slack(1), new []
            {
                new VariableFactor(Variable.Problem(1), -2) 
            },100  ), smeq);
        }


        private static StreamReader GetStreamReader(string content)
        {
            var ms = new MemoryStream(Encoding.Default.GetBytes(content));

            return new StreamReader(ms);
        }
    }
}