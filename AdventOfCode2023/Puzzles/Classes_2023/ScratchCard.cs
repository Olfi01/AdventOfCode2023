using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Puzzles.Classes_2023
{
    public record ScratchCard
    {
        public int Id { get; }
        public int[] WinningNumbers { get; }
        public int[] MyNumbers { get; }
        public int MatchingNumbers { get => MyNumbers.Count(n => WinningNumbers.Contains(n)); }
        public int Copies { get; set; } = 1;

        public ScratchCard(string input)
        {
            string[] split = input.Split(':');
            Id = int.Parse(split[0][5..]);
            string[] numbers = split[1].Split('|');
            WinningNumbers = numbers[0].Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToArray();
            MyNumbers = numbers[1].Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToArray();
        }

        public int GetPoints()
        {
            if (MatchingNumbers == 0) return 0;
            return (int)Math.Pow(2, MatchingNumbers - 1);
        }
    }
}
