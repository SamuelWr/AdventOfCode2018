using System;
using System.Collections.Generic;
using System.Linq;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("input.txt");
            Test();
            (var map, var carts) = parseInput(input);
            Console.WriteLine(A(map, carts));
        }

        static void Test()
        {
            var testinput =
@"/->-\        
|   |  /----\
| /-+--+-\  |
| | |  | v  |
\-+-/  \-+--/
  \------/   ".Split("\r\n");
            var t = parseInput(testinput);
            var testA = A(t.map, t.carts);
            if (testA != (7, 3)) Console.WriteLine("Fail A");
            else Console.WriteLine("A Test OK.");
        }

        private static (int X, int Y) A(char[,] map, Cart[] carts)
        {
            while (true)
            {
                var orderedCarts = carts.OrderBy(c => c.Row).ThenBy(c => c.Column).ToArray();
                foreach (var cart in orderedCarts)
                {
                    cart.Move();
                    cart.Turn(map[cart.Row, cart.Column]);
                    if (carts.Any(c => c.Id != cart.Id && c.Row == cart.Row && c.Column == cart.Column))
                        return (cart.Column, cart.Row);
                }
            }
        }

        static (char[,] map, Cart[] carts) parseInput(string[] input)
        {
            var map = new char[input.Length, input[0].Length];
            var carts = new List<Cart>();
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[0].Length; j++)
                {
                    var character = input[i][j];
                    switch (character)
                    {
                        case ' ':
                        case '|':
                        case '-':
                        case '/':
                        case '\\':
                        case '+':
                            map[i, j] = character;
                            break;
                        case '>':
                            map[i, j] = '-';
                            carts.Add(new Cart { Row = i, Column = j, Direction = Direction.right });
                            break;
                        case '<':
                            map[i, j] = '-';
                            carts.Add(new Cart { Row = i, Column = j, Direction = Direction.left });
                            break;
                        case '^':
                            map[i, j] = '|';
                            carts.Add(new Cart { Row = i, Column = j, Direction = Direction.up });
                            break;
                        case 'v':
                            map[i, j] = '|';
                            carts.Add(new Cart { Row = i, Column = j, Direction = Direction.down });
                            break;
                        default:
                            throw new ArgumentException();
                    }
                }
            }

            return (map, carts.ToArray());
        }
    }

    internal class Cart
    {
        private static int id = 0;

        public int Id { get; }
        public int Row { get; set; }
        public int Column { get; set; }
        public Direction Direction { get; set; }
        private int turnIndex = 0;
        public Cart()
        {
            Id = ++id;
        }
        public void Move()
        {
            switch (Direction)
            {
                case Direction.up:
                    Row--;
                    break;
                case Direction.down:
                    Row++;
                    break;
                case Direction.left:
                    Column--;
                    break;
                case Direction.right:
                    Column++;
                    break;
            }
        }
        public void Turn(char tile)
        {
            if (tile == '+')
                TurnAtIntersection();
            else if (tile == '\\')
            {
                switch (Direction)
                {
                    case Direction.up:
                        this.Direction = Direction.left;
                        break;
                    case Direction.down:
                        this.Direction = Direction.right;
                        break;
                    case Direction.left:
                        this.Direction = Direction.up;
                        break;
                    case Direction.right:
                        this.Direction = Direction.down;
                        break;
                };
            }
            else if (tile == '/')
            {
                switch (Direction)
                {
                    case Direction.up:
                        this.Direction = Direction.right;
                        break;
                    case Direction.down:
                        this.Direction = Direction.left;
                        break;
                    case Direction.left:
                        this.Direction = Direction.down;
                        break;
                    case Direction.right:
                        this.Direction = Direction.up;
                        break;
                };
            }
            else if (tile == '-' || tile == '|') ; // go straight
            else throw new Exception();

        }
        private void TurnAtIntersection()
        {
            if (turnIndex % 3 == 0)
            { //left
                switch (Direction)
                {
                    case Direction.up:
                        this.Direction = Direction.left;
                        break;
                    case Direction.down:
                        this.Direction = Direction.right;
                        break;
                    case Direction.left:
                        this.Direction = Direction.down;
                        break;
                    case Direction.right:
                        this.Direction = Direction.up;
                        break;
                }
            }
            if (turnIndex % 3 == 1)
            { //straight
            }
            if (turnIndex % 3 == 2)
            { //right
                switch (Direction)
                {
                    case Direction.up:
                        this.Direction = Direction.right;
                        break;
                    case Direction.down:
                        this.Direction = Direction.left;
                        break;
                    case Direction.left:
                        this.Direction = Direction.up;
                        break;
                    case Direction.right:
                        this.Direction = Direction.down;
                        break;
                }
            }

            turnIndex++;
        }

    }

    enum Direction
    {
        up, down, left, right
    }
}
