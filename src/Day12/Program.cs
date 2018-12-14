using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day12
{
    class Program
    {
        static readonly string initial_state = "#.#####.#.#.####.####.#.#...#.......##..##.#.#.#.###..#.....#.####..#.#######.#....####.#....##....#";
        static readonly string transformations = @"##.## => .
#.#.. => .
..... => .
##..# => #
###.. => #
.##.# => .
..#.. => #
##.#. => #
.##.. => .
#..#. => .
###.# => #
.#### => #
.#.## => .
#.##. => #
.###. => #
##### => .
..##. => .
#.#.# => .
...#. => #
..### => .
.#.#. => #
.#... => #
##... => #
.#..# => #
#.### => #
#..## => #
....# => .
####. => .
#...# => #
#.... => .
...## => .
..#.# => #";



        static void Main(string[] args)
        {
            Dictionary<string, string> rules = ParseRules(transformations);
            Console.WriteLine("A: " + A(initial_state, rules));
            Console.WriteLine("B: " + B(initial_state, rules));
        }

        private static Dictionary<string, string> ParseRules(string transformations)
        {
            return transformations.Split("\r\n")
                .Select(l => l.Split(" => "))
                .ToDictionary(p => p[0], p => p[1]);
        }

        static int A(string intialState, Dictionary<string, string> transformations)
        {
            HashSet<int> currentState = new HashSet<int>();
            int minIndex = 0;
            int maxIndex = initial_state.Length - 1;
            for (int i = 0; i < initial_state.Length; i++)
            {
                if (initial_state[i] == '#') currentState.Add(i);
            }
            foreach (var generation in Enumerable.Range(1, 20))
            {
                minIndex--; maxIndex++;
                var nextState = new HashSet<int>();
                for (int i = minIndex; i <= maxIndex; i++)
                {
                    string surrounding = (currentState.Contains(i - 2) ? "#" : ".")
                        + (currentState.Contains(i - 1) ? "#" : ".")
                        + (currentState.Contains(i - 0) ? "#" : ".")
                        + (currentState.Contains(i + 1) ? "#" : ".")
                        + (currentState.Contains(i + 2) ? "#" : ".");
                    if (transformations[surrounding] == "#") nextState.Add(i);
                }
                currentState = nextState;
            }
            return currentState.Sum();
        }

        static long B(string intialState, Dictionary<string, string> transformations)
        {
            long MAX_GENERATION = 50000000000;

            Dictionary<string, (long leftMost, long generation)> seenStates = new Dictionary<string, (long, long)>(); //Key: filled pots, value: position of leftmost
            seenStates[intialState.Trim('.')] = (0, 0);

            HashSet<long> currentState = new HashSet<long>();
            long minIndex = 0;
            long maxIndex = initial_state.Length - 1;
            for (int i = 0; i < initial_state.Length; i++)
            {
                if (initial_state[i] == '#') currentState.Add(i);
            }
            for (long generation = 1; generation <= MAX_GENERATION; generation++)
            {
                minIndex = currentState.Min() - 1;
                maxIndex = currentState.Max() + 1;
                var nextState = new HashSet<long>();
                for (long i = minIndex; i <= maxIndex; i++)
                {
                    string surrounding = (currentState.Contains(i - 2) ? "#" : ".")
                        + (currentState.Contains(i - 1) ? "#" : ".")
                        + (currentState.Contains(i - 0) ? "#" : ".")
                        + (currentState.Contains(i + 1) ? "#" : ".")
                        + (currentState.Contains(i + 2) ? "#" : ".");
                    if (transformations[surrounding] == "#") nextState.Add(i);
                }
                currentState = nextState;

                // caching...
                var sb = new StringBuilder();
                var min = currentState.Min();
                var max = currentState.Max();
                for (long i = min; i <= max; i++)
                {
                    if (currentState.Contains(i)) sb.Append('#');
                    else sb.Append('.');
                }
                var state = sb.ToString();

                if (seenStates.ContainsKey(state))
                {
                    var seen = seenStates[state];
                    Console.WriteLine("Cache hit!");

                    var generationDiff = generation - seen.generation;
                    var leftDiff = min - seen.leftMost;
                    var jumps = (MAX_GENERATION - generation) / generationDiff;

                    generation += jumps * generationDiff;
                    HashSet<long> jumpedState = new HashSet<long>();
                    foreach (var item in currentState)
                    {
                        jumpedState.Add(item + jumps * leftDiff);
                    }
                    currentState = jumpedState;
                }
                else
                {
                    seenStates[state] = (min, generation);
                }

            }
            return currentState.Sum();
        }

    }
}
