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
        public readonly VariableFactor[] Factors;
        public readonly double Coefficient;

        /// <summary>
        /// A minimal Equation used for solving the SimplexAlgorithm
        /// </summary
        public Equation(Variable leftTerm, VariableFactor[] factors, double coefficient)
        {
            if (factors.Select(f => f.Variable).Distinct().Count() < factors.Length)
                throw new InvalidOperationException("There are duplicated variables");

            LeftTerm = leftTerm;
            Factors = new VariableFactor[factors.Length];
            Array.Copy(factors, Factors, factors.Length);

            Coefficient = coefficient;
        }

        public double this[Variable index] => (from f in Factors where f.Variable == index select f.Factor).FirstOrDefault();
        public VariableFactor this[int index] => Factors[index];

        public int IndexOf(Variable v)
        {
            for (int i = 0; i < Factors.Length; i++)
            {
                if (Factors[i].Variable == v)
                    return i;
            }

            return -1;
        }

        public double Ration(Variable problem) => Math.Abs(Coefficient / this[problem]);

        public Equation Switch(Variable leftTerm)
        {
            var baseFac = (-1) / this[leftTerm];

            var factors = new VariableFactor[Factors.Length];
            var si = IndexOf(leftTerm);
            factors[si] = new VariableFactor(LeftTerm, -baseFac);

            for (var i = 0; i < Factors.Length; i++)
            {
                if (i != si)
                    factors[i] = Factors[i] * baseFac;
            }

            return new Equation(leftTerm, factors, Coefficient * baseFac);
        }


        /// <summary>
        /// Returns an Equation which has applied the given Equation to the current Equation. The LeftTerm is equal to the current.
        /// </summary>
        /// <param name="equation">An Equation whith a LeftTerm Variable that exists in the current Equation Summands</param>
        /// <returns></returns>
        public Equation Resolve(Equation equation)
        {
            var fac = this[equation.LeftTerm];

            var vars = new VariableFactor[equation.Factors.Length];

            for (var i = 0; i < vars.Length; i++)
            {
                vars[i] = (equation[i] * fac) + this[equation[i].Variable];
            }

            var coefficient = Coefficient + equation.Coefficient * fac;

            return new Equation(LeftTerm, vars, coefficient);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(LeftTerm);
            sb.Append(" = ");

            if (Factors != null)
            {
                for (int i = 0; i < Factors.Length; i++)
                {
                    var f = Factors[i];

                    if (i > 0)
                        sb.Append(f.Factor >= 0 ? " + " : " - ");

                    if (Math.Abs(Math.Abs(f.Factor) - 1) > double.Epsilon)
                    {
                        sb.Append(i > 0 ? Math.Abs(f.Factor) : f.Factor);

                        sb.Append("*");
                    }
                    else if (i == 0 && f.Factor < 0)
                    {
                        sb.Append("-");
                    }
                    sb.Append(f.Variable);
                }
            }

            if (Factors == null || Factors.Length == 0)
                sb.Append(" " + Coefficient);
            else
                sb.Append((Coefficient >= 0 ? " + " : " - ") + Math.Abs(Coefficient));

            return sb.ToString();
        }

        public static bool operator ==(Equation e1, Equation e2)
        {
            if (e1.Factors.Length != e2.Factors.Length)
                return false;

            if (e1.Factors.Any(f => Math.Abs(f.Factor - e2[f.Variable]) >= double.Epsilon))
                return false;

            return e1.LeftTerm == e2.LeftTerm
                && Math.Abs(e1.Coefficient - e2.Coefficient) <= double.Epsilon;
        }

        public static bool operator !=(Equation e1, Equation e2)
        {
            return !(e1 == e2);
        }

        public bool Equals(Equation other) => this == other;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Equation && Equals((Equation)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = LeftTerm.GetHashCode();
                hashCode = (hashCode * 397) ^ (Factors?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Coefficient.GetHashCode();
                return hashCode;
            }
        }
    }
}
