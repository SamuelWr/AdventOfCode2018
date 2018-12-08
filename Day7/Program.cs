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
            Console.WriteLine(input.Last());
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
    }
}
