using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Puzzles.Classes_2021
{
    public record BingoBoard
    {
        public int[,] Numbers { get; }
        public bool[,] Marked { get; }
        public BingoBoard(string input)
        {
            string[] lines = input.Split("\n");
            Numbers = new int[5,5];
            Marked = new bool[5,5];
            for (int line = 0; line < 5; line++)
            {
                for (int num = 0; num < 5; num++)
                {
                    Numbers[line, num] = int.Parse(lines[line][(num * 3)..Math.Min(num * 3 + 3, lines[line].Length)]);
                    Marked[line, num] = false;
                }
            }
        }

        public void Mark(int drawn)
        {
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (Numbers[row, col] == drawn) Marked[row, col] = true;
                }
            }
        }

        public bool HasBingo()
        {
            for (int i = 0; i < 5; i++)
            {
                bool horizontal = true;
                bool vertical = true;
                for (int x = 0; x < 5; x++)
                {
                    if (!Marked[i, x]) horizontal = false;
                    if (!Marked[x, i]) vertical = false;
                    if (!horizontal && !vertical) break;
                }
                if (horizontal || vertical) return true;
            }
            return false;
        }

        public int CalculateScore(int lastDrawn)
        {
            int sum = 0;
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (!Marked[row, col]) sum += Numbers[row, col];
                }
            }
            return sum * lastDrawn;
        }
    }
}
