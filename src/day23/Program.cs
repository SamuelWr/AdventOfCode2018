using System;
using System.Linq;

namespace day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("input.txt");
            Console.WriteLine("A: " + A(input));
        }

        private static int A(string[] input)
        {
            var bots = input.Select(i => Nanobot.fromString(i)).ToArray();
            Nanobot best = bots[0];
            foreach (var bot in bots)
            {
                if (bot.r > best.r) best = bot;
            }

            return bots.Count(b => b.Distance(best) <= best.r);
        }
    }
    struct Nanobot
    {
        public readonly int x;
        public readonly int y;
        public readonly int z;
        public readonly int r;
        private Nanobot(int x, int y, int z, int r)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.r = r;
        }
        public static Nanobot fromString(string s)
        {
            const string pattern = @"pos=<(-?\d+),(-?\d+),(-?\d+)>, r=(\d+)";
            var match = System.Text.RegularExpressions.Regex.Match(s, pattern);
            if (match.Success)
            {
                return new Nanobot(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
            }
            throw new ArgumentException("Not pattern!");
        }

        internal int Distance(Nanobot other)
        {
            int dx = this.x - other.x;
            int dy = this.y - other.y;
            int dz = this.z - other.z;
            dx = dx > 0 ? dx : -dx;
            dy = dy > 0 ? dy : -dy;
            dz = dz > 0 ? dz : -dz;
            return dx + dy + dz;
        }
    }
}
