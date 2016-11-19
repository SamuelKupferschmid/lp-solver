using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexAlgorithm
{
    public struct Equation
    {
        public readonly Variable LeftTerm;
        public readonly Variable[] SummandVariables;
        public readonly double[] SummandValues;
        public readonly double Coefficient;

        public Equation(Variable leftTerm, Tuple<Variable,double>[] summands, double coefficient)
        {
            LeftTerm = leftTerm;
            var l = summands.Count();
            SummandVariables = new Variable[l];
            SummandValues = new double[l];

            int i = 0;
            foreach (var s in summands)
            {
                SummandVariables[i] = s.Item1;
                SummandValues[i++] = s.Item2;
            }

            Coefficient = coefficient;
        }

        public double Value(Variable v)
        {
            for (var i = 0; i < SummandVariables.Length; i++)
            {
                if (SummandVariables[i] == v)
                    return SummandValues[i];
            }

            throw new ArgumentOutOfRangeException();
        }

        public double Ration(Variable problem) => Math.Abs(Coefficient/Value(problem));
    }
}
