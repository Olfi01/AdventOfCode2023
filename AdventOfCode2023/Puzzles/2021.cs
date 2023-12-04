using AdventOfCode2023.Helpers;
using AdventOfCode2023.Puzzles.Classes_2021;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using Brushes = System.Windows.Media.Brushes;

namespace AdventOfCode2023.Puzzles
{
    public class _2021
    {
        private static int[] ReadInputDay1(string input)
        {
            return input.Split('\n').Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).ToArray();
        }

        [Puzzle(day: 1, part: 1)]
        public static void Day1Part1(string input, Grid display, Label outputLabel)
        {
            int[] scans = ReadInputDay1(input);
            WpfPlot plot = new();
            plot.Plot.AddSignal(scans);
            display.Children.Add(plot);
            plot.Refresh();
            int increases = 0;
            for (int i = 1; i < scans.Length; i++)
            {
                if (scans[i] > scans[i - 1]) increases++;
            }
            outputLabel.Content = increases;
        }

        [Puzzle(day: 1, part: 2)]
        public static void Day1Part2(string input, Grid _, Label outputLabel)
        {
            int[] scans = ReadInputDay1(input);
            int increases = 0;
            for (int i = 3; i < scans.Length; i++)
            {
                if (scans[i] > scans[i - 3]) increases++;
            }
            outputLabel.Content = increases;
        }

        private static SubmarineInstruction[] ReadInputDay2(string input)
        {
            return input.Split('\n').Where(x => !string.IsNullOrEmpty(x)).Select(x => SubmarineInstruction.From(x)).ToArray();
        }

        [Puzzle(day: 2, part: 1)]
        public static void Day2Part1(string input, Grid display, Label outputLabel)
        {
            SubmarineInstruction[] instructions = ReadInputDay2(input);
            int position = 0;
            int depth = 0;
            foreach (var instruction in instructions)
            {
                switch(instruction.Direction)
                {
                    case SubmarineInstruction.EDirection.Forward:
                        position += instruction.Amount;
                        break;
                    case SubmarineInstruction.EDirection.Up:
                        depth -= instruction.Amount;
                        break;
                    case SubmarineInstruction.EDirection.Down:
                        depth += instruction.Amount;
                        break;
                }
            }
            outputLabel.Content = position * depth;
        }


        [Puzzle(day: 2, part: 2)]
        public static void Day2Part2(string input, Grid display, Label outputLabel)
        {
            SubmarineInstruction[] instructions = ReadInputDay2(input);
            int position = 0;
            int depth = 0;
            int aim = 0;
            foreach (var instruction in instructions)
            {
                switch (instruction.Direction)
                {
                    case SubmarineInstruction.EDirection.Forward:
                        position += instruction.Amount;
                        depth += aim * instruction.Amount;
                        break;
                    case SubmarineInstruction.EDirection.Up:
                        aim -= instruction.Amount;
                        break;
                    case SubmarineInstruction.EDirection.Down:
                        aim += instruction.Amount;
                        break;
                }
            }
            outputLabel.Content = position * depth;
        }

        private static List<string> ReadInputDay3(string input)
        {
            return input.Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToList();
        }

        [Puzzle(day: 3, part: 1)]
        public static void Day3Part1(string input, Grid display, Label outputLabel)
        {
            List<string> lines = ReadInputDay3(input);
            string gammaRate = "";
            string epsilonRate = "";
            for (int i=0; i < lines[0].Length; i++)
            {
                gammaRate += lines.Select(l => l[i]).MostCommon();
                epsilonRate += lines.Select(l => l[i]).LeastCommon();
            }
            int gamma = Convert.ToInt32(gammaRate, 2);
            int epsilon = Convert.ToInt32(epsilonRate, 2);

            outputLabel.Content = gamma * epsilon;
        }


        [Puzzle(day: 3, part: 2)]
        public static void Day3Part2(string input, Grid display, Label outputLabel)
        {
            List<string> lines = ReadInputDay3(input);
            List<string> linesCopy = new(lines);
            for (int i=0; i < lines[0].Length; i++)
            {
                char mostCommon = lines.Select(l => l[i]).MostCommon(tiebreaker: '1');
                lines.RemoveAll(line => line[i] != mostCommon);
                if (lines.Count == 1) break;
            }
            for (int i = 0; i < linesCopy[0].Length; i++)
            {
                char leastCommon = linesCopy.Select(l => l[i]).LeastCommon(tiebreaker: '0');
                linesCopy.RemoveAll(line => line[i] != leastCommon);
                if (linesCopy.Count == 1) break;
            }
            int oxygenGeneratorRating = Convert.ToInt32(lines.First(), 2);
            int co2ScrubberRating = Convert.ToInt32(linesCopy.First(), 2);

            outputLabel.Content = oxygenGeneratorRating * co2ScrubberRating;
        }

        public static BingoGame ReadInputDay4(string input)
        {
            return new BingoGame(input);
        }


        [Puzzle(day: 4, part: 1)]
        public static void Day4Part1(string input, Grid display, Label outputLabel)
        {
            BingoGame game = ReadInputDay4(input);

            BingoBoard? winner = null;

            bool cont = true;
            int lastDrawn = 0;
            foreach (int drawn in game.Numbers)
            {
                if (!cont) break;
                lastDrawn = drawn;
                foreach (BingoBoard board in game.Boards)
                {
                    board.Mark(drawn);
                    if (board.HasBingo())
                    {
                        winner = board;
                        cont = false;
                        break;
                    }
                }
            }

            if (winner != null) outputLabel.Content = winner.CalculateScore(lastDrawn);
        }


        [Puzzle(day: 4, part: 2)]
        public static void Day4Part2(string input, Grid display, Label outputLabel)
        {
            BingoGame game = ReadInputDay4(input);

            int lastDrawn = 0;
            BingoBoard? loser = null;
            foreach (int drawn in game.Numbers)
            {
                if (game.Boards.Count < 1) break;
                lastDrawn = drawn;
                List<BingoBoard> toDelete = new();
                foreach (BingoBoard board in game.Boards)
                {
                    board.Mark(drawn);
                    if (board.HasBingo())
                    {
                        loser = board;
                        toDelete.Add(board);
                    }
                }
                foreach(BingoBoard board in toDelete) game.Boards.Remove(board);
            }

            if (loser != null) outputLabel.Content = loser.CalculateScore(lastDrawn);
        }
    }
}
