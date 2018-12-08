using System;
using System.Collections.Generic;
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
            return React(input);
        }

        static int B(string input)
        {
            return alphabet
                .Select(c => input.Replace("" + c, "").Replace("" + char.ToUpper(c), ""))
                .Select(polymer => React(polymer))
                .Min();
        }

        static int React(string input)
        {
            var polymer = new Stack<char>();
            polymer.Push(input.First());

            foreach (var unit in input.Skip(1))
            {
                if (polymer.TryPeek(out var prevUnit))
                {
                    switch (char.IsLower(unit))
                    {
                        case true:
                            if (char.ToUpper(unit) == prevUnit)
                                polymer.Pop();
                            else polymer.Push(unit);
                            break;
                        case false:
                            if (unit == char.ToUpper(prevUnit))
                                polymer.Pop();
                            else polymer.Push(unit);
                            break;
                    }
                }
                else
                {
                    polymer.Push(unit);
                }
                
            }

            return polymer.Count;
        }
    }
}
