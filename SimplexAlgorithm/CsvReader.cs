using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimplexAlgorithm
{
    public class CsvReader
    {
        public readonly Variable[] HeadVariables;
        public readonly Variable[] RowVariables;
        public readonly Variable TargetVariable;
        public readonly double[][] Matrix;
        public readonly Solver.Optimization Optimization;

        public CsvReader(string filename)
            : this(new StreamReader(File.OpenRead(filename)))
        { }

        public CsvReader(StreamReader stream)
        {
            var data = ReadAllLines(stream).Select(l => l.Split(';').ToArray()).ToArray();

            var vCnt = int.Parse(data[0][0]);
            var cCnt = int.Parse(data[0][1]);
            var cOffset = 3;

            //increase for equations
            for (var i = cCnt + cOffset - 1; i > cOffset; i--)
            {
                if (data[i][0] == "=")
                    cCnt++;
            }

            var nonNegCnt = 0;
            for (var i = 0; i < vCnt; i++)
            {
                if (data[2][i].Contains("true"))
                    nonNegCnt++;
            }

            Optimization = data[1][0].Contains("max") ? Solver.Optimization.Maximize : Solver.Optimization.Minimize;

            TargetVariable = Variable.Target();

            HeadVariables = new Variable[vCnt];

            for (var i = 0; i < HeadVariables.Length; i++)
                HeadVariables[i] = Variable.Problem(i + 1);

            RowVariables = new Variable[cCnt];

            for (var i = 0; i < RowVariables.Length; i++)
                RowVariables[i] = Variable.Slack(i + 1);

            Matrix = new double[cCnt + nonNegCnt + 1][];

            var addedInversions = 0;

            for (var i = 0; i < cCnt; i++)
            {
                Matrix[i] = new double[vCnt + 1];

                var lineIndex = i + cOffset - addedInversions;

                var op = data[lineIndex][0];

                for (var j = 0; j <= vCnt; j++)
                {
                    var val = double.Parse(data[lineIndex][j + 1]);

                    //negate variable values if necessary
                    if (op == "<=" && j < vCnt)
                        val = -val;

                    Matrix[i][j] = val;
                }

                if (op == "=")
                {
                    i++;
                    addedInversions++;

                    Matrix[i] = new double[vCnt + 1];
                    Array.Copy(Matrix[i - 1], Matrix[i], vCnt + 1);
                    Matrix[i][vCnt] = -Matrix[i][vCnt];
                }
            }

            var index = cCnt;
            for(var i = 0; i < vCnt; i++)
            {
                if (data[2][i].Contains("true"))
                {
                    Matrix[index] = new double[vCnt + 1];
                    Matrix[index][i] = 1;
                    index++;
                }
            }

            Matrix[cCnt + nonNegCnt] = new double[vCnt + 1];

            for (var i = 0; i <= vCnt; i++)
            {
                var val = double.Parse(data[1][i + 1]);
                Matrix[cCnt + nonNegCnt][i] = val;
            }


        }

        private IEnumerable<string> ReadAllLines(StreamReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}
