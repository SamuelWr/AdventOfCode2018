using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {            
            var nodes = ParseInput(File.ReadAllText("input.txt"));
            Test();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("A:");
            Console.WriteLine("\tresult: " +A(nodes));
            Console.WriteLine("\tin: " +watch.ElapsedMilliseconds + "ms");

            watch.Restart();
            Console.WriteLine("B:");
            Console.WriteLine("\tresult: " + B(nodes));
            Console.WriteLine("\tin: " + watch.ElapsedMilliseconds + "ms");
        }

        private static void Test()
        {
            var testInput = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";

            var resA = A(ParseInput(testInput));
            if (resA == 138) Console.WriteLine("Test A OK");
            else Console.WriteLine($"Test A FAIL! expected {138} returned {resA}");

            var resB = B(ParseInput(testInput));
            if (resB == 66) Console.WriteLine("Test B OK");
            else Console.WriteLine($"Test B FAIL! expected {66} returned {resB}");
        }

        static int A(Node root)
        {
            return root.SumMetaData();
        }
        static int B(Node root)
        {
            return root.GetValue();
        }

        static Node ParseInput(string s)
        {
            var numbers = s.Split().Select(n => byte.Parse(n)).ToArray();
            var stream = new MemoryStream(numbers);
            var nodes = Node.FromData(stream);
            if (stream.ReadByte() != -1) throw new Exception("Did not read all values!");
            return nodes;
        }
    }

    class Node
    {
        public List<Node> Children { get; }
        public List<byte> Metadata { get; set; }
        private Node(List<Node> children, List<byte> metadata)
        {
            Children = children;
            Metadata = metadata;
        }

        public int SumMetaData()
        {
            return Metadata.Select(b => Convert.ToInt32(b)).Sum() + Children.Sum(c => c.SumMetaData());
        }

        public int GetValue()
        {
            if (!Children.Any())
                return Metadata.Select(b => Convert.ToInt32(b)).Sum();

            int sum = 0;
            foreach (var index in Metadata)
            {
                if (index < 1 || index > Children.Count)
                    continue;
                sum += Children[index - 1].GetValue();
            }
            return sum;
        }

        public static Node FromData(Stream data)
        {
            var numChildren = data.ReadByte();
            var numMetadata = data.ReadByte();
            var children = new List<Node>(numChildren);
            for (int i = 0; i < numChildren; i++)
            {
                children.Add(Node.FromData(data));
            }
            var metadata = new Byte[numMetadata];
            var bytesRead = data.Read(metadata);
            if (bytesRead != numMetadata) throw new Exception("End of stream!");
            return new Node(children, metadata.ToList());
        }
    }
}
