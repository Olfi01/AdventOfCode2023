﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdventOfCode2023.Puzzles
{
    public class _2023
    {
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
            foreach (string line in lines)
            {
                int lowestNum = -1;
                int lowestIdx = 10000;
                int highestNum = -1;
                int highestIdx = -1;
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (line.Contains(numbers[i]) && line.IndexOf(numbers[i]) < lowestIdx)
                    {
                        lowestNum = i;
                        lowestIdx = line.IndexOf(numbers[i]);
                    }
                    if (line.Contains(numberWords[i]) && line.IndexOf(numberWords[i]) < lowestIdx)
                    {
                        lowestNum = i;
                        lowestIdx = line.IndexOf(numberWords[i]);
                    }
                    if (line.LastIndexOf(numbers[i]) > highestIdx) {
                        highestNum = i;
                        highestIdx = line.LastIndexOf(numbers[i]);
                    }
                    if (line.LastIndexOf(numberWords[i]) > highestIdx)
                    {
                        highestNum = i;
                        highestIdx = line.LastIndexOf(numberWords[i]);
                    }
                }
                sum += lowestNum * 10 + highestNum;
            }

            outputLabel.Content = sum;
        }
    }
}
