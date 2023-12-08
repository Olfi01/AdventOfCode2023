using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Puzzles.Classes_2023
{
    public record DesertMap
    {
        public char[] Instructions { get; set; }
        public Dictionary<string, DesertNode> Nodes { get; }

        public DesertMap(string input)
        {
            string[] split = input.Split("\n\n");
            Instructions = split[0].ToCharArray();
            Nodes = split[1].Split('\n').Where(s => !string.IsNullOrEmpty(s)).Select(l => new DesertNode(l)).ToDictionary(node => node.Name);
        }
    }

    public record DesertNode
    {
        public string Name { get; }
        public string Left { get; }
        public string Right { get; }
        public DesertNode(string line)
        {
            Name = line[..3];
            Left = line[7..10];
            Right = line[12..15];
        }
    }
}
