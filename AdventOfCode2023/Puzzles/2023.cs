using AdventOfCode2023.Puzzles.Classes_2023;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace AdventOfCode2023.Puzzles
{
    public class _2023
    {
        #region General constants
        private static readonly char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        #endregion

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

        #region Day 2
        public static List<CubeGame> ReadInputDay2(string input)
        {
            return input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).Select(l => new CubeGame(l)).ToList();
        }

        [Puzzle(day: 2, part: 1)]
        public static void Day2Part1(string input, Grid display, Label outputLabel)
        {
            List<CubeGame> games = ReadInputDay2(input);

            int sum = 0;
            foreach(CubeGame game in games)
            {
                if (game.Draws.All(d => IsDrawPossibleWith(d, red: 12, green: 13, blue: 14))) sum += game.Id;
            }

            outputLabel.Content = sum;
        }

        private static bool IsDrawPossibleWith(List<(CubeGame.Color c, int n)> draw, int red, int green, int blue)
        {
            return draw.All(e => IsDrawPossibleWith(e, red, green, blue));
        }

        private static bool IsDrawPossibleWith((CubeGame.Color c, int n) element, int red, int green, int blue)
        {
            return element.c switch
            {
                CubeGame.Color.Red => element.n <= red,
                CubeGame.Color.Green => element.n <= green,
                CubeGame.Color.Blue => element.n <= blue,
                _ => throw new ArgumentOutOfRangeException(nameof(element))
            };
        }

        [Puzzle(day: 2, part: 2)]
        public static void Day2Part2(string input, Grid display, Label outputLabel)
        {
            List<CubeGame> games = ReadInputDay2(input);

            int sum = 0;
            foreach (CubeGame game in games)
            {
                (int minRed, int minGreen, int minBlue) = (0, 0, 0);
                foreach (var draw in game.Draws)
                {
                    (int r, int g, int b) = GetMinCubes(draw);
                    if (r > minRed) minRed = r;
                    if (g > minGreen) minGreen = g;
                    if (b > minBlue) minBlue = b;
                }
                sum += minRed * minGreen * minBlue;
            }

            outputLabel.Content = sum;
        }

        private static (int r, int g, int b) GetMinCubes(List<(CubeGame.Color c, int n)> draw)
        {
            (int r, int g, int b) = (0, 0, 0);
            foreach (var (c, n) in draw)
            {
                switch (c)
                {
                    case CubeGame.Color.Red:
                        r = n;
                        break;
                    case CubeGame.Color.Green:
                        g = n;
                        break;
                    case CubeGame.Color.Blue:
                        b = n;
                        break;
                }
            }
            return (r, g, b);
        }

        #endregion

        #region Day 3
        private static string[] ReadInputDay3(string input)
        {
            return input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }


        [Puzzle(day: 3, part: 1)]
        public static void Day3Part1(string input, Grid display, Label outputLabel)
        {
            string[] engineChart = ReadInputDay3(input);

            int sum = 0;
            TextBlock debug = new();
            display.Children.Add(debug);

            for (int row = 0; row < engineChart.Length; row++)
            {
                string line = engineChart[row];
                int? numberStart = null;
                for (int col = 0; col < line.Length; col++)
                {
                    if (numberStart.HasValue)
                    {
                        if (!digits.Contains(line[col]))
                        {
                            int number = int.Parse(line[numberStart.Value..col]);
                            if (HasAdjacentSymbol(row, from: numberStart.Value, to: col, engineChart))
                            {
                                sum += number;
                                debug.Inlines.Add(new Run(number + "\n") { Foreground = Brushes.Green });
                            }
                            else
                            {
                                debug.Inlines.Add(new Run(number + "\n") { Foreground = Brushes.Red });
                            }
                            numberStart = null;
                        }
                    }
                    else
                    {
                        if (digits.Contains(line[col]))
                        {
                            numberStart = col;
                        }
                    }
                }
                if (numberStart.HasValue)
                {
                    int number = int.Parse(line[numberStart.Value..]);
                    if (HasAdjacentSymbol(row, from: numberStart.Value, to: line.Length - 1, engineChart))
                    {
                        sum += number;
                        debug.Inlines.Add(new Run(number + "\n") { Foreground = Brushes.Green });
                    }
                    else
                    {
                        debug.Inlines.Add(new Run(number + "\n") { Foreground = Brushes.Red });
                    }
                }
            }

            outputLabel.Content = sum;
        }

        private static bool HasAdjacentSymbol(int row, int from, int to, string[] engineChart)
        {
            if (from != 0) from--;
            if (to != engineChart[0].Length - 1) to++;
            if (engineChart[row][from..to].Any(IsSymbol)) return true;
            if (row > 0 && engineChart[row - 1][from..to].Any(IsSymbol)) return true;
            if (row < engineChart.Length - 1 && engineChart[row + 1][from..to].Any(IsSymbol)) return true;
            return false;
        }

        private static bool IsSymbol(char c)
        {
            return c != '.' && !digits.Contains(c);
        }


        [Puzzle(day: 3, part: 2)]
        public static void Day3Part2(string input, Grid display, Label outputLabel)
        {
            string[] engineChart = ReadInputDay3(input);

            int sum = 0;
            TextBlock debug = new();
            display.Children.Add(debug);
            List<(int number, int row, int startIndex, int endIndex)> numbers = new();

            for (int row = 0; row < engineChart.Length; row++)
            {
                string line = engineChart[row];
                int? numberStart = null;
                for (int col = 0; col < line.Length; col++)
                {
                    if (numberStart.HasValue)
                    {
                        if (!digits.Contains(line[col]))
                        {
                            int number = int.Parse(line[numberStart.Value..col]);
                            numbers.Add((number, row, numberStart.Value, col));
                            numberStart = null;
                        }
                    }
                    else
                    {
                        if (digits.Contains(line[col]))
                        {
                            numberStart = col;
                        }
                    }
                }
                if (numberStart.HasValue)
                {
                    int number = int.Parse(line[numberStart.Value..]);
                    numbers.Add((number, row, numberStart.Value, line.Length - 1));
                }
            }

            for (int row = 0; row < engineChart.Length; row++)
            {
                string line = engineChart[row];
                for (int col = 0; col < line.Length; col++)
                {
                    if (IsSymbol(line[col]) && HasTwoAdjacentNumbers(row, col, numbers))
                    {
                        (int num1, int num2) = GetTwoAdjacentNumbers(row, col, numbers);
                        sum += num1 * num2;
                    }
                }
            }

            outputLabel.Content = sum;
        }

        private static bool HasTwoAdjacentNumbers(int row, int col, List<(int number, int row, int startIndex, int endIndex)> numbers)
        {
            return numbers.Count(n => IsNumberAdjacentToGear(n, row, col)) == 2;
        }

        private static bool IsNumberAdjacentToGear((int number, int row, int startIndex, int endIndex) n, int row, int col)
        {
            return n.row >= row - 1 && n.row <= row + 1 && n.startIndex <= col + 1 && n.endIndex >= col;
        }

        private static (int num1, int num2) GetTwoAdjacentNumbers(int row, int col, List<(int number, int row, int startIndex, int endIndex)> numbers)
        {
            var results = numbers.Where(n => IsNumberAdjacentToGear(n, row, col));
            return (results.ElementAt(0).number, results.ElementAt(1).number);
        }
        #endregion

        #region Day 4
        private static ScratchCard[] ReadInputDay4(string input)
        {
            return input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).Select(s => new ScratchCard(s)).ToArray();
        }


        [Puzzle(day: 4, part: 1)]
        public static void Day4Part1(string input, Grid display, Label outputLabel)
        {
            ScratchCard[] cards = ReadInputDay4(input);

            int sum = cards.Sum(c => c.GetPoints());

            outputLabel.Content = sum;
        }


        [Puzzle(day: 4, part: 2)]
        public static void Day4Part2(string input, Grid display, Label outputLabel)
        {
            ScratchCard[] cards = ReadInputDay4(input);

            for (int i = 0; i < cards.Length; i++)
            {
                var card = cards[i];
                for (int j = 1; j <= card.MatchingNumbers; j++)
                {
                    cards[i + j].Copies += card.Copies;
                }
            }

            int sum = cards.Sum(c => c.Copies);

            outputLabel.Content = sum;
        }
        #endregion

        #region Day 5
        private static IslandIslandAlmanac ReadInputDay5(string input)
        {
            return new IslandIslandAlmanac(input);
        }


        [Puzzle(day: 5, part: 1)]
        public static void Day5Part1(string input, Grid display, Label outputLabel)
        {
            IslandIslandAlmanac almanac = ReadInputDay5(input);
            long[] locations = almanac.Seeds
                .Select(s => IslandIslandAlmanac.Convert(s, almanac.SeedToSoil))
                .Select(s => IslandIslandAlmanac.Convert(s, almanac.SoilToFertilizer))
                .Select(f => IslandIslandAlmanac.Convert(f, almanac.FertilizerToWater))
                .Select(w => IslandIslandAlmanac.Convert(w, almanac.WaterToLight))
                .Select(l => IslandIslandAlmanac.Convert(l, almanac.LightToTemperature))
                .Select(t => IslandIslandAlmanac.Convert(t, almanac.TemperatureToHumidity))
                .Select(h => IslandIslandAlmanac.Convert(h, almanac.HumidityToLocation))
                .ToArray();
            outputLabel.Content = locations.Min();
        }


        [Puzzle(day: 5, part: 2)]
        public static void Day5Part2(string input, Grid display, Label outputLabel)
        {
            IslandIslandAlmanac almanac = ReadInputDay5(input);
            var locations = IslandIslandAlmanac.Convert(almanac.SeedRanges, almanac.SeedToSoil);
            locations = IslandIslandAlmanac.Convert(locations, almanac.SoilToFertilizer);
            locations = IslandIslandAlmanac.Convert(locations, almanac.FertilizerToWater);
            locations = IslandIslandAlmanac.Convert(locations, almanac.WaterToLight);
            locations = IslandIslandAlmanac.Convert(locations, almanac.LightToTemperature);
            locations = IslandIslandAlmanac.Convert(locations, almanac.TemperatureToHumidity);
            locations = IslandIslandAlmanac.Convert(locations, almanac.HumidityToLocation);
            outputLabel.Content = locations.MinBy(r => r.start).start;
        }
        #endregion

        #region Day 6
        private static BoatRace[] ReadInputDay6(string input)
        {
            return BoatRace.ReadRaces(input);
        }


        [Puzzle(day: 6, part: 1)]
        public static void Day6Part1(string input, Grid display, Label outputLabel)
        {
            BoatRace[] races = ReadInputDay6(input);
            int product = 1;
            foreach (var race in races)
            {
                int options = 0;
                for (int t = 1; t < race.Time; t++)
                {
                    if (t * (race.Time - t) > race.Distance) options++;
                    else if (options > 0) break;
                }
                product *= options;
            }
            outputLabel.Content = product;
        }

        private static BoatRace ReadInputDay6ButRight(string input)
        {
            return BoatRace.ReadRace(input);
        }


        [Puzzle(day: 6, part: 2)]
        public static void Day6Part2(string input, Grid display, Label outputLabel)
        {
            BoatRace race = ReadInputDay6ButRight(input);
            ConcurrentBag<int> options = new();
            int calculated = 0;
            ProgressBar progress = new() { Minimum = 0, Maximum = race.Time - 1, MaxWidth = 200, MaxHeight = 40 };
            display.Children.Add(progress);
            var dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal, progress.Dispatcher);
            dispatcherTimer.Tick += (sender, e) =>
            {
                progress.Value = calculated;
            };
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(10);
            dispatcherTimer.Start();
            var task = Task.Run(() =>
            {
                Parallel.ForEach(Enumerable.Range(1, race.Time), t =>
                {
                    if ((long)t * (race.Time - t) > race.Distance) options.Add(t);
                    Interlocked.Increment(ref calculated);
                });
            }).ContinueWith(task => 
            { 
                outputLabel.Dispatcher.Invoke(() => outputLabel.Content = options.Count);
                dispatcherTimer.Stop();
                progress.Dispatcher.Invoke(() => progress.Value = calculated);
            });
        }
        #endregion

        #region Day 7
        private static CamelCardsHand[] ReadInputDay7(string input)
        {
            return input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).Select(l => new CamelCardsHand(l)).ToArray();
        }


        [Puzzle(day: 7, part: 1)]
        public static void Day7Part1(string input, Grid display, Label outputLabel)
        {
            CamelCardsHand[] hands = ReadInputDay7(input);
            hands = hands.OrderBy(hand => hand.Type).ThenBy(hand => hand.Cards[0].Value).ThenBy(hand => hand.Cards[1].Value).ThenBy(hand => hand.Cards[2].Value).ThenBy(hand => hand.Cards[3].Value).ThenBy(hand => hand.Cards[4].Value).ToArray();
            for (int i = 0; i < hands.Length; i++)
            {
                hands[i].Rank = i + 1;
            }
            int winnings = hands.Sum(hand => hand.Rank * hand.Bid);
            outputLabel.Content = winnings;
        }


        [Puzzle(day: 7, part: 2)]
        public static void Day7Part2(string input, Grid display, Label outputLabel)
        {
            CamelCardsHand[] hands = ReadInputDay7(input);
            hands = hands.OrderBy(hand => hand.Type2).ThenBy(hand => hand.Cards[0].Value2).ThenBy(hand => hand.Cards[1].Value2).ThenBy(hand => hand.Cards[2].Value2).ThenBy(hand => hand.Cards[3].Value2).ThenBy(hand => hand.Cards[4].Value2).ToArray();
            for (int i = 0; i < hands.Length; i++)
            {
                hands[i].Rank = i + 1;
            }
            int winnings = hands.Sum(hand => hand.Rank * hand.Bid);
            outputLabel.Content = winnings;
        }
        #endregion
    }
}
