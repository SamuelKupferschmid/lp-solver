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
        public readonly Equation[] Equations;


        public Tableau(Equation[] equations)
        {
            if(equations == null)
                throw new ArgumentNullException(nameof(equations));

            Equations = equations;
        }

        public Equation TargetEquation => Equations[Equations.Length - 1];

        private const int ColWidth = 6;

        public Equation this[Variable index] => (from e in Equations where e.LeftTerm == index select e).FirstOrDefault();
        public Equation this[int index] => Equations[index];

        public int IndexOf(Variable v)
        {
            for (int i = 0; i < Equations.Length; i++)
            {
                if (Equations[i].LeftTerm == v)
                    return i;
            }

            return -1;
        }

        public bool FindPivot(out Variable head, out Variable row)
        {
            head = default(Variable);
            row = default(Variable);

            var minVal = double.MaxValue;
            var minIndex = -1;

            var facs = TargetEquation;

            for (int i = 0; i < facs.Factors.Length; i++)
            {
                if (facs[i].Factor > 0 && facs[i].Factor < minVal)
                {
                    minVal = facs[i].Factor;
                    minIndex = i;
                }
            }

            if (minIndex == -1)
                return false;

            head = facs[minIndex].Variable;

            minVal = double.MaxValue;

            for (var i = 0; i < Equations.Length - 1; i++)
            {
                var ration = Equations[i].Ration(head);

                if (ration < minVal)
                {
                    minVal = ration;
                    row = Equations[i].LeftTerm;
                }
            }

            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(new string(' ', ColWidth));

            var format = "{0," + ColWidth + "}";

            foreach (var v in Equations[0].Factors)
            {
                sb.AppendFormat(format, v.Variable);
            }

            sb.AppendFormat(format, "c");

            sb.AppendLine(null);

            foreach (var eq in Equations.OrderBy(e => e.LeftTerm.Type))
            {
                sb.AppendFormat(format, eq.LeftTerm);
                foreach (var val in eq.Factors)
                {
                    sb.AppendFormat(format, val);
                }

                sb.AppendFormat(format, eq.Coefficient);
                sb.AppendLine(null);
            }

            return sb.ToString();
        }
    }
}
