using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexAlgorithm
{
    public class Solver
    {
        private readonly Equation[] _equations;

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

            while (tableau.FindPivot(out head, out row))
            {
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
