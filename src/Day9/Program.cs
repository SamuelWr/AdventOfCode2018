using System;
using System.Linq;

namespace Day9
{
    public class Program
    {
        static void Main(string[] args)
        {
            var numPlayers = 448;
            var maxMarble = 71628;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("A:");
            Console.WriteLine("\tresult: " + A(numPlayers, maxMarble));
            Console.WriteLine("\tin: " + watch.ElapsedMilliseconds + "ms");
        }

        public static int A(int numPlayers, int maxMarble)
        {
            int[] scores = new int[numPlayers];
            int currentPlayer = 0;

            //first marble
            Marble current = new Marble(0);
            current.CW = current;
            current.CCW = current;

            // the rest of the marbles
            for (int marble = 1; marble <= maxMarble; marble++)
            {
                var newMarble = new Marble(marble);
                if (marble % 23 == 0)
                {
                    scores[currentPlayer] += marble;

                    var toRemove = current.CCW.CCW.CCW.CCW.CCW.CCW.CCW;
                    toRemove.CW.CCW = toRemove.CCW;
                    toRemove.CCW.CW = toRemove.CW;
                    scores[currentPlayer] += toRemove.Value;

                    current = toRemove.CW;
                }
                else
                {
                    current.CW.InsertCW(newMarble);
                    current = newMarble;
                }

                currentPlayer = (currentPlayer + 1) % numPlayers;

                if (false)
                {
                    //Debug output, compare to example on webpage
                    var initial = current;
                    while (initial.Value != 0) initial = initial.CW;
                    Console.Write($"[{(currentPlayer == 0 ? numPlayers : currentPlayer),2}]");
                    do
                    {
                        if (initial.Value == current.Value) Console.Write($"({initial.Value,2})");
                        else Console.Write($" {initial.Value,2} ");
                        initial = initial.CW;
                    } while (initial.Value != 0);
                    Console.WriteLine("");
                }
            }

            return scores.Max();
        }
    }

    class Marble
    {
        public int Value { get; }
        public Marble CW { get; set; }
        public Marble CCW { get; set; }

        public Marble(int value)
        {
            Value = value;
        }

        internal void InsertCW(Marble newMarble)
        {
            var oldCw = CW;

            this.CW = newMarble;
            newMarble.CCW = this;

            oldCw.CCW = newMarble;
            newMarble.CW = oldCw;
        }
    }
}
