using System;
using System.Collections.Generic;
using System.Linq;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "440231";
            Test();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("A: ");
            Console.WriteLine("\tresult: " + A(int.Parse(input)));
            Console.WriteLine("\tin: " + watch.ElapsedMilliseconds + "ms");

            watch.Restart();
            Console.WriteLine("B: ");
            Console.WriteLine("\tresult: " + B(input));
            Console.WriteLine("\tin: " + watch.ElapsedMilliseconds + "ms");
        }

        private static void Test()
        {
            bool ok = true;
            int inputA;
            string resA;
            string expectedA;
            //If the Elves think their skill will improve after making 9 recipes, the scores of the ten recipes after the first nine on the scoreboard would be 5158916779 (highlighted in the last line of the diagram).
            inputA = 9;
            expectedA = "5158916779";
            resA = A(inputA);
            if (resA != expectedA)
            {
                ok = false;
                Console.WriteLine($"Test A Failed for input {inputA}! result was {resA}, expected {expectedA}");
            }
            //After 5 recipes, the scores of the next ten would be 0124515891.
            inputA = 5;
            expectedA = "0124515891";
            resA = A(inputA);
            if (resA != expectedA)
            {
                ok = false;
                Console.WriteLine($"Test A Failed for input {inputA}! result was {resA}, expected {expectedA}");
            }
            //After 18 recipes, the scores of the next ten would be 9251071085.
            inputA = 18;
            expectedA = "9251071085";
            resA = A(inputA);
            if (resA != expectedA)
            {
                ok = false;
                Console.WriteLine($"Test A Failed for input {inputA}! result was {resA}, expected {expectedA}");
            }
            //After 2018 recipes, the scores of the next ten would be 5941429882.
            inputA = 2018;
            expectedA = "5941429882";
            resA = A(inputA);
            if (resA != expectedA)
            {
                ok = false;
                Console.WriteLine($"Test A Failed for input {inputA}! result was {resA}, expected {expectedA}");
            }

            if (ok) Console.WriteLine("Test A: OK");


            //B
            ok = true;
            string inputB;
            int resB;
            int expectedB;

            //51589 first appears after 9 recipes.
            inputB = "51589";
            expectedB = 9;
            resB = B(inputB);
            if (resB != expectedB)
            {
                ok = false;
                Console.WriteLine($"Test B Failed for input {inputB}! result was {resB}, expected {expectedB}");
            }

            //01245 first appears after 5 recipes.
            inputB = "01245";
            expectedB = 5;
            resB = B(inputB);
            if (resB != expectedB)
            {
                ok = false;
                Console.WriteLine($"Test B Failed for input {inputB}! result was {resB}, expected {expectedB}");
            }

            //92510 first appears after 18 recipes.
            inputB = "92510";
            expectedB = 18;
            resB = B(inputB);
            if (resB != expectedB)
            {
                ok = false;
                Console.WriteLine($"Test B Failed for input {inputB}! result was {resB}, expected {expectedB}");
            }

            //59414 first appears after 2018 recipes.
            inputB = "59414";
            expectedB = 2018;
            resB = B(inputB);
            if (resB != expectedB)
            {
                ok = false;
                Console.WriteLine($"Test B Failed for input {inputB}! result was {resB}, expected {expectedB}");
            }

            if (ok) Console.WriteLine("Test B: OK");
        }

        private static int B(string input)
        {
            int[] endSuffix = input.Select(c => int.Parse("" + c)).ToArray();

            List<int> recipies = new List<int> { 3, 7 };
            int elfOneRecipe = 0;
            int elfTwoRecipe = 1;
            while (true)
            {
                // new recipies
                var sum = (recipies[elfOneRecipe] + recipies[elfTwoRecipe]).ToString(); ;

                recipies.Add(int.Parse("" + sum[0]));
                if (isEndCondition())
                    return recipies.Count - input.Length;
                if (sum.Length == 2)

                {
                    recipies.Add(int.Parse("" + sum[1]));
                    if (isEndCondition())
                        return recipies.Count - input.Length;
                }
                // elves change recipies
                elfOneRecipe = (elfOneRecipe + 1 + recipies[elfOneRecipe]) % recipies.Count;
                elfTwoRecipe = (elfTwoRecipe + 1 + recipies[elfTwoRecipe]) % recipies.Count;
            }
            bool isEndCondition()
            {
                if (recipies.Count < input.Length)
                    return false;

                for (int i = 0; i < input.Length; i++)
                {
                    if (endSuffix[input.Length - 1 - i] != recipies[recipies.Count - 1 - i])
                        return false;
                }
                return true;                
            }
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
                    recipies.Add(int.Parse("" + sum[0]));
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
