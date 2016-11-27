using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimplexAlgorithm
{
    public class Tableau
    {
        public Variable[] HeadVariables { get; private set; }
        public Variable[] RowVariables { get; private set; }
        public Variable TargetVariable { get; private set; }
        public double[][] Matrix { get; private set; }
        public int TargetIndex { get; private set; }
        public int CoefficientIndex { get; private set; }

        public Tableau(Variable[] headVariables, Variable[] rowVariables, Variable targetVariable, double[][] matrix)
        {
            HeadVariables = headVariables;
            RowVariables = rowVariables;
            TargetVariable = targetVariable;
            Matrix = matrix;

            TargetIndex = rowVariables.Length;
            CoefficientIndex = headVariables.Length;
        }

        public Tableau(double[][] matrix)
        {
            Matrix = matrix;

            HeadVariables = new Variable[matrix[0].Length - 1];

            for (var i = 0; i < HeadVariables.Length; i++)
                HeadVariables[i] = Variable.Problem(i + 1);

            RowVariables = new Variable[matrix.Length - 1];

            for (var i = 0; i < RowVariables.Length; i++)
                RowVariables[i] = Variable.Slack(i + 1);

            TargetIndex = RowVariables.Length;
            CoefficientIndex = HeadVariables.Length;

            TargetVariable = Variable.Target();
        }

        public bool Pivot(out int row, out int head)
        {
            head = -1;
            row = -1;

            var minVal = double.MaxValue;
            var minIndex = -1;

            for (var i = 0; i < CoefficientIndex; i++)
            {
                if (Matrix[TargetIndex][i] < minVal && Matrix[TargetIndex][i] > 0)
                {
                    minVal = Matrix[TargetIndex][i];
                    minIndex = i;
                }
            }

            if (minIndex == -1)
                return false;

            head = minIndex;

            minVal = double.MaxValue;

            for (var i = 0; i < TargetIndex; i++)
            {
                var ration = Math.Abs(Matrix[i][CoefficientIndex] / Matrix[i][head]);

                if (ration < minVal)
                {
                    minVal = ration;
                    row = i;
                }
            }

            return true;
        }

        public void Switch(int row, int head)
        {
            var pFact = 1 / Matrix[row][head];

            for (var i = 0; i < Matrix[row].Length; i++)
                Matrix[row][i] = (i == head ? -1 : Matrix[row][i]) * -pFact;


            for (var i = 0; i < Matrix.Length; i++)
            {
                if (i == row)
                    continue;

                var rFact = Matrix[i][head];

                //set new slack variable value
                Matrix[i][head] *= pFact;

                for (var j = 0; j < Matrix[i].Length; j++)
                {
                    if (j != head)
                        Matrix[i][j] += rFact * Matrix[row][j];
                }
            }

            //switch row/head variables
            var rVar = RowVariables[row];
            RowVariables[row] = HeadVariables[head];
            HeadVariables[head] = rVar;

        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("\t");

            for (var i = 0; i < HeadVariables.Length; i++)
                sb.Append($"{HeadVariables[i]}\t");

            sb.Append("c\n");

            for (var i = 0; i < Matrix.Length; i++)
            {
                sb.Append((i == Matrix.Length - 1 ? TargetVariable.Name : RowVariables[i].Name) + "\t");

                for (var j = 0; j <= HeadVariables.Length; j++)
                {
                    sb.Append($"{Matrix[i][j]}\t");
                }
                sb.Append("\n");
            }


            return sb.ToString();
        }
    }
}
