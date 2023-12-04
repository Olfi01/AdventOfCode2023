using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AdventOfCode2023.Puzzles
{
    public class _2023
    {
        #region Day 1
        private static string[] ReadInputDay1(string input)
        {
            return input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }


        [Puzzle(day: 1, part: 1)]
        public static void Day1Part1(string input, Grid display, Label outputLabel)
        {
            string[] lines = ReadInputDay1(input);
            char[] numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int sum = 0;
            foreach (string line in lines)
            {
                sum += Array.IndexOf(numbers, line.First(x => numbers.Contains(x))) * 10;
                sum += Array.IndexOf(numbers, line.Last(x => numbers.Contains(x)));
            }

            outputLabel.Content = sum;
        }


        [Puzzle(day: 1, part: 2)]
        public static void Day1Part2(string input, Grid display, Label outputLabel)
        {
            string[] lines = ReadInputDay1(input);
            string[] numbers = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string[] numberWords = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            int sum = 0;
            TextBlock textBlock = new();
            display.Children.Add(textBlock);
            
            foreach (string line in lines)
            {
                int lowestNum = -1;
                int lowestIdx = 10000;
                int lowestLength = 0;
                int highestNum = -1;
                int highestIdx = -1;
                int highestLength = 0;
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (line.Contains(numbers[i]) && line.IndexOf(numbers[i]) < lowestIdx)
                    {
                        lowestNum = i;
                        lowestIdx = line.IndexOf(numbers[i]);
                        lowestLength = 1;
                    }
                    if (line.Contains(numberWords[i]) && line.IndexOf(numberWords[i]) < lowestIdx)
                    {
                        lowestNum = i;
                        lowestIdx = line.IndexOf(numberWords[i]);
                        lowestLength = numberWords[i].Length;
                    }
                    if (line.LastIndexOf(numbers[i]) > highestIdx) {
                        highestNum = i;
                        highestIdx = line.LastIndexOf(numbers[i]);
                        highestLength = 1;
                    }
                    if (line.LastIndexOf(numberWords[i]) > highestIdx)
                    {
                        highestNum = i;
                        highestIdx = line.LastIndexOf(numberWords[i]);
                        highestLength = numberWords[i].Length;
                    }
                }
                textBlock.Inlines.Add(new Run(line[..lowestIdx]));
                if (lowestIdx == highestIdx)
                {
                    textBlock.Inlines.Add(new Run(line[lowestIdx..(lowestIdx + lowestLength)]) { Foreground = Brushes.Blue });
                }
                else
                {
                    textBlock.Inlines.Add(new Run(line[lowestIdx..(lowestIdx + lowestLength)]) { Foreground = Brushes.Green });
                    textBlock.Inlines.Add(new Run(line[(lowestIdx + lowestLength)..highestIdx]));
                    textBlock.Inlines.Add(new Run(line[highestIdx..(highestIdx + highestLength)]) { Foreground = Brushes.Red });
                }
                textBlock.Inlines.Add(new Run(line[(highestIdx + highestLength)..]));
                textBlock.Inlines.Add(new Run("\n"));

                sum += lowestNum * 10 + highestNum;
            }

            outputLabel.Content = sum;
        }

        #endregion
    }
}
