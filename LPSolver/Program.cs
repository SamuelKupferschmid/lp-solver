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
            var cells = File.ReadLines(args[0]).Select(l=>l.Split(';')).ToArray();

            var vars = new Variable[int.Parse(cells[0][1])];
            var conditons = new Equation[int.Parse(cells[0][1])];

        }
    }
}
