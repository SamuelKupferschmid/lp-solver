using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexAlgorithm
{
    public class Variable
    {
        public readonly VariableType Type;
        public readonly string Name;
        public readonly string Description;
        public readonly string Unit;

        public Variable(VariableType type, string name, string description = null, string unit = null)
        {
            Type = type;
            Name = name;
            Description = description;
            Unit = unit;
        }

        public const string ProblemPrefix = "x";
        public const string SlackPrefix = "y";
        public const string TargetPrefix = "z";


        public static Variable Problem(int i) => new Variable(VariableType.Problem, ProblemPrefix + i);
        public static Variable Slack(int i) => new Variable(VariableType.Slack, SlackPrefix + i);
        public static Variable Target() => new Variable(VariableType.Target, TargetPrefix);

        public override string ToString() => $"{Name}";

        public string ToString(double value)
        {
            return $"{Name} = {value}";
        }
        public static bool operator ==(Variable v1, Variable v2) => v1.Type == v2.Type && v1.Name == v2.Name;

        public static bool operator !=(Variable v1, Variable v2) => !(v1 == v2);

        protected bool Equals(Variable other) => this == other;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Variable)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Type;
                hashCode = (hashCode * 397) ^ (Name?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Description?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Unit?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }

    public enum VariableType
    {
        Problem,
        Slack,
        Target
    }
}
