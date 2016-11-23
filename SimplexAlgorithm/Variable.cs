using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexAlgorithm
{
    public struct Variable
    {
        public readonly int Index;
        public readonly VariableType Type;

        public Variable(VariableType type, int index)
        {
            Type = type;
            Index = index;
        }

        public static Variable Slack(int index) => new Variable(VariableType.Slack, index);
        public static Variable Problem (int index) => new Variable(VariableType.Problem, index);
        public static Variable Target () => new Variable(VariableType.Target, 0);

        public static bool operator ==(Variable v1, Variable v2) => v1.Type == v2.Type && (v1.Index == v2.Index || v1.Type == VariableType.Target);
        public static bool operator !=(Variable v1, Variable v2) => !(v1 == v2);

        public bool Equals(Variable other)
        {
            return Index == other.Index && Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Variable && Equals((Variable)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Index * 397) ^ (int)Type;
            }
        }

        public override string ToString()
        {
            if (Type == VariableType.Target)
                return "z";

            return (Type == VariableType.Problem ? "x" : "y") + Index;
        }
    }

    public enum VariableType
    {
        Slack,
        Problem,
        Target
    }
}
