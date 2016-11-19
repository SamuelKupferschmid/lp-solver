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

        /// <summary>
        /// A minimal Equation used for solving the SimplexAlgorithm
        /// </summary>
        /// <param name="leftTerm">A Variable used as left Term</param>
        /// <param name="summands">An Array of Variable double Tuples</param>
        /// <param name="coefficient"></param>
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

        public Equation Switch(Variable leftTerm)
        {
            var baseFactor = (-1)/Value(leftTerm);

            var summands = new List<Tuple<Variable, double>> {new Tuple<Variable, double>(LeftTerm, -baseFactor)};


            for (int i = 0; i < SummandVariables.Length; i++)
            {
                if(SummandVariables[i] != leftTerm)
                    summands.Add(new Tuple<Variable, double>(SummandVariables[i],SummandValues[i]  * baseFactor));
            }

            return new Equation(leftTerm,summands.ToArray(),Coefficient * baseFactor);
        }
    }
}
