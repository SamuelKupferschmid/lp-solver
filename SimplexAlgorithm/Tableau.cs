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
            Equations = equations;
        }

        public Equation TargetEquation => Equations[Equations.Length - 1];

        private const int ColWidth = 6;

        public void Print()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(new string(' ', ColWidth));

            var format = "{0," + ColWidth + "}";

            foreach (var v in Equations[0].SummandVariables)
            {
                sb.AppendFormat(format, v);
            }

            sb.AppendFormat(format, "c");

            sb.AppendLine(null);

            foreach (var eq in Equations.OrderBy(e=>e.LeftTerm.Type))
            {
                sb.AppendFormat(format, eq.LeftTerm);
                foreach (var val in eq.SummandValues)
                {
                    sb.AppendFormat(format, val);
                }

                sb.AppendFormat(format, eq.Coefficient);
                sb.AppendLine(null);
            }

            Console.WriteLine(sb);
        }

        public bool FindPivot(out Variable head, out Variable row)
        {
            head = default(Variable);
            row = default(Variable);

            var minVal = double.MaxValue;
            var minIndex = -1;

            var target = TargetEquation;

            for (int i = 0; i < target.SummandValues.Length; i++)
            {
                if (target.SummandValues[i] > 0 && target.SummandValues[i] < minVal)
                {
                    minVal = target.SummandValues[i];
                    minIndex = i;
                }
            }

            if (minIndex == -1)
                return false;

            head = target.SummandVariables[minIndex];

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
    }
}
