using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexAlgorithm
{
    public class Solver
    {
        private readonly Equation[] _equations;

        public Solver(string filename)
        {
            var cells = File.ReadAllLines(filename).Select(l => l.Split(';')).ToArray();

            var vCnt = int.Parse(cells[0][0]);
            var cCnt = int.Parse(cells[0][1]);

            var pVars = new Variable[vCnt];
            var sVars = new Variable[cCnt];

            for(int i = 0; i < pVars.Length;i++)
                pVars[i] = Variable.Problem(i+1);

            var eqs = new Equation[cCnt + 1];

            for (var i = 0; i < cCnt; i++)
            {
                var facs = new VariableFactor[vCnt];
                for (var j = 0; j < vCnt; j++)
                {
                    facs[j] = new VariableFactor(pVars[j],double.Parse(cells[i + 3][j + 1]));
                }

                eqs[i] = new Equation(Variable.Slack(i+1),facs, double.Parse(cells[i + 3][vCnt + 1]));
            }

            //target function
            var tFactors = new VariableFactor[vCnt];

            for (int i = 0; i < tFactors.Length; i++)
            {
                tFactors[i] = new VariableFactor(pVars[i],double.Parse(cells[1][i+1]));
            }

            var target = new Equation(Variable.Target(), tFactors, 0);

            eqs[eqs.Length - 1] = target;

            _equations = eqs;
        }

        public Solver(Equation[] equations)
        {
            _equations = equations;
        }

        public Equation[] Equations { get; private set; }
        public VariableFactor[] ResultFactors { get; private set; }

        public ResultType Solve()
        {
            var tableau = new Tableau(_equations);

            Variable head;
            Variable row;
            int watchdog = 100;

            while (tableau.FindPivot(out head, out row))
            {
                if(watchdog-- <= 0)
                    throw new Exception();
                tableau = NextTableau(tableau, head, row);
            }

            ResultFactors = (from e in tableau.Equations where e.LeftTerm.Type != VariableType.Slack select new VariableFactor(e.LeftTerm, e.Coefficient)).ToArray();

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
