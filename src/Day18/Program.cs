using System;
using System.Collections.Generic;
using System.Text;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            var input = System.IO.File.ReadAllLines("input.txt");
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("A:");
            Console.WriteLine("\tresult: " + A(input));
            Console.WriteLine("\tin: " + watch.ElapsedMilliseconds + "ms");

            watch.Restart();
            Console.WriteLine("B:");
            Console.WriteLine("\tresult: " + B(input));
            Console.WriteLine("\tin: " + watch.ElapsedMilliseconds + "ms");
        }

        private static void Test()
        {
            var input = @".#.#...|#.
.....#|##|
.|..|...#.
..|#.....#
#.#|||#|#|
...#.||...
.|....|...
||...#|.#|
|.||||..|.
...#.|..|.".Split("\r\n");

            var resA = A(input, true);
            if (resA != 1147) Console.WriteLine("Test A Fail! expected 1147 but got: " + resA);
            else Console.WriteLine("Test A OK");
        }

        static int A(string[] input, bool debug = false)
        {
            var area = CollectionArea.FromInput(input);
            if (debug) Console.WriteLine(area.ToString());
            for (int i = 0; i < 10; i++)
            {
                area.Step();
                if (debug) Console.WriteLine(area.ToString());
            }
            return area.Value();
        }

        static int B(string[] input)
        {
            const int MAX_GENERATION = 1_000_000_000;
            Dictionary<int, int> seenValues = new Dictionary<int, int>();


            var area = CollectionArea.FromInput(input);
            while (area.Generation < MAX_GENERATION)
            {
                area.Step();
                if (area.Generation > 1000)
                {
                    if (seenValues.ContainsKey(area.Value()))
                    {
                        var gen = area.Generation;
                        var prevGen = seenValues[area.Value()];
                        var gendiff = gen - prevGen;
                        var skipSteps = (MAX_GENERATION - area.Generation) / gendiff;
                        area.SkipForward(skipSteps * gendiff);
                    }
                    else
                    {
                        seenValues[area.Value()] = area.Generation;
                    }
                }
            }
            return area.Value();
        }
    }



    class CollectionArea
    {
        public int Generation { get; private set; }
        private acre[,] acres;
        private readonly int dimension;
        private CollectionArea(int dimension)
        {
            this.dimension = dimension;
            acres = new acre[dimension + 2, dimension + 2];
        }

        public int Value()
        {
            int wood = 0, lumber = 0;
            for (int row = 1; row <= dimension; row++)
            {
                for (int col = 1; col <= dimension; col++)
                {
                    switch (acres[row, col])
                    {
                        case acre.open:
                            break;
                        case acre.wood:
                            wood++;
                            break;
                        case acre.lumberyard:
                            lumber++;
                            break;
                    }
                }
            }
            return wood * lumber;
        }

        public void Step()
        {
            var newAcres = new acre[dimension + 2, dimension + 2];
            for (int row = 1; row <= dimension; row++)
            {
                for (int col = 1; col <= dimension; col++)
                {
                    acre nextState;
                    int neighWood = 0;
                    int neighLumber = 0;
                    switch (acres[row, col])
                    {
                        case acre.open:
                            if (acres[row - 1, col - 1] == acre.wood) neighWood++;
                            if (acres[row - 1, col + 0] == acre.wood) neighWood++;
                            if (acres[row - 1, col + 1] == acre.wood) neighWood++;
                            if (acres[row + 0, col - 1] == acre.wood) neighWood++;
                            if (acres[row + 0, col + 1] == acre.wood) neighWood++;
                            if (acres[row + 1, col - 1] == acre.wood) neighWood++;
                            if (acres[row + 1, col + 0] == acre.wood) neighWood++;
                            if (acres[row + 1, col + 1] == acre.wood) neighWood++;

                            if (neighWood >= 3) { nextState = acre.wood; }
                            else nextState = acre.open;
                            break;
                        case acre.wood:
                            if (acres[row - 1, col + 0] == acre.lumberyard) neighLumber++;
                            if (acres[row - 1, col + 1] == acre.lumberyard) neighLumber++;
                            if (acres[row - 1, col - 1] == acre.lumberyard) neighLumber++;
                            if (acres[row + 0, col - 1] == acre.lumberyard) neighLumber++;
                            if (acres[row + 0, col + 1] == acre.lumberyard) neighLumber++;
                            if (acres[row + 1, col - 1] == acre.lumberyard) neighLumber++;
                            if (acres[row + 1, col + 0] == acre.lumberyard) neighLumber++;
                            if (acres[row + 1, col + 1] == acre.lumberyard) neighLumber++;

                            if (neighLumber >= 3) { nextState = acre.lumberyard; }
                            else nextState = acre.wood;
                            break;
                        case acre.lumberyard:
                            if (acres[row - 1, col - 1] == acre.wood) neighWood++;
                            if (acres[row - 1, col + 0] == acre.wood) neighWood++;
                            if (acres[row - 1, col + 1] == acre.wood) neighWood++;
                            if (acres[row + 0, col - 1] == acre.wood) neighWood++;
                            if (acres[row + 0, col + 1] == acre.wood) neighWood++;
                            if (acres[row + 1, col - 1] == acre.wood) neighWood++;
                            if (acres[row + 1, col + 0] == acre.wood) neighWood++;
                            if (acres[row + 1, col + 1] == acre.wood) neighWood++;

                            if (acres[row - 1, col + 0] == acre.lumberyard) neighLumber++;
                            if (acres[row - 1, col + 1] == acre.lumberyard) neighLumber++;
                            if (acres[row - 1, col - 1] == acre.lumberyard) neighLumber++;
                            if (acres[row + 0, col - 1] == acre.lumberyard) neighLumber++;
                            if (acres[row + 0, col + 1] == acre.lumberyard) neighLumber++;
                            if (acres[row + 1, col - 1] == acre.lumberyard) neighLumber++;
                            if (acres[row + 1, col + 0] == acre.lumberyard) neighLumber++;
                            if (acres[row + 1, col + 1] == acre.lumberyard) neighLumber++;

                            if (neighLumber > 0 && neighWood > 0) nextState = acre.lumberyard;
                            else { nextState = acre.open; }
                            break;
                        default: throw new Exception("unreachable"); //to satisfy compliler that nextState has been set.
                    }
                    newAcres[row, col] = nextState;
                }
            }
            acres = newAcres;
            Generation++;
        }

        public static CollectionArea FromInput(string[] input)
        {
            var area = new CollectionArea(input.Length)
            {
                Generation = 0,
            };

            for (int row = 1; row < input.Length + 1; row++)
            {
                for (int column = 1; column < input.Length + 1; column++)
                {
                    area.acres[row, column] = parseAcre(input[row - 1][column - 1]);
                }
            }


            return area;
            acre parseAcre(char c)
            {
                switch (c)
                {
                    case '.': return acre.open;
                    case '|': return acre.wood;
                    case '#': return acre.lumberyard;

                    default:
                        throw new Exception("parse error");
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Generation: " + this.Generation);
            for (int row = 0; row <= dimension + 1; row++)
            {
                sb.Append(Environment.NewLine);
                for (int col = 0; col <= dimension + 1; col++)
                {
                    switch (acres[row, col])
                    {
                        case acre.open:
                            sb.Append('.');
                            break;
                        case acre.wood:
                            sb.Append('|');
                            break;
                        case acre.lumberyard:
                            sb.Append('#');
                            break;
                    }
                }
            }
            return sb.ToString();
        }

        internal void SkipForward(int skip)
        {
            Generation += skip;
        }

        public enum acre
        {
            open = 0,
            wood,
            lumberyard
        }
    }
}
