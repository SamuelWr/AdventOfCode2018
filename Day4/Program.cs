using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var input = Readinput();
            var Guards = parseGuards(input);

            A(Guards);
            B(Guards);
            watch.Stop();
            Console.WriteLine("Total elapsed: " + watch.ElapsedMilliseconds + "ms");
        }

        static void A(Dictionary<int, Guard> guards)
        {

            var sleepiestGuard = guards.Values.OrderByDescending(g => g.TotalSleep).First();
            var sleepiestMinute = sleepiestGuard.SleepMinutes.Select((value, index) => new { value, index }).GroupBy(m => m.value).OrderByDescending(g => g.Key).First().Single();
            Console.WriteLine("A:");
            Console.WriteLine("Sleepiest guard: " + sleepiestGuard.Id);
            Console.WriteLine("Sleepiest minute: " + sleepiestMinute.index + " with value: " + sleepiestMinute.value);
            Console.WriteLine($"Answer: {sleepiestGuard.Id}*{sleepiestMinute.index} = {sleepiestMinute.index * sleepiestGuard.Id}");

        }

        static void B(Dictionary<int, Guard> guards)
        {
            var minutes = guards.Values.SelectMany(g => g.SleepMinutes.Select((value, index) => new { minute = index, asleep = value, guardId = g.Id }));

            var mostAsleep = minutes.GroupBy(m => m.asleep).OrderByDescending(g => g.Key).First().Single();
            Console.WriteLine("B:");
            Console.WriteLine($"sleepiest minute was minute {mostAsleep.minute}, which guard {mostAsleep.guardId} spent sleeping {mostAsleep.asleep} times");
            Console.WriteLine("Answer: " + (mostAsleep.guardId * mostAsleep.minute));

        }

        private static Dictionary<int, Guard> parseGuards(IEnumerable<GuardEvent> events)
        {
            var Guards = new Dictionary<int, Guard>();
            Guard currentGuard = null;

            foreach (var item in events)
            {
                if (item is GuardEvent.GuardChange change)
                {
                    if (!Guards.TryGetValue(change.Id, out currentGuard))
                    {
                        currentGuard = new Guard(change.Id);
                        Guards[change.Id] = currentGuard;
                    }
                }
                else if (item is GuardEvent.FallAsleep asleep)
                {
                    var minute = asleep.TimeStamp.Minute;
                    for (int i = minute; i < currentGuard.SleepMinutes.Length; i++)
                    {
                        currentGuard.SleepMinutes[i]++;
                    }
                }
                else if (item is GuardEvent.WakeUp wake)
                {
                    var minute = wake.TimeStamp.Minute;
                    for (int i = minute; i < currentGuard.SleepMinutes.Length; i++)
                    {
                        currentGuard.SleepMinutes[i]--;
                    }
                }
            }

            return Guards;
        }

        static IEnumerable<GuardEvent> Readinput()
        {
            var lines = System.IO.File.ReadLines("input.txt");
            return lines.Select(l => GuardEvent.FromString(l)).OrderBy(l => l.TimeStamp).ToArray();
        }
    }
    class Guard
    {
        public int Id { get; }
        public int[] SleepMinutes { get; } = new int[60];

        private int? totalSleep;
        public int TotalSleep => totalSleep.HasValue ? totalSleep.Value : (int)(totalSleep = SleepMinutes.Sum());
        public Guard(int id)
        {
            Id = id;
        }
    }

    abstract class GuardEvent
    {
        private static readonly Regex changeRegex;
        private static readonly Regex asleepRegex;
        private static readonly Regex wakeRegex;

        static GuardEvent()
        {
            string guardChangePattern = @"\[(\d\d\d\d-\d\d-\d\d \d\d:\d\d)\] Guard #(\d+) begins shift";
            string fallAsleepPattern = @"\[(\d\d\d\d-\d\d-\d\d \d\d:\d\d)\] falls asleep";
            string wakeUpPattern = @"\[(\d\d\d\d-\d\d-\d\d \d\d:\d\d)\] wakes up";

            changeRegex = new Regex(guardChangePattern);
            asleepRegex = new Regex(fallAsleepPattern);
            wakeRegex = new Regex(wakeUpPattern);
        }


        public DateTime TimeStamp { get; }
        private GuardEvent(DateTime timeStamp)
        {
            TimeStamp = timeStamp;
        }

        public class GuardChange : GuardEvent
        {
            public GuardChange(DateTime time, int id) : base(time)
            {
                this.Id = id;
            }
            public int Id { get; }
        }
        public class WakeUp : GuardEvent
        {
            public WakeUp(DateTime time) : base(time)
            {

            }
        }
        public class FallAsleep : GuardEvent
        {
            public FallAsleep(DateTime time) : base(time)
            {

            }
        }
        public static GuardEvent FromString(string s)
        {
            Match match;

            match = changeRegex.Match(s);
            if (match.Success)
            {
                return new GuardChange(DateTime.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
            }
            match = wakeRegex.Match(s);
            if (match.Success)
            {
                return new WakeUp(DateTime.Parse(match.Groups[1].Value));
            }
            match = asleepRegex.Match(s);
            if (match.Success)
            {
                return new FallAsleep(DateTime.Parse(match.Groups[1].Value));
            }
            throw new ArgumentException("No match!");
        }
    }


}
