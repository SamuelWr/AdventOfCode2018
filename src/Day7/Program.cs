using System;
using System.Collections.Generic;
using System.Linq;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = ParseInput(System.IO.File.ReadLines("input.txt"));
            Test();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("A:");
            Console.WriteLine("\tResult: " + A(input));
            Console.WriteLine("in: " + watch.ElapsedMilliseconds + "ms");

            watch.Restart();
            Console.WriteLine("B:");
            Console.WriteLine("\tResult: " + B(input));
            Console.WriteLine("in: " + watch.ElapsedMilliseconds + "ms");
        }

        private static (char step, char requires)[] ParseInput(IEnumerable<string> input)
        {
            string pattern = @"Step (.) must be finished before step (.) can begin.";
            var regex = new System.Text.RegularExpressions.Regex(pattern);

            return input
                .Select(l => regex.Match(l))
                .Select(match => (match.Groups[2].Value.Single(), match.Groups[1].Value.Single()))
                .ToArray();
        }

        private static void Test()
        {
            var testInput = @"Step C must be finished before step A can begin.
Step C must be finished before step F can begin.
Step A must be finished before step B can begin.
Step A must be finished before step D can begin.
Step B must be finished before step E can begin.
Step D must be finished before step E can begin.
Step F must be finished before step E can begin.";
            var input = ParseInput(testInput.Split(new[] { '\n' }).Select(l => l.Trim()));

            string resA = A(input);
            if (resA == "CABDFE") Console.WriteLine("Test A OK");
            else Console.WriteLine("Test A FAIL, returned \"" + resA + "\" expected \"CABDFE\"");

            int resB = B(input, 2, 0);
            if (resB == 15) Console.WriteLine("Test B OK");
            else Console.WriteLine("Test B FAIL, returned " + resB + " expected 15");
        }



        private static string A((char step, char requires)[] input)
        {
            var steps = input.SelectMany(i => new[] { i.step, i.requires }).Distinct().ToArray();
            var requirements = steps.ToDictionary(step => step, step => new HashSet<char>());
            foreach (var (step, requires) in input)
            {
                requirements[step].Add(requires);
            }

            var result = new List<char>();

            while (requirements.Any())
            {
                var first = requirements.Where(r => !r.Value.Any()).Select(KvP => KvP.Key).OrderBy(k => k).First();
                result.Add(first);
                requirements.Remove(first);
                foreach (var item in requirements.Values)
                {
                    item.Remove(first);
                }
            }

            return string.Join("", result);
        }

        private static int B((char step, char requires)[] input, int workers = 5, int baseTime = 60)
        {
            var steps = input.SelectMany(i => new[] { i.step, i.requires }).Distinct().ToArray();
            var remainingSteps = steps.ToDictionary(step => step, step => new HashSet<char>());
            foreach (var (step, requires) in input)
            {
                remainingSteps[step].Add(requires);
            }

            int time = 0;
            var runningTasks = new List<(char step, int finishTime)>();

            while (remainingSteps.Any())
            {
                //Handle finished tasks
                var finishedSteps = runningTasks.Where(t => t.finishTime <= time).Select(t => t.step);

                foreach (var finished in finishedSteps)
                {
                    remainingSteps.Remove(finished);
                    foreach (var requirements in remainingSteps.Values)
                    {
                        requirements.Remove(finished);
                    }
                }
                runningTasks.RemoveAll(task => task.finishTime <= time);

                //queue available tasks
                var availableWorkers = workers - runningTasks.Count;
                var tasksToStart = remainingSteps
                    .Where(s => !s.Value.Any())
                    .Select(KvP => KvP.Key)
                    .Where(s => !runningTasks.Any(t => t.step == s))
                    .OrderBy(s => s)
                    .Take(availableWorkers);
                foreach (var task in tasksToStart)
                {
                    runningTasks.Add((task, time + taskTime(task)));
                }

                //move time to next finished task (if any)
                if (runningTasks.Any())
                {
                    time = runningTasks.Min(t => t.finishTime);
                }
            }

            return time;

            int taskTime(char step)
            {
                return baseTime + step - 'A' + 1;
            }
        }
    }
}
