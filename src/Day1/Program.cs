using System.Linq;
using System;
using System.Collections.Generic;

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
            B(input);
        }

        static void A(int[] input)
        {
            Console.WriteLine("A: " + input.Sum());
        }
        static void B(int[] input)
        {
            int frequency = 0;
            HashSet<int> seenFrequencies = new HashSet<int>();
            seenFrequencies.Add(frequency);
            foreach (var change in Loop(input))
            {
                frequency += change;
                if (seenFrequencies.Contains(frequency))
                {
                    Console.WriteLine("B: first repeat: " + frequency);
                    return;
                }
                else
                {
                    seenFrequencies.Add(frequency);
                }
            }
            Console.WriteLine("B: No repeats");
        }

        static IEnumerable<T> Loop<T>(IEnumerable<T> Source)
        {
            while (true)
            {
                foreach (var item in Source)
                {
                    yield return item;
                }
            }
        }
    }
}
