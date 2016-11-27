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
        public void PrettyPrintTableau()
        {
            var x1 = Variable.Problem(1);
            var x2 = Variable.Problem(2);

            var y1 = Variable.Slack(1);
            var z = Variable.Target();

            var t = new Tableau(new[] { x1, x2 }, new[] { y1 }, z, new[]
            {
                new []{1d,2d,3d},
                new [] {2d,0d,5d}
            });

            Assert.AreEqual("\tx1\tx2\tc\ny1\t1\t2\t3\t\nz\t2\t0\t5\t\n", t.ToString());
        }

        [TestMethod]
        public void SwitchOverMinus1()
        {
            var t = new Tableau(new[]
            {
                new[] {-1d,-1d,40},
                new[] {-40d,-120d,2400},
                new[] {-7d,-12d,312},
                new[] {100d,250d,0}
            });

            t.Switch(0, 0);
            Assert.AreEqual("y1", t.HeadVariables[0].Name);
            Assert.AreEqual(4000, t.Matrix[t.TargetIndex][t.CoefficientIndex]);

        }

        [TestMethod]
        public void SwitchOverMinus5()
        {
            var t = new Tableau(new[]
            {
                new[] {-1d,-1d,40},
                new[] {40d,-80d,800},
                new[] {7d,-5d,32},
                new[] {-100d,150d,4000}
            });

            t.Switch(2, 1);
            Assert.AreEqual(7d / 5, t.Matrix[2][0], 0.01);
            Assert.AreEqual(-1d / 5, t.Matrix[2][1], 0.01);
            Assert.AreEqual(32d / 5, t.Matrix[2][2], 0.01);


            Assert.AreEqual(4960, t.Matrix[t.TargetIndex][t.CoefficientIndex]);

        }
    }
}