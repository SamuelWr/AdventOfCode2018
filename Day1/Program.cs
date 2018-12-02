using System.Linq;
using System;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] input = System.IO.File.ReadAllLines("input.txt")
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => int.Parse(l)).ToArray();
            A(input);
        }

        static void A(int[] input)
        {
            Console.WriteLine("A: " + input.Sum());
        }
    }
}
