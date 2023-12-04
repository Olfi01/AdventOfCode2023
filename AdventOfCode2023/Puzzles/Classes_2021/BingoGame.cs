using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Puzzles.Classes_2021
{
    public record BingoGame
    {
        public int[] Numbers { get; }
        public List<BingoBoard> Boards { get; }
        
        public BingoGame(string input)
        {
            string[] segments = input.Split("\n\n");
            Numbers = segments[0].Split(",").Select(int.Parse).ToArray();
            Boards = new();

            for (int i = 1; i < segments.Length; i++)
            {
                Boards.Add(new BingoBoard(segments[i]));
            }
        }
    }
}
