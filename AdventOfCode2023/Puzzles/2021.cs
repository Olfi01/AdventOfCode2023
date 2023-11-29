using AdventOfCode2023.Puzzles.Classes_2021;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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


        [Puzzle(day: 3, part: 1)]
        public static void Day3Part1(string input, Grid display, Label outputLabel)
        {
            
            outputLabel.Content = null;
        }
    }
}
