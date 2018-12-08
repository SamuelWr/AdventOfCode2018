using System;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var points = System.IO.File.ReadLines("input.txt")
                .Select(l => l.Split(','))
                .Select(pair => new Point(int.Parse(pair[0]), int.Parse(pair[1])))
                .ToArray();
            Console.WriteLine(points.Last());
        }
    }

    struct Point
    {
        public int Column { get; }
        public int Row { get; }

        public Point(int row, int column)
        {
            Row = row;
            Column = column;
        }
        public override string ToString()
        {
            return $"row: {Row}, column: {Column}";
        }
    }
}
