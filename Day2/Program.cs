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
            B(input);
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

        static void B(string[] input)
        {
            foreach (var (a, b) in input.Join(input, _ => 1, _ => 1, (a, b) => (a, b)))
            {
                if (DiffersAtExactlyOnePosition(a, b))
                {
                    Console.WriteLine("CANDIDATE FOUND:");
                    Console.WriteLine("\t" + a);
                    Console.WriteLine("\t" + b);
                    Console.WriteLine("COMMON CHARACTERS:");
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (a[i] == b[i]) Console.Write(a[i]);
                    }
                    Console.WriteLine("");
                }
            }
        }

        static bool DiffersAtExactlyOnePosition(string a, string b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException("not same length");
            int differences = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) differences++;
                if (differences > 1) return false;
            }
            return differences == 1;
        }
    }
}
