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
        private readonly Equation[] _equations;

        public Solver(string filename)
            : this(File.OpenText(filename))
        { }

        public Solver(StreamReader reader)
        {
            var cells = ReadAllLines(reader).Select(l => l.Split(';')).ToArray();

            var vCnt = int.Parse(cells[0][0]);
            var cCnt = int.Parse(cells[0][1]);

            var pVars = new Variable[vCnt];
            var sVars = new Variable[cCnt];

            for (var i = 0; i < pVars.Length; i++)
                pVars[i] = Variable.Problem(i + 1);

            var eqs = new List<Equation>();

            for (var i = 0; i < cCnt; i++)
            {
                var op = cells[i + 3][0];
                var fCells = cells[i + 3].Skip(1).ToArray();

                if (op == "<=" || op == "=")
                    eqs.Add(new Equation(Variable.Slack(i + 1), GetFactorsByLine(pVars,fCells,true), double.Parse(cells[i + 3][vCnt + 1])));

                if (op == ">=" || op == "=")
                    eqs.Add(new Equation(Variable.Slack(i + 1), GetFactorsByLine(pVars,fCells,false), double.Parse(cells[i + 3][vCnt + 1])));

            }

            //target function
            var tFactors = new VariableFactor[vCnt];

            for (var i = 0; i < tFactors.Length; i++)
            {
                tFactors[i] = new VariableFactor(pVars[i], double.Parse(cells[1][i + 1]));
            }

            var target = new Equation(Variable.Target(), tFactors, 0);

            eqs.Add(target);

            _equations = eqs.ToArray();
        }

        private VariableFactor[] GetFactorsByLine(Variable[] vars, string[] fCells, bool negate)
        {
            var f = new VariableFactor[vars.Length];

            for (var i = 0; i < f.Length; i++)
            {
                var val = double.Parse(fCells[i]);

                if (negate)
                    val = 0 - val;

                f[i] = new VariableFactor(vars[i], val);
            }

            return f;
        }

        public IEnumerable<Equation> Equations => _equations.AsEnumerable();

        public Solver(Equation[] equations)
        {
            _equations = equations;
        }

        public IEnumerable<string> ReadAllLines(StreamReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        public VariableFactor[] ResultFactors { get; private set; }

        public ResultType Solve()
        {
            var tableau = new Tableau(_equations);

            Variable head;
            Variable row;
            List<Equation> targetHistory = new List<Equation>();
            var isCircular = false;

            while (tableau.FindPivot(out head, out row))
            {
                tableau = NextTableau(tableau, head, row);

                //check cycles
                if (targetHistory.Contains(tableau.TargetEquation))
                {
                    isCircular = true;
                    break;
                }
                targetHistory.Add(tableau.TargetEquation);
            }

            ResultFactors = (from e in tableau.Equations where e.LeftTerm.Type != VariableType.Slack select new VariableFactor(e.LeftTerm, e.Coefficient)).ToArray();

            if(isCircular)
                return ResultType.InfinitResults;
            return ResultType.OneResult;
        }

        public static Tableau NextTableau(Tableau t, Variable pHead, Variable pRow)
        {
            var pIndex = t.IndexOf(pRow);
            var pEq = t[pRow].Switch(pHead);

            var equations = new Equation[t.Equations.Length];

            for (int i = 0; i < equations.Length; i++)
            {
                if (i == pIndex)
                {
                    equations[i] = pEq;
                }
                else
                {
                    equations[i] = t[i].Resolve(pEq);
                }
            }

            return new Tableau(equations);
        }

        public enum ResultType
        {
            NoResults,
            OneResult,
            InfinitResults
        }
    }
}
