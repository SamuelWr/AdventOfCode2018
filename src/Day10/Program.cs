using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day10
{
    class Program
    {
        static string exampleInput = @"position=< 9,  1> velocity=< 0,  2>
position=< 7,  0> velocity=<-1,  0>
position=< 3, -2> velocity=<-1,  1>
position=< 6, 10> velocity=<-2, -1>
position=< 2, -4> velocity=< 2,  2>
position=<-6, 10> velocity=< 2, -2>
position=< 1,  8> velocity=< 1, -1>
position=< 1,  7> velocity=< 1,  0>
position=<-3, 11> velocity=< 1, -2>
position=< 7,  6> velocity=<-1, -1>
position=<-2,  3> velocity=< 1,  0>
position=<-4,  3> velocity=< 2,  0>
position=<10, -3> velocity=<-1,  1>
position=< 5, 11> velocity=< 1, -2>
position=< 4,  7> velocity=< 0, -1>
position=< 8, -2> velocity=< 0,  1>
position=<15,  0> velocity=<-2,  0>
position=< 1,  6> velocity=< 1,  0>
position=< 8,  9> velocity=< 0, -1>
position=< 3,  3> velocity=<-1,  1>
position=< 0,  5> velocity=< 0, -1>
position=<-2,  2> velocity=< 2,  0>
position=< 5, -2> velocity=< 1,  2>
position=< 1,  4> velocity=< 2,  1>
position=<-2,  7> velocity=< 2, -2>
position=< 3,  6> velocity=<-1, -1>
position=< 5,  0> velocity=< 1,  0>
position=<-6,  0> velocity=< 2,  0>
position=< 5,  9> velocity=< 1, -2>
position=<14,  7> velocity=<-2,  0>
position=<-3,  6> velocity=< 2, -1>";
        static void Main(string[] args)
        {
            var points = System.IO.File.ReadAllLines("input.txt").Select(l => Point.FromString(l)).ToArray();
            // points = exampleInput.Split('\n').Select(l => Point.FromString(l)).ToArray();
            A(points);
        }
        static void A(Point[] points)
        {
            var t = FindSmallestTime(points);
            Console.WriteLine("Smallest time: " + t);
            ConsoleKeyInfo key;
            do
            {
                Print(points, t);
                key = System.Console.ReadKey();
                if (key.Key == ConsoleKey.LeftArrow) t--;
                if (key.Key == ConsoleKey.RightArrow) t++;

            } while (key.KeyChar != 'q');
        }

        static void Print(Point[] points, int time)
        {
            Console.Clear();
            int minX = int.MaxValue, maxX = int.MinValue, minY = int.MaxValue, maxY = int.MinValue;
            var positions = new HashSet<Position>();

            foreach (var position in points.Select(p => p.PositionAtTime(time)))
            {
                minX = minX < position.X ? minX : position.X;
                maxX = maxX > position.X ? maxX : position.X;

                minY = minY < position.Y ? minY : position.Y;
                maxY = maxY > position.Y ? maxY : position.Y;

                positions.Add(position);
            }            

            Console.WriteLine($"At time: {time}, arrow keys to move time. q to quit");
            Console.WriteLine($"Displaying x:[{minX},{maxX}], y:[{minY},{maxY}]");
            var sb = new StringBuilder();
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    sb.Append(positions.Contains(new Position(x, y)) ? '#' : ' ');
                }
                Console.WriteLine(sb.ToString());
                sb.Clear();
            }

        }
        static int FindSmallestTime(Point[] points)
        {
            var size = int.MaxValue;
            int t = 0;
            while (true)
            {
                var newSize = getsizeAt(t);
                if (newSize > size) return t - 1;
                size = newSize;
                t++;
            }


            int getsizeAt(int time)
            {
                int minX = int.MaxValue, maxX = int.MinValue, minY = int.MaxValue, maxY = int.MinValue;

                foreach (var position in points.Select(p => p.PositionAtTime(time)))
                {
                    minX = minX < position.X ? minX : position.X;
                    maxX = maxX > position.X ? maxX : position.X;

                    minY = minY < position.Y ? minY : position.Y;
                    maxY = maxY > position.Y ? maxY : position.Y;

                }

                return (maxX - minX) + (maxY - minY);
            }
        }
    }

    struct Point
    {
        public int InitialX { get; }
        public int InitialY { get; }
        public int deltaX { get; }
        public int deltaY { get; }

        private Point(int x, int y, int dx, int dy)
        {
            InitialX = x; InitialY = y; deltaX = dx; deltaY = dy;
        }
        public Position PositionAtTime(int t)
        {
            return new Position(InitialX + t * deltaX, InitialY + t * deltaY);
        }

        public static Point FromString(string s)
        {
            var parts = s.Replace("position=<", "").Replace("> velocity=<", ",").Replace(">", "").Split(",").Select(p => p.Trim()).Select(p => int.Parse(p)).ToArray();

            return new Point(parts[0], parts[1], parts[2], parts[3]);
        }
    }

    struct Position : IEquatable<Position>
    {
        public readonly int X;
        public readonly int Y;

        public Position(int x, int y)
        {
            X = x; Y = y;
        }

        public bool Equals(Position other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X << 16 + Y; //good enough if close to (0,0)
        }
    }
}
