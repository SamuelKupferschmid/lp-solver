using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplexAlgorithm;

namespace myLP
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = args?[0];

            if (filename == null)
            {
                Console.WriteLine("1 argument with the filename expected.");
                return;
            }
            else if (!File.Exists(filename))
            {
                Console.WriteLine("File '%s' don't exist");
                return;
            }

            var csv = new CsvReader(filename);

            var result = Solver.Solve(csv);

            switch (result.Type)
            {
                case Solver.ResultType.NoResults:
                    Console.WriteLine("No result found.");
                    break;
                case Solver.ResultType.OneResult:
                    Console.WriteLine("One result found:");
                    foreach (var vals in result.Values)
                    {
                        Console.WriteLine(vals.Key.ToString(vals.Value));
                    }
                    break;
                case Solver.ResultType.InfinitResults:
                    Console.WriteLine("Infinit results found.");
                    break;
            }
        }
    }
}
