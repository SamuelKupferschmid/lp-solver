using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimplexAlgorithm
{
    public class Solver
    {
        public static SolverResult Solve(CsvReader reader)
        {
            return Solve(reader.HeadVariables, reader.RowVariables, reader.TargetVariable, reader.Matrix,
                reader.Optimization);
        }

        public static SolverResult Solve(Variable[] headVariables, Variable[] rowVariables, Variable targetVariable, double[][] matrix, Optimization optimization)
        {
            if(optimization == Optimization.Minimize)
                InvertMinimizeToMaximizeMatrix(matrix);

            var t = matrix.Select(r=>r[r.Length - 1]).Any(v => v < 0)
                ? PreprocessMatrix(headVariables, rowVariables, matrix) 
                : new Tableau(headVariables,rowVariables,targetVariable,matrix);


            return InnerSolve(t, optimization);
        }

        private static SolverResult InnerSolve(Tableau tableau, Optimization optimization)
        {
            var head = 0;
            var row = 0;

            var watchdog = 1000;

            while (tableau.Pivot(out row, out head))
            {
                if (--watchdog < 0)
                    throw new Exception();
                tableau.Switch(row, head);
            }

            var values = new Dictionary<Variable, double>();

            for (var i = 0; i < tableau.RowVariables.Length; i++)
            {
                if (tableau.RowVariables[i].Type == VariableType.Problem)
                    values.Add(tableau.RowVariables[i], tableau.Matrix[i][tableau.CoefficientIndex]);
            }

            values.Add(tableau.TargetVariable, tableau.Matrix[tableau.TargetIndex][tableau.CoefficientIndex]);

            return new SolverResult(ResultType.OneResult, optimization, values);
        }

        private static void InvertMinimizeToMaximizeMatrix(double[][] matrix)
        {
            //invert target function for minimization
            var targetLine = matrix[matrix.Length - 1];
            for (var i = 0; i < targetLine.Length; i++)
            {
                targetLine[i] = -targetLine[i];
            }
        }

        private static Tableau PreprocessMatrix(Variable[] headVariables, Variable[] rowVariables, double[][] matrix)
        {
            var helpVar = Variable.Problem(0);
            var helpTarget = Variable.Target();
            headVariables = new[] { helpVar }.Concat(headVariables).ToArray();

            for (var i = 0; i < matrix.Length - 1; i++)
            {
                var newRow = new double[headVariables.Length + 1];
                newRow[0] = 1;
                Array.Copy(matrix[i],0, newRow, 1, matrix[i].Length);
                matrix[i] = newRow;
            }

            var targetIndex = matrix.Length - 1;
            var mainTarget = matrix[targetIndex];
            matrix[targetIndex] = new double[headVariables.Length + 1];

            matrix[targetIndex][0] = -1;

            var tableau = new Tableau(headVariables,rowVariables,helpTarget, matrix);

            for (var i = 0; i < tableau.TargetIndex; i++)
            {
                if(tableau.Matrix[i][tableau.CoefficientIndex] < 0)
                    tableau.Switch(i, 0);
            }

            return tableau;
        }

        public enum ResultType
        {
            NoResults,
            OneResult,
            InfinitResults
        }

        public enum Optimization
        {
            Maximize,
            Minimize
        }

        public class SolverResult
        {
            public readonly ResultType Type;
            public readonly Optimization Optimization;
            public readonly Dictionary<Variable, double> Values;

            protected internal SolverResult(ResultType type, Optimization optimization, Dictionary<Variable, double> values)
            {
                Type = type;
                Optimization = optimization;
                Values = values;
            }
        }
    }
}
