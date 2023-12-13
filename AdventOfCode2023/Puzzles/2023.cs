using AdventOfCode2023.Helpers;
using AdventOfCode2023.Puzzles.Classes_2023;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using Brushes = System.Windows.Media.Brushes;

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
                    if (line.LastIndexOf(numbers[i]) > highestIdx)
                    {
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
            foreach (CubeGame game in games)
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

        #region Day 8
        private static DesertMap ReadInputDay8(string input)
        {
            return new DesertMap(input);
        }


        [Puzzle(day: 8, part: 1)]
        public static void Day8Part1(string input, Grid display, Label outputLabel)
        {
            DesertMap map = ReadInputDay8(input);
            string currentNode = "AAA";
            string goal = "ZZZ";
            int steps = 0;
            while (currentNode != goal)
            {
                for (int i = 0; i < map.Instructions.Length; i++)
                {
                    steps++;
                    switch (map.Instructions[i])
                    {
                        case 'L':
                            currentNode = map.Nodes[currentNode].Left;
                            break;
                        case 'R':
                            currentNode = map.Nodes[currentNode].Right;
                            break;
                    }
                    if (currentNode == goal) break;
                }
            }
            outputLabel.Content = steps;
        }


        [Puzzle(day: 8, part: 2)]
        public static void Day8Part2(string input, Grid display, Label outputLabel)
        {
            DesertMap map = ReadInputDay8(input);
            string[] currentNodes = map.Nodes.Where(node => node.Key.EndsWith('A')).Select(node => node.Key).ToArray();
            int[] firstGoals = new int[currentNodes.Length];
            int[] goalLoopSteps = new int[currentNodes.Length];
            for (int i = 0; i < currentNodes.Length; i++)
            {
                string currentNode = currentNodes[i];
                string startNode = currentNode;
                List<(int step, int instructionStep, string node)> path = new()
                {
                    (0, 0, currentNode)
                };
                int steps = 0;
                do
                {
                    for (int j = 0; j < map.Instructions.Length; j++)
                    {
                        steps++;
                        switch (map.Instructions[j])
                        {
                            case 'L':
                                currentNode = map.Nodes[currentNode].Left;
                                break;
                            case 'R':
                                currentNode = map.Nodes[currentNode].Right;
                                break;
                        }
                        path.Add((steps, j + 1, currentNode));
                        if (path.SkipLast(1).Any(x => x.instructionStep == path.Last().instructionStep && x.node == path.Last().node)) break;
                    }
                } while (!path.SkipLast(1).Any(x => x.instructionStep == path.Last().instructionStep && x.node == path.Last().node));
                int count = path.SkipWhile(x => x.instructionStep != path.Last().instructionStep || x.node != path.Last().node).Count(x => x.node.EndsWith('Z'));
                firstGoals[i] = path.First(x => x.node.EndsWith('Z')).step;
                goalLoopSteps[i] = path.SkipWhile(x => x.instructionStep != path.Last().instructionStep || x.node != path.Last().node).Skip(1).Count();
            }
            /* If firstGoals and goalLoopSteps weren't completely equal, I couldn't think of any other way than this
            int[] stepses = new int[currentNodes.Length];
            for (int i = 0; i < stepses.Length; i++)
            {
                stepses[i] = firstGoals[i];
            }
            
            while (stepses.GroupBy(i => i).Count() > 1)
            {
                for (int i = 0; i < stepses.Length; i++)
                {
                    if (stepses[i] == stepses.Min())
                    {
                        stepses[i] = stepses[i] + goalLoopSteps[i];
                        break;
                    }
                }
            }*/
            outputLabel.Content = Lcm(goalLoopSteps.Select(i => (long)i).ToArray()); // this only works because firstGoals is completely equal to goalLoopSteps
        }

        private static async IAsyncEnumerable<long> GetStepses(long start, long interval, [EnumeratorCancellation] CancellationToken ct)
        {
            await Task.CompletedTask;
            long num = start;
            while (!ct.IsCancellationRequested)
            {
                yield return num;
                num += interval;
            }
        }

        // *proudly* I stole it!
        private static long Gcd(long n1, long n2)
        {
            if (n2 == 0)
            {
                return n1;
            }
            else
            {
                return Gcd(n2, n1 % n2);
            }
        }

        private static long Lcm(long[] numbers)
        {
            return numbers.Aggregate((S, val) => S * val / Gcd(S, val));
        }
        #endregion

        #region Day 9
        private static List<int[]> ReadInputDay9(string input)
        {
            return input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).Select(l => l.Split(" ").Select(i => int.Parse(i)).ToArray()).ToList();
        }

        [Puzzle(day: 9, part: 1)]
        public static void Day9Part1(string input, Grid display, Label outputLabel)
        {
            List<int[]> sequences = ReadInputDay9(input);
            int sum = 0;
            foreach (int[] sequence in sequences)
            {
                Stack<int[]> derivations = new();
                int[] deriv = new int[sequence.Length - 1];
                for (int i = 0; i < sequence.Length - 1; i++)
                {
                    deriv[i] = sequence[i + 1] - sequence[i];
                }
                derivations.Push(deriv);
                while (derivations.Peek().Any(i => i != 0))
                {
                    int[] before = derivations.Peek();
                    int[] d = new int[before.Length - 1];
                    for (int i = 0; i < before.Length - 1; i++)
                    {
                        d[i] = before[i + 1] - before[i];
                    }
                    derivations.Push(d);
                }
                int last = 0;
                while (derivations.TryPop(out int[]? pop))
                {
                    last += pop!.Last();
                }
                last += sequence.Last();
                sum += last;
            }
            outputLabel.Content = sum;
        }

        [Puzzle(day: 9, part: 2)]
        public static void Day9Part2(string input, Grid display, Label outputLabel)
        {
            List<int[]> sequences = ReadInputDay9(input);
            int sum = 0;
            foreach (int[] sequence in sequences)
            {
                Stack<int[]> derivations = new();
                int[] deriv = new int[sequence.Length - 1];
                for (int i = 0; i < sequence.Length - 1; i++)
                {
                    deriv[i] = sequence[i + 1] - sequence[i];
                }
                derivations.Push(deriv);
                while (derivations.Peek().Any(i => i != 0))
                {
                    int[] before = derivations.Peek();
                    int[] d = new int[before.Length - 1];
                    for (int i = 0; i < before.Length - 1; i++)
                    {
                        d[i] = before[i + 1] - before[i];
                    }
                    derivations.Push(d);
                }
                int first = 0;
                derivations.Pop();
                while (derivations.TryPop(out int[]? pop))
                {
                    first = pop!.First() - first;
                }
                first = sequence.First() - first;
                sum += first;
            }
            outputLabel.Content = sum;
        }
        #endregion

        #region Day 10
        private static PipeMap ReadInputDay10(string input)
        {
            return new PipeMap(input);
        }


        [Puzzle(day: 10, part: 1)]
        public static void Day10Part1(string input, Grid display, Label outputLabel)
        {
            PipeMap map = ReadInputDay10(input);
            int pathLength = map.FindLoopLength();
            outputLabel.Content = pathLength / 2;
        }


        [Puzzle(day: 10, part: 2)]
        public static void Day10Part2(string input, Grid display, Label outputLabel)
        {
            PipeMap map = ReadInputDay10(input);
            List<(int row, int col)> loop = map.FindLoop();
            PipeMap.TileType[,] loopMap = CreatePipeLoopMap(map.Map, loop);
            int maxCol = loopMap.GetLength(1) - 1;
            for (int row = 0; row < loopMap.GetLength(0); row++)
            {
                if (loopMap[row, 0] != PipeMap.TileType.Pipe) loopMap[row, 0] = PipeMap.TileType.Outer;
                if (loopMap[row, maxCol] != PipeMap.TileType.Pipe) loopMap[row, maxCol] = PipeMap.TileType.Outer;
            }
            int maxRow = loopMap.GetLength(0) - 1;
            for (int col = 0; col < loopMap.GetLength(1); col++)
            {
                if (loopMap[0, col] != PipeMap.TileType.Pipe) loopMap[0, col] = PipeMap.TileType.Outer;
                if (loopMap[maxRow, col] != PipeMap.TileType.Pipe) loopMap[maxRow, col] = PipeMap.TileType.Outer;
            }
            for (int row = 0; row < loopMap.GetLength(0); row++)
            {
                if (loopMap[row, 0] == PipeMap.TileType.Outer) FloodOuters(row, 0, loopMap, maxRow, maxCol);
                if (loopMap[row, maxCol] == PipeMap.TileType.Outer) FloodOuters(row, maxCol, loopMap, maxRow, maxCol);
            }
            for (int col = 0; col < loopMap.GetLength(1); col++)
            {
                if (loopMap[0, col] == PipeMap.TileType.Outer) FloodOuters(0, col, loopMap, maxRow, maxCol);
                if (loopMap[maxRow, col] == PipeMap.TileType.Outer) FloodOuters(maxRow, col, loopMap, maxRow, maxCol);
            }
            int enclosed = 0;
            TextBlock debug = new() { FontFamily = new("Cascadia Code") };
            display.Children.Add(debug);
            for (int row = 0; row < loopMap.GetLength(0); row += 2)
            {
                for (int col = 0; col < loopMap.GetLength(1); col += 2)
                {
                    if (loopMap[row, col] == PipeMap.TileType.Undecided) enclosed++;
                    debug.Inlines.Add(new Run(map.GetPipeChar(row / 2, col / 2)) { Foreground = loopMap[row, col].GetColor() });
                }
                debug.Inlines.Add(new Run("\n"));
            }
            outputLabel.Content = enclosed;
        }

        private static void FloodOuters(int row, int col, PipeMap.TileType[,] loopMap, int maxRow, int maxCol)
        {
            for (int dR = -1; dR < 2; dR++)
            {
                for (int dC = -1; dC < 2; dC++)
                {
                    if (row + dR > maxRow || col + dC > maxCol
                        || row + dR < 0 || col + dC < 0) continue;
                    if (loopMap[row + dR, col + dC] == PipeMap.TileType.Undecided)
                    {
                        loopMap[row + dR, col + dC] = PipeMap.TileType.Outer;
                        FloodOuters(row + dR, col + dC, loopMap, maxRow, maxCol);
                    }
                }
            }
        }

        private static PipeMap.TileType[,] CreatePipeLoopMap(PipeMap.Pipe[,] map, List<(int row, int col)> loop)
        {
            PipeMap.TileType[,] outMap = new PipeMap.TileType[map.GetLength(0) * 2 - 1, map.GetLength(1) * 2 - 1];
            foreach ((int row, int col) in loop)
            {
                outMap[row * 2, col * 2] = PipeMap.TileType.Pipe;
            }
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    PipeMap.Pipe pipe = map[row, col];
                    if (pipe.ConnectsNorth && row > 0) outMap[row * 2 - 1, col * 2] = PipeMap.TileType.Pipe;
                    if (pipe.ConnectsSouth && row * 2 < outMap.GetLength(0) - 1) outMap[row * 2 + 1, col * 2] = PipeMap.TileType.Pipe;
                    if (pipe.ConnectsEast && col * 2 < outMap.GetLength(1) - 1) outMap[row * 2, col * 2 + 1] = PipeMap.TileType.Pipe;
                    if (pipe.ConnectsWest && col > 0) outMap[row * 2, col * 2 - 1] = PipeMap.TileType.Pipe;
                }
            }
            return outMap;
        }
        #endregion

        #region Day 11
        private static List<Galaxy> ReadInputDay11(string input)
        {
            List<Galaxy> list = new();
            string[] lines = input.Split('\n').Where(s => !string.IsNullOrEmpty(s)).ToArray();
            int rows = lines.Length;
            int cols = lines[0].Length;
            for (int row = 0; row < lines.Length; row++)
            {
                string line = lines[row];
                for (int col = 0; col < line.Length; col++)
                {
                    char c = line[col];
                    if (c == '#') list.Add(new(row, col));
                }
            }
            for (int row = 0; row < rows; row++)
            {
                if (!list.Any(g => g.Row == row))
                {
                    foreach (var g in list.Where(g => g.Row > row))
                    {
                        g.Row++;
                    }
                    row++;
                    rows++;
                }
            }
            for (int col = 0; col < cols; col++)
            {
                if (!list.Any(g => g.Column == col))
                {
                    foreach (var g in list.Where(g => g.Column > col))
                    {
                        g.Column++;
                    }
                    col++;
                    cols++;
                }
            }
            return list;
        }


        [Puzzle(day: 11, part: 1)]
        public static void Day11Part1(string input, Grid display, Label outputLabel)
        {
            List<Galaxy> galaxies = ReadInputDay11(input);
            long sum = 0;
            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    sum += Math.Abs(galaxies[i].Column - galaxies[j].Column) + Math.Abs(galaxies[i].Row - galaxies[j].Row);
                }
            }
            outputLabel.Content = sum;
        }

        private static List<Galaxy> ReadInputDay11Part2(string input)
        {
            List<Galaxy> list = new();
            string[] lines = input.Split('\n').Where(s => !string.IsNullOrEmpty(s)).ToArray();
            int rows = lines.Length;
            int cols = lines[0].Length;
            for (int row = 0; row < lines.Length; row++)
            {
                string line = lines[row];
                for (int col = 0; col < line.Length; col++)
                {
                    char c = line[col];
                    if (c == '#') list.Add(new(row, col));
                }
            }
            for (long row = 0; row < rows; row++)
            {
                if (!list.Any(g => g.Row == row))
                {
                    foreach (var g in list.Where(g => g.Row > row))
                    {
                        g.Row += 999999;
                    }
                    row += 999999;
                    rows += 999999;
                }
            }
            for (long col = 0; col < cols; col++)
            {
                if (!list.Any(g => g.Column == col))
                {
                    foreach (var g in list.Where(g => g.Column > col))
                    {
                        g.Column += 999999;
                    }
                    col += 999999;
                    cols += 999999;
                }
            }
            return list;
        }


        [Puzzle(day: 11, part: 2)]
        public static void Day11Part2(string input, Grid display, Label outputLabel)
        {
            List<Galaxy> galaxies = ReadInputDay11Part2(input);
            long sum = 0;
            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    sum += Math.Abs(galaxies[i].Column - galaxies[j].Column) + Math.Abs(galaxies[i].Row - galaxies[j].Row);
                }
            }
            outputLabel.Content = sum;
        }
        #endregion

        #region Day 12
        private static SpringRecord[] ReadInputDay12(string input)
        {
            return input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).Select(l => new SpringRecord(l)).ToArray();
        }


        [Puzzle(day: 12, part: 1)]
        public static void Day12Part1(string input, Grid display, Label outputLabel)
        {
            SpringRecord[] records = ReadInputDay12(input);
            int possibleArrangements = 0;
            foreach (SpringRecord record in records)
            {
                FindPossibleArrangements(record, unknowns: new List<SpringCondition>(), ref possibleArrangements);
            }
            outputLabel.Content = possibleArrangements;
        }

        private static void FindPossibleArrangements(SpringRecord record, List<SpringCondition> unknowns, ref int possibleArrangements)
        {
            if (record.Conditions.Count(c => c == SpringCondition.Unknown) == unknowns.Count)
            {
                // check if it matches other format, and if yes, increment posssibleArrangements
                int unknownsIdx = 0;
                List<int> groups = new();
                bool lastWasDamaged = false;
                foreach (SpringCondition condition in record.Conditions)
                {
                    SpringCondition c = condition;
                    if (c == SpringCondition.Unknown)
                    {
                        c = unknowns[unknownsIdx++];
                    }

                    if (c == SpringCondition.Damaged)
                    {
                        if (lastWasDamaged)
                        {
                            groups[^1]++;
                        }
                        else
                        {
                            groups.Add(1);
                            lastWasDamaged = true;
                        }
                    }
                    else if (c == SpringCondition.Operational)
                    {
                        lastWasDamaged = false;
                    }
                }
                if (groups.SequenceEqual(record.DamagedSpringGroups)) possibleArrangements++;
            }
            else
            {
                if (CouldNextBeDamaged(record, unknowns))
                {
                    unknowns.Add(SpringCondition.Damaged);
                    FindPossibleArrangements(record, unknowns, ref possibleArrangements);
                    unknowns.RemoveAt(unknowns.Count - 1);
                }
                if (CouldNextBeOperational(record, unknowns))
                {
                    unknowns.Add(SpringCondition.Operational);
                    FindPossibleArrangements(record, unknowns, ref possibleArrangements);
                    unknowns.RemoveAt(unknowns.Count - 1);
                }
            }
        }

        private static bool CouldNextBeDamaged(SpringRecord record, List<SpringCondition> unknowns)
        {
            int unknownsIdx = 0;
            List<int> groups = new();
            bool lastWasDamaged = false;
            for (int i = 0; i < record.Conditions.Length; i++)
            {
                SpringCondition c = record.Conditions[i];
                if (c == SpringCondition.Unknown)
                {
                    if (unknowns.Count > unknownsIdx)
                    {
                        c = unknowns[unknownsIdx++];
                    }
                    else
                    {
                        if (lastWasDamaged)
                        {
                            return record.DamagedSpringGroups.Length >= groups.Count
                                && groups.SkipLast(1).SequenceEqual(record.DamagedSpringGroups.Take(groups.Count - 1))
                                && groups[^1] + record.Conditions.Skip(i + 1).TakeWhile(c => c == SpringCondition.Damaged).Count() + 1 <= record.DamagedSpringGroups[groups.Count - 1]
                                && groups[^1] + record.Conditions.Skip(i + 1).TakeWhile(c => c != SpringCondition.Operational).Count() + 1 >= record.DamagedSpringGroups[groups.Count - 1];
                        }
                        else
                        {
                            return record.DamagedSpringGroups.Length > groups.Count
                                && groups.SequenceEqual(record.DamagedSpringGroups.Take(groups.Count))
                                && record.Conditions.Skip(i + 1).TakeWhile(c => c == SpringCondition.Damaged).Count() + 1 <= record.DamagedSpringGroups[groups.Count]
                                && record.Conditions.Skip(i + 1).TakeWhile(c => c != SpringCondition.Operational).Count() + 1 >= record.DamagedSpringGroups[groups.Count];
                        }
                    }
                }

                if (c == SpringCondition.Damaged)
                {
                    if (lastWasDamaged)
                    {
                        groups[^1]++;
                    }
                    else
                    {
                        groups.Add(1);
                        lastWasDamaged = true;
                    }
                }
                else if (c == SpringCondition.Operational)
                {
                    lastWasDamaged = false;
                }
            }
            throw new IndexOutOfRangeException();
        }

        private static bool CouldNextBeOperational(SpringRecord record, List<SpringCondition> unknowns)
        {
            int unknownsIdx = 0;
            List<int> groups = new();
            bool lastWasDamaged = false;
            for (int i = 0; i < record.Conditions.Length; i++)
            {
                SpringCondition c = record.Conditions[i];
                if (c == SpringCondition.Unknown)
                {
                    if (unknowns.Count > unknownsIdx)
                    {
                        c = unknowns[unknownsIdx++];
                    }
                    else
                    {
                        return groups.SequenceEqual(record.DamagedSpringGroups.Take(groups.Count))
                            && record.Conditions.Skip(i).Count(c => c != SpringCondition.Operational) >= record.DamagedSpringGroups.Skip(groups.Count).Sum();
                    }
                }

                if (c == SpringCondition.Damaged)
                {
                    if (lastWasDamaged)
                    {
                        groups[^1]++;
                    }
                    else
                    {
                        groups.Add(1);
                        lastWasDamaged = true;
                    }
                }
                else if (c == SpringCondition.Operational)
                {
                    lastWasDamaged = false;
                }
            }
            throw new IndexOutOfRangeException();
        }

        private static (string springs, int[] damaged)[] ReadInputDay12Part2(string input)
        {
            return input.Split("\n").Where(x => !string.IsNullOrEmpty(x))
                .Select(l => SpringRecord.UnfoldLine(l))
                .Select(l => SpringRecord.OptimizeLine(l))
                .Select(l =>
            {
                string[] split = l.Split(" ");
                return (split[0], split[1].Split(',').Select(i => int.Parse(i)).ToArray());
            }).ToArray();
        }

        [Puzzle(day: 12, part: 2)]
        public static void Day12Part2(string input, Grid display, Label outputLabel)
        {
            (string springs, int[] damaged)[] records = ReadInputDay12Part2(input);
            long possibleArrangements = 0;
            var cache = new Dictionary<string, Dictionary<int[], long>>();
            foreach (var (springs, damaged) in records)
            {
                long count = FindPossibleArrangements(springs, damaged, cache);
                possibleArrangements += count;
            }
            outputLabel.Content = possibleArrangements;
        }

        private static long FindPossibleArrangements(string springs, int[] damaged, Dictionary<string, Dictionary<int[], long>> cache)
        {
            long count = 0;
            if (damaged.Length == 0) return springs.Contains('#') ? 0 : 1;
            if (!springs.Contains('#') && !springs.Contains('?')) return 0;

            int maxIndex = Math.Min(springs.Contains('#') ? springs.IndexOf('#') : springs.Length - 1, springs.Length - damaged.Sum() - 1);

            for (int i = 1; i <= maxIndex; i++)
            {
                string part = springs[i..];
                if (part[..damaged[0]].Contains('.'))
                    continue;

                string newSprings = part[damaged[0]..];
                int[] newDamaged = damaged.Skip(1).ToArray();
                if (!cache.ContainsKey(newSprings)) cache.Add(newSprings, new Dictionary<int[], long>(new SpringRecord.IntArrayEqualityComparer()));
                if (!cache[newSprings].ContainsKey(newDamaged))
                {
                    cache[newSprings].Add(newDamaged, FindPossibleArrangements(newSprings, newDamaged, cache));
                }
                count += cache[newSprings][newDamaged];
            }

            return count;
        }
        #endregion

        #region Day 13
        private static List<char[,]> ReadInputDay13(string input)
        {
            string[] split = input.Split("\n\n");
            List<char[,]> output = new();
            foreach (string s in split)
            {
                string[] lines = s.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray();
                char[,] pattern = new char[lines.Length, lines[0].Length];
                for (int row = 0; row < lines.Length; row++)
                {
                    string line = lines[row];
                    for (int col = 0; col < line.Length; col++)
                    {
                        pattern[row, col] = line[col];
                    }
                }
                output.Add(pattern);
            }
            return output;
        }

        [Puzzle(day: 13, part: 1)]
        public static void Day13Part1(string input, Grid display, Label outputLabel)
        {
            List<char[,]> patterns = ReadInputDay13(input);
            int sum = 0;
            foreach (char[,] pattern in patterns)
            {
                sum += FindVerticalReflectionScore(pattern);
                sum += FindHorizontalReflectionScore(pattern);
            }
            outputLabel.Content = sum;
        }

        private static int FindVerticalReflectionScore(char[,] pattern)
        {
            List<char[]> columns = pattern.GetColumns();
            for (int col = 0; col < columns.Count - 1; col++)
            {
                bool mirror = true;
                for (int i = 0; i < Math.Min(col + 1, columns.Count - col - 1); i++)
                {
                    char[] leftCol = columns[col - i];
                    char[] rightCol = columns[col + i + 1];
                    if (!leftCol.SequenceEqual(rightCol))
                    {
                        mirror = false;
                        break;
                    }
                }
                if (mirror) return col + 1;
            }
            return 0;
        }

        private static int FindHorizontalReflectionScore(char[,] pattern)
        {
            List<char[]> rows = pattern.GetRows();
            for (int row = 0; row < rows.Count - 1; row++)
            {
                bool mirror = true;
                for (int i = 0; i < Math.Min(row + 1, rows.Count - row - 1); i++)
                {
                    char[] leftCol = rows[row - i];
                    char[] rightCol = rows[row + i + 1];
                    if (!leftCol.SequenceEqual(rightCol))
                    {
                        mirror = false;
                        break;
                    }
                }
                if (mirror) return (row + 1) * 100;
            }
            return 0;
        }

        [Puzzle(day: 13, part: 2)]
        public static void Day13Part2(string input, Grid display, Label outputLabel)
        {
            List<char[,]> patterns = ReadInputDay13(input);
            int sum = 0;
            foreach (char[,] pattern in patterns)
            {
                sum += FindVerticalReflectionScoreWithSmudge(pattern);
                sum += FindHorizontalReflectionScoreWithSmudge(pattern);
            }
            outputLabel.Content = sum;
        }

        private static int FindVerticalReflectionScoreWithSmudge(char[,] pattern)
        {
            List<char[]> columns = pattern.GetColumns();
            for (int col = 0; col < columns.Count - 1; col++)
            {
                bool mirror = true;
                bool smudgeFixed = false;
                for (int i = 0; i < Math.Min(col + 1, columns.Count - col - 1); i++)
                {
                    char[] leftCol = columns[col - i];
                    char[] rightCol = columns[col + i + 1];
                    if (!leftCol.SequenceEqual(rightCol))
                    {
                        if (!smudgeFixed && leftCol.SequenceEqualExceptOne(rightCol))
                        {
                            smudgeFixed = true;
                        }
                        else
                        {
                            mirror = false;
                            break;
                        }
                    }
                }
                if (mirror && smudgeFixed) return col + 1;
            }
            return 0;
        }

        private static int FindHorizontalReflectionScoreWithSmudge(char[,] pattern)
        {
            List<char[]> rows = pattern.GetRows();
            for (int row = 0; row < rows.Count - 1; row++)
            {
                bool mirror = true;
                bool smudgeFixed = false;
                for (int i = 0; i < Math.Min(row + 1, rows.Count - row - 1); i++)
                {
                    char[] leftCol = rows[row - i];
                    char[] rightCol = rows[row + i + 1];
                    if (!leftCol.SequenceEqual(rightCol))
                    {
                        if (!smudgeFixed && leftCol.SequenceEqualExceptOne(rightCol))
                        {
                            smudgeFixed = true;
                        }
                        else
                        {
                            mirror = false;
                            break;
                        }
                    }
                }
                if (mirror && smudgeFixed) return (row + 1) * 100;
            }
            return 0;
        }
        #endregion
    }
}
