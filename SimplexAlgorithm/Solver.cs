using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimplexAlgorithm
{
    public class Solver
    {
        private readonly Equation[] _equations;

        public Solver(string filename)
            : this(File.OpenText(filename))
        { }

        public Solver(StreamReader reader)
        {
            var cells = ReadAllLines(reader).Select(l => l.Split(';')).ToArray();

            var vCnt = int.Parse(cells[0][0]);
            var cCnt = int.Parse(cells[0][1]);

            var pVars = new Variable[vCnt];
            var sVars = new Variable[cCnt];

            for (var i = 0; i < pVars.Length; i++)
                pVars[i] = Variable.Problem(i + 1);

            var eqs = new List<Equation>();

            for (var i = 0; i < cCnt; i++)
            {
                var op = cells[i + 3][0];
                var fCells = cells[i + 3].Skip(1).ToArray();

                if (op == "<=" || op == "=")
                    eqs.Add(new Equation(Variable.Slack(i + 1), GetFactorsByLine(pVars, fCells, true), double.Parse(cells[i + 3][vCnt + 1])));

                if (op == ">=" || op == "=")
                    eqs.Add(new Equation(Variable.Slack(i + 1), GetFactorsByLine(pVars, fCells, false), double.Parse(cells[i + 3][vCnt + 1])));

            }

            //target function
            var tFactors = new VariableFactor[vCnt];
            var minimize = cells[1][0].Contains("min");

            for (var i = 0; i < tFactors.Length; i++)
            {
                var val = double.Parse(cells[1][i + 1]);

                if (minimize)
                    val = -val;

                tFactors[i] = new VariableFactor(pVars[i], val);
            }
            var targetVal = double.Parse(cells[1][vCnt + 1]);

            if (minimize)
                targetVal = -targetVal;

            var target = new Equation(Variable.Target(), tFactors, targetVal);

            eqs.Add(target);

            _equations = eqs.ToArray();
        }

        private VariableFactor[] GetFactorsByLine(Variable[] vars, string[] fCells, bool negate)
        {
            var f = new VariableFactor[vars.Length];

            for (var i = 0; i < f.Length; i++)
            {
                var val = double.Parse(fCells[i]);

                if (negate)
                    val = 0 - val;

                f[i] = new VariableFactor(vars[i], val);
            }

            return f;
        }

        public IEnumerable<Equation> Equations => _equations.AsEnumerable();

        public Solver(Equation[] equations)
        {
            _equations = equations;
        }

        public IEnumerable<string> ReadAllLines(StreamReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        public VariableFactor[] ResultFactors { get; private set; }

        public ResultType Solve()
        {
            var tableau = new Tableau(_equations);

            tableau = PreprocessTableau(tableau);

            return SolveTableau(ref tableau);
        }

        private Tableau PreprocessTableau(Tableau t)
        {
            var initialTarget = t.TargetEquation;
            //check if there is no need to preprocess the table
            if (t.Equations.All(e => e.LeftTerm.Type == VariableType.Target || e.Coefficient >= 0))
                return t;

            var eqs = new Equation[t.Equations.Length];
            var tVar = Variable.Problem(0);

            for (var i = 0; i < eqs.Length - 1; i++)
                eqs[i] = t.Equations[i].AddFactor(new VariableFactor(tVar, 1));

            var f = new VariableFactor[t.Equations[0].Factors.Length];
            for (var i = 0; i < f.Length; i++)
            {
                f[i] = new VariableFactor(t.Equations[0].Factors[i].Variable,0);
            }

            var target = new Equation(Variable.Target(), f, 0);
            target = target.AddFactor(new VariableFactor(tVar,-1));

            eqs[eqs.Length - 1] = target;
            t = new Tableau(eqs);

            for (var i = 0; i < eqs.Length; i++)
            {
                if (t[i].LeftTerm.Type != VariableType.Target && t[i].Coefficient < 0)
                    t = NextTableau(t, tVar, t[i].LeftTerm);
            }

            SolveTableau(ref t);

            var mainEquations = new Equation[t.Equations.Length];


            for (var i = 0; i < mainEquations.Length - 1; i++)
                mainEquations[i] = t.Equations[i].RemoveFactor(tVar);

            //var newTarget = new Equation(target.LeftTerm,target.Factors,CalculatePreprocessedTargetCoefficient(initialTarget,t))
            //    .RemoveFactor(tVar);

            mainEquations[mainEquations.Length - 1] = CalculatePreprocessedMainTarget(initialTarget,t);
            return new Tableau(mainEquations);
        }

        public static Equation CalculatePreprocessedMainTarget(Equation initialTarget, Tableau preprocessingTableau)
        {
            if (Math.Abs(preprocessingTableau.TargetEquation.Coefficient) > double.Epsilon)
                throw new NotImplementedException();


            var resolvedVars =
                preprocessingTableau.Equations.Where(e => e.LeftTerm.Type == VariableType.Problem)
                    .Select(e => new Equation(e.LeftTerm, new VariableFactor[0], e.Coefficient)).ToArray();

            for (var i = 0; i < resolvedVars.Length; i++)
                initialTarget = initialTarget.Resolve(resolvedVars[i]);

            return new Equation(preprocessingTableau.TargetEquation.LeftTerm,preprocessingTableau.TargetEquation.Factors, initialTarget.Coefficient);
        }

        private ResultType SolveTableau(ref Tableau t)
        {
            Variable head;
            Variable row;

            var prevTargetVal = 0d;
            var isCircular = false;

            while (t.FindPivot(out head, out row))
            {
                t = NextTableau(t, head, row);

                //check cycles
                if (Math.Abs(t.TargetEquation.Coefficient - prevTargetVal) <= double.Epsilon)
                {
                    isCircular = true;
                    break;
                }

                prevTargetVal = t.TargetEquation.Coefficient;
            }

            ResultFactors = (from e in t.Equations where e.LeftTerm.Type != VariableType.Slack select new VariableFactor(e.LeftTerm, e.Coefficient)).ToArray();

            if (isCircular)
                return ResultType.InfinitResults;
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
