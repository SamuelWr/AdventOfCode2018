using System;
using System.Collections.Generic;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var points = System.IO.File.ReadLines("input.txt")
                .Select(l => l.Split(','))
                .Select(pair => new Point(int.Parse(pair[1]), int.Parse(pair[0])))
                .ToArray();

            Test();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("A: ");
            Console.WriteLine("\tResult: " + A(points));
            Console.WriteLine("\tin: " + watch.ElapsedMilliseconds + "ms");
            watch.Restart();
            Console.WriteLine("B:");
            Console.WriteLine("\tResult: " + B(points));
            Console.WriteLine("\tin: " + watch.ElapsedMilliseconds + "ms");
        }

        private static void Test()
        {
            var points =
                @"1, 1
1, 6
8, 3
3, 4
5, 5
8, 9".Split(new char[] { '\n' }).Select(l => l.Trim())
                .Select(l => l.Split(','))
                .Select(pair => new Point(int.Parse(pair[1]), int.Parse(pair[0])))
                .ToArray();

            var res = A(points);
            if (res == 17) Console.WriteLine("Test OK for A");
            else Console.WriteLine("Test FAILED for A, returned " + res + " should have been 17");

            var res2 = B(points, 32);
            if (res2 == 16) Console.WriteLine("Test OK for B");
            else Console.WriteLine("Test FAILED for B, returned " + res2 + " should have been 16");

        }



        static int A(Point[] points)
        {
            //setup
            int rows = points.Max(p => p.Row) + 1;
            int columns = points.Max(p => p.Column) + 1;
            var map = new char[rows, columns];
            var neighbourhoods = new Dictionary<char, Neighourhood>();

            char id = 'a';
            foreach (var point in points)
            {
                map[point.Row, point.Column] = id;
                neighbourhoods[id] = new Neighourhood(point);

                id++;
            }


            //main loop
            bool didanything;
            do
            {
                didanything = false;
                //consolidate surviving claims from previous rounds
                foreach (var item in neighbourhoods)
                {
                    id = item.Key;
                    foreach (var point in item.Value.Edge)
                    {
                        item.Value.Interior.Add(point);
                    }
                }
                //claim new coordinates
                foreach (var item in neighbourhoods)
                {
                    id = item.Key;
                    var oldEdge = item.Value.Edge;
                    foreach (var point in oldEdge)
                    {
                        item.Value.Interior.Add(point);
                    }
                    item.Value.Edge = new HashSet<Point>();
                    foreach (var point in oldEdge.SelectMany(p => p.GetAdjacent()))
                    {
                        if (point.Row < 0 || point.Row >= rows || point.Column < 0 || point.Column >= columns) { } //outside area
                        else if (map[point.Row, point.Column] == id) { } // already in neighbourhood
                        else if (map[point.Row, point.Column] == '.') { } // existing conflict
                        else if (map[point.Row, point.Column] == default(char))
                        {
                            didanything = true;
                            item.Value.Edge.Add(point);
                            map[point.Row, point.Column] = id;
                        }
                        else
                        {
                            var otherId = map[point.Row, point.Column];
                            if (!neighbourhoods[otherId].Interior.Contains(point))
                            {
                                neighbourhoods[otherId].Edge.Remove(point);
                                map[point.Row, point.Column] = '.';
                            }
                        }
                    }
                }
            } while (didanything);

            //find biggestS
            var candidates = neighbourhoods;

            //remove infinite areas
            for (int row = 0; row < rows; row++)
            {
                candidates.Remove(map[row, 0]);
                candidates.Remove(map[row, columns - 1]);
            }
            for (int column = 0; column < columns; column++)
            {
                candidates.Remove(map[0, column]);
                candidates.Remove(map[rows - 1, column]);
            }

            return candidates.Values.OrderByDescending(n => n.Interior.Count).First().Interior.Count;
        }

        static int B(Point[] points, int MAX_DISTANCE = 10_000)
        {
            var numPoints = points.Length;
            var minCol = 0 - (MAX_DISTANCE / numPoints);
            var maxCol = points.Max(p => p.Column) + (MAX_DISTANCE / numPoints);
            var minRow = 0 - (MAX_DISTANCE / numPoints);
            var maxRow = points.Max(p => p.Row) + (MAX_DISTANCE / numPoints);

            int sizeOfArea = 0;

            for (int row = minRow; row <= maxRow; row++)
            {
                for (int column = minCol; column <= maxCol; column++)
                {
                    var point = new Point(row, column);
                    int distance = points.Sum(p => p.Distance(point));
                    if (distance < MAX_DISTANCE) sizeOfArea++;
                }
            }


            return sizeOfArea;
        }
    }

    class Neighourhood
    {
        public Point Origin { get; }
        public HashSet<Point> Edge { get; set; }
        public HashSet<Point> Interior { get; }
        public Neighourhood(Point origin)
        {
            Origin = origin;
            Edge = new HashSet<Point>(new[] { origin });
            Interior = new HashSet<Point>();
        }
    }

    struct Point : IEquatable<Point>
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
        public override int GetHashCode()
        {
            return Row << 16 + Column; //good enough for the small numbers in day 6
        }

        public bool Equals(Point other)
        {
            return this.Column == other.Column && this.Row == other.Row;
        }

        public IEnumerable<Point> GetAdjacent()
        {
            yield return new Point(Row - 1, Column);
            yield return new Point(Row + 1, Column);
            yield return new Point(Row, Column - 1);
            yield return new Point(Row, Column + 1);
        }

        public int Distance(Point other)
        {
            int dRow = this.Row - other.Row;
            dRow = dRow > 0 ? dRow : -dRow;

            int dCol = this.Column - other.Column;
            dCol = dCol > 0 ? dCol : -dCol;

            return dRow + dCol;
        }
    }
}
