using System;
using System.Collections.Generic;
using System.Linq;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            int input = 440231;
            Test();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("A: ");
            Console.WriteLine("\tresult: " + A(input));
            Console.WriteLine("\tin: " +watch.ElapsedMilliseconds+"ms");
        }

        private static void Test()
        {
            bool ok = true;
            int input;
            string resA;
            string expected;
            //If the Elves think their skill will improve after making 9 recipes, the scores of the ten recipes after the first nine on the scoreboard would be 5158916779 (highlighted in the last line of the diagram).
            input = 9;
            expected = "5158916779";
            resA = A(input);
            if (resA != expected)
            {
                ok = false;
                Console.WriteLine($"Test A Failed for input {input}! result was {resA}, expected {expected}");
            }
            //After 5 recipes, the scores of the next ten would be 0124515891.
            input = 5;
            expected = "0124515891";
            resA = A(input);
            if (resA != expected)
            {
                ok = false;
                Console.WriteLine($"Test A Failed for input {input}! result was {resA}, expected {expected}");
            }
            //After 18 recipes, the scores of the next ten would be 9251071085.
            input = 18;
            expected = "9251071085";
            resA = A(input);
            if (resA != expected)
            {
                ok = false;
                Console.WriteLine($"Test A Failed for input {input}! result was {resA}, expected {expected}");
            }
            //After 2018 recipes, the scores of the next ten would be 5941429882.
            input = 2018;
            expected = "5941429882";
            resA = A(input);
            if (resA != expected)
            {
                ok = false;
                Console.WriteLine($"Test A Failed for input {input}! result was {resA}, expected {expected}");
            }

            if (ok) Console.WriteLine("Test A: OK");
        }

        static string A(int input)
        {
            List<int> recipies = new List<int> { 3, 7 };
            int elfOneRecipe = 0;
            int elfTwoRecipe = 1;

            while (recipies.Count < input + 10)
            {
                // new recipies
                var sum = (recipies[elfOneRecipe] + recipies[elfTwoRecipe]).ToString(); ;
                if (sum.Length == 1)
                    recipies.Add(int.Parse(""+sum[0]));
                else
                {
                    recipies.Add(int.Parse("" + sum[0]));
                    recipies.Add(int.Parse("" + sum[1]));

                }
                // elves change recipies
                elfOneRecipe = (elfOneRecipe + 1 + recipies[elfOneRecipe]) % recipies.Count;
                elfTwoRecipe = (elfTwoRecipe + 1 + recipies[elfTwoRecipe]) % recipies.Count;
            }

            return string.Join("", recipies.Skip(input).Take(10));
        }
    }
}
