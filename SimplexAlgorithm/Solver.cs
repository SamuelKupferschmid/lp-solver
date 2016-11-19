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


            ResultFactors = null;
            Equations = null;
            return ResultType.OneResult;
        }

        public static Tableau NextTableau(Tableau t)
        {
            Variable head;
            Variable row;

            t.FindPivot(out head, out row);

            var pIndex = t.IndexOf(row);
            var pEq = t[row].Switch(head);

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
