using System;
using System.Collections.Generic;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = System.IO.File.ReadAllLines("input.txt");
            A(input);
        }

        static void A(string[] input)
        {
            int exactlyTwo = input.Count(l => HasCharOccuringExactly(l, 2));
            int exactlyThree = input.Count(l => HasCharOccuringExactly(l, 3));
            Console.WriteLine("A: " + (exactlyTwo * exactlyThree));
        }

        private static bool HasCharOccuringExactly(string boxId, int times)
        {
            var count = new Dictionary<char, int>();
            foreach (var character in boxId)
            {
                if (!count.TryAdd(character, 1))
                    count[character]++;
            }
            return count.Values.Any(v => v == times);
        }
    }
}
