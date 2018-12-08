﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {            
            ParseInput(File.ReadAllText("input.txt"));
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
