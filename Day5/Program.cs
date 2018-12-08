using System;

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
            return -1;
        }

        static int B(string input)
        {
            return -1;
        }

        static string React(string input)
        {
            return "";
        }
    }
}
