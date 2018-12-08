using System;
using System.Linq;

namespace Day5
{
    class Program
    {
        static string alphabet = "abcdefghijklmnopqrstuvwxyz";

        static void Main(string[] args)
        {
            Test();

            var input = System.IO.File.ReadAllText("input.txt");
            var watch = System.Diagnostics.Stopwatch.StartNew();

            Console.WriteLine("A:");
            Console.WriteLine("\tresult: " + A(input));
            Console.WriteLine("\tin: " + watch.ElapsedMilliseconds + "ms");

            watch.Restart();
            Console.WriteLine("B:");
            Console.WriteLine("\tresult: " + B(input));
            Console.WriteLine("\tin: " + watch.ElapsedMilliseconds + "ms");
        }

        static void Test()
        {
            var testInput = "dabAcCaCBAcCcaDA";

            var resA = A(testInput);
            if (resA == 10) Console.WriteLine("Test A OK");
            else Console.WriteLine($"Test A FAIL! expected 10 got {resA}");

            var resB = B(testInput);
            if (resB == 4) Console.WriteLine("Test B OK");
            else Console.WriteLine($"Test B FAIL! expected 4 got {resB}");
        }

        static int A(string input)
        {
            return React(input).Length;
        }

        static int B(string input)
        {
            return alphabet
                .Select(c => input.Replace("" + c, "").Replace("" + char.ToUpper(c), ""))
                .Select(polymer => React(polymer))
                .Select(polymer => polymer.Length)
                .Min();
        }

        static string React(string input)
        {
            var pairs = alphabet.SelectMany(c => new[] { "" + c + char.ToUpper(c), "" + char.ToUpper(c) + c }).ToArray();

            int oldLength;

            do
            {
                oldLength = input.Length;
                foreach (var pair in pairs)
                {
                    input = input.Replace(pair, "");
                }                
            } while (oldLength != input.Length);

            return input;
        }
    }
}
