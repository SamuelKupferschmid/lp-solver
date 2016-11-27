using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimplexAlgorithm;

namespace SimplexAlgorithmTests
{
    [TestClass]
    public class VariableTests
    {
        [TestMethod]
        public void ShorthandIsEqual()
        {
            var x1 = new Variable(VariableType.Problem, "x1");
            Assert.AreEqual(x1, Variable.Problem(1));
        }

    }
}