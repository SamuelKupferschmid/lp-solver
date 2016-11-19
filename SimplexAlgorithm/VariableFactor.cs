using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexAlgorithm
{
    public struct VariableFactor
    {
        public readonly Variable Variable;
        public readonly double Factor;

        public VariableFactor(Variable variable, double factor)
        {
            Variable = variable;
            Factor = factor;
        }


        public static VariableFactor operator *(VariableFactor vf, double f) => new VariableFactor(vf.Variable, vf.Factor * f);
        public static VariableFactor operator +(VariableFactor vf, double f) => new VariableFactor(vf.Variable, vf.Factor + f);

        public static bool operator ==(VariableFactor f1, VariableFactor f2)
            => f1.Variable == f2.Variable && f1.Factor - f2.Factor < double.Epsilon;
        public static bool operator !=(VariableFactor f1, VariableFactor f2) => !(f1 == f2);
    }
}
