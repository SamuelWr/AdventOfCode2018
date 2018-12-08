using System;
using System.Collections.Generic;
using System.Linq;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = ParseInput(System.IO.File.ReadLines("input.txt"));
            Test();
        }

        private static (char step, char requires)[] ParseInput(IEnumerable<string> input)
        {
            string pattern = @"Step (.) must be finished before step (.) can begin.";
            var regex = new System.Text.RegularExpressions.Regex(pattern);

            return input
                .Select(l => regex.Match(l))
                .Select(match => (match.Groups[2].Value.Single(), match.Groups[1].Value.Single()))
                .ToArray();
        }

        private static void Test()
        {
            var testInput = @"Step C must be finished before step A can begin.
Step C must be finished before step F can begin.
Step A must be finished before step B can begin.
Step A must be finished before step D can begin.
Step B must be finished before step E can begin.
Step D must be finished before step E can begin.
Step F must be finished before step E can begin.";
            var input = ParseInput(testInput.Split(new[] { '\n' }).Select(l => l.Trim()));

            string resA = A(input);
            if (resA == "CABDFE") Console.WriteLine("Test A OK");
            else Console.WriteLine("Test A FAIL, returned \"" + resA + "\" expected \"CABDFE\"");
        }

        private static string A((char step, char requires)[] input)
        {
            return "";
        }
    }
}
