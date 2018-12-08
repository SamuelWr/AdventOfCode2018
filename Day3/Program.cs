using System;
using System.Collections.Generic;
using System.Linq;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("input.txt").Where(l => !string.IsNullOrWhiteSpace(l));
            A(input);
            B(input);
        }

        static void A(IEnumerable<string> input)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var claims = input.Select(i => Claim.FromString(i)).ToArray();
            var maxColumn = claims.Max(c => c.Width + c.StartColumn);
            var maxRow = claims.Max(c => c.Height + c.StartRow);
            var dimension = Math.Max(maxColumn, maxRow);
            int[,] fabric = new int[dimension, dimension];

            foreach (var claim in claims)
            {
                for (int i = claim.StartRow; i < claim.StartRow + claim.Height; i++)
                {
                    for (int j = claim.StartColumn; j < claim.StartColumn + claim.Width; j++)
                    {
                        fabric[i, j]++;
                    }
                }
            }
            int doubles = 0;
            foreach (var item in fabric)
            {
                if (item > 1) doubles++;
            }
            watch.Stop();
            Console.WriteLine("A: " + doubles);
            Console.WriteLine($"\tin: {watch.ElapsedMilliseconds}ms");
        }

        static void B(IEnumerable<string> input)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var claims = input.Select(i => Claim.FromString(i)).ToArray();
            var maxColumn = claims.Max(c => c.Width + c.StartColumn);
            var maxRow = claims.Max(c => c.Height + c.StartRow);
            var dimension = Math.Max(maxColumn, maxRow);
            int[,] fabric = new int[dimension, dimension];

            foreach (var claim in claims)
            {
                for (int i = claim.StartRow; i < claim.StartRow + claim.Height; i++)
                {
                    for (int j = claim.StartColumn; j < claim.StartColumn + claim.Width; j++)
                    {
                        fabric[i, j]++;
                    }
                }
            }

            Stack<int> uniques = new Stack<int>();

            foreach (var claim in claims)
            {
                bool unique = true;
                for (int i = claim.StartRow; i < claim.StartRow + claim.Height; i++)
                {
                    for (int j = claim.StartColumn; j < claim.StartColumn + claim.Width; j++)
                    {
                        if (fabric[i, j] != 1) unique = false;
                    }
                }
                if (unique) uniques.Push(claim.Id);

            }
            if (uniques.Count != 1)
            {
                Console.WriteLine("Not a unique solution to B! candidates: ");
                Console.WriteLine(string.Join(", ", uniques));
                return;
            }

            watch.Stop();
            Console.WriteLine("B: " + uniques.Pop());
            Console.WriteLine($"\tin: {watch.ElapsedMilliseconds}ms");
        }
    }

    struct Claim
    {
        public readonly int Id;
        public readonly int Width, Height;
        public readonly int StartColumn, StartRow;
        private Claim(int id, int width, int height, int column, int row)
        {
            Id = id; Width = width; Height = height; StartColumn = column; StartRow = row;
        }
        public static Claim FromString(string s)
        {
            const string pattern = @"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)";
            var regex = new System.Text.RegularExpressions.Regex(pattern);

            var match = regex.Match(s);
            if (!match.Success) throw new ArgumentException("Not correct input!" + " " + s);

            return new Claim(
                id: int.Parse(match.Groups[1].Value),
                column: int.Parse(match.Groups[2].Value),
                row: int.Parse(match.Groups[3].Value),
                width: int.Parse(match.Groups[4].Value),
                height: int.Parse(match.Groups[5].Value)
                );
        }
    }

}
