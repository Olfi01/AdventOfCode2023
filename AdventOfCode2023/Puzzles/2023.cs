using AdventOfCode2023.Helpers;
using AdventOfCode2023.Puzzles.Classes_2023;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
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

        #region Day 14
        private static char[,] ReadInputDay14(string input)
        {
            string[] lines = input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray();
            char[,] grid = new char[lines.Length, lines[0].Length];
            for (int row = 0; row < lines.Length; row++)
            {
                string line = lines[row];
                for (int col = 0; col < line.Length; col++)
                {
                    grid[row, col] = line[col];
                }
            }
            return grid;
        }


        [Puzzle(day: 14, part: 1)]
        public static void Day14Part1(string input, Grid display, Label outputLabel)
        {
            char[,] grid = ReadInputDay14(input);
            RollNorth(grid);
            TextBlock textBlock = new() { FontFamily = new System.Windows.Media.FontFamily("Cascadia Code") };
            display.Children.Add(textBlock);
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    textBlock.Inlines.Add(grid[row, col].ToString());
                }
                textBlock.Inlines.Add("\n");
            }

            outputLabel.Content = CalculateLoad(grid);
        }

        private static void RollNorth(char[,] grid)
        {
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col] == 'O')
                    {
                        int newRow = row;
                        while (newRow > 0 && grid[newRow - 1, col] == '.')
                        {
                            newRow--;
                        }
                        grid[row, col] = '.';
                        grid[newRow, col] = 'O';
                    }
                }
            }
        }

        private static void RollWest(char[,] grid)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                for (int row = 0; row < grid.GetLength(0); row++)
                {
                    if (grid[row, col] == 'O')
                    {
                        int newCol = col;
                        while (newCol > 0 && grid[row, newCol - 1] == '.')
                        {
                            newCol--;
                        }
                        grid[row, col] = '.';
                        grid[row, newCol] = 'O';
                    }
                }
            }
        }

        private static void RollSouth(char[,] grid)
        {
            for (int row = grid.GetLength(0) - 1; row >= 0; row--)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col] == 'O')
                    {
                        int newRow = row;
                        while (newRow < grid.GetLength(0) - 1 && grid[newRow + 1, col] == '.')
                        {
                            newRow++;
                        }
                        grid[row, col] = '.';
                        grid[newRow, col] = 'O';
                    }
                }
            }
        }

        private static void RollEast(char[,] grid)
        {
            for (int col = grid.GetLength(1) - 1; col >= 0; col--)
            {
                for (int row = 0; row < grid.GetLength(0); row++)
                {
                    if (grid[row, col] == 'O')
                    {
                        int newCol = col;
                        while (newCol < grid.GetLength(1) - 1 && grid[row, newCol + 1] == '.')
                        {
                            newCol++;
                        }
                        grid[row, col] = '.';
                        grid[row, newCol] = 'O';
                    }
                }
            }
        }

        private static int CalculateLoad(char[,] grid)
        {
            int load = 0;
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col] == 'O')
                    {
                        load += grid.GetLength(0) - row;
                    }
                }
            }
            return load;
        }


        [Puzzle(day: 14, part: 2)]
        public static void Day14Part2(string input, Grid display, Label outputLabel)
        {
            char[,] grid = ReadInputDay14(input);
            TextBlock textBlock = new() { FontFamily = new System.Windows.Media.FontFamily("Cascadia Code") };
            display.Children.Add(textBlock);
            Dictionary<char[,], int> grids = new() { { grid.Copy(), 0 } };
            bool checkForCycle = true;
            for (int i = 0; i < 1000000000; i++)
            {
                SpinCycle(grid);
                if (grids.Keys.Any(g => g.Cast<char>().SequenceEqual(grid.Cast<char>())))
                {
                    for (int j = 0; j < grids.Keys.Count; j++)
                    {
                        if (checkForCycle && grids.Keys.ElementAt(j).Cast<char>().SequenceEqual(grid.Cast<char>()))
                        {
                            int loopIndex = grids[grids.Keys.ElementAt(j)];
                            // after i+1 spin cycles we are at grid, after loopIndex spin cycles we were at the same
                            int loopLength = (i + 1) - loopIndex;
                            while (i + loopLength < 1000000000)
                            {
                                i += loopLength;
                            }
                            checkForCycle = false;
                        }
                    }
                }
                if (checkForCycle) grids.Add(grid.Copy(), i + 1);
            }
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    textBlock.Inlines.Add(grid[row, col].ToString());
                }
                textBlock.Inlines.Add("\n");
            }
            outputLabel.Content = CalculateLoad(grid);
        }

        private static void SpinCycle(char[,] grid)
        {
            RollNorth(grid);
            RollWest(grid);
            RollSouth(grid);
            RollEast(grid);
        }


        #endregion

        #region Day 15
        private static string[] ReadInputDay15(string input)
        {
            return input.Split(",").Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }


        [Puzzle(day: 15, part: 1)]
        public static void Day15Part1(string input, Grid display, Label outputLabel)
        {
            string[] initializationSequence = ReadInputDay15(input);
            outputLabel.Content = initializationSequence.Select(s => HASH(s)).Sum();
        }

        public static int HASH(string str)
        {
            int hash = 0;
            foreach (char c in str)
            {
                if (c == '\n') continue;
                int ascii = (int)c;
                hash += ascii;
                hash *= 17;
                hash %= 256;
            }
            return hash;
        }


        [Puzzle(day: 15, part: 2)]
        public static void Day15Part2(string input, Grid display, Label outputLabel)
        {
            string[] initializationSequence = ReadInputDay15(input);
            List<(string label, int focalLength)>[] boxes = new List<(string label, int focalLength)>[256];
            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i] = new();
            }
            foreach (string step in initializationSequence)
            {
                if (step.Contains('-'))
                {
                    string label = step[..^1];
                    int boxNumber = HASH(label);
                    var box = boxes[boxNumber];
                    int lensIndex = box.FindIndex(x => x.label == label);
                    if (lensIndex >= 0)
                    {
                        box.RemoveAt(lensIndex);
                    }
                }
                else if (step.Contains('='))
                {
                    string[] split = step.Split('=');
                    string label = split[0];
                    int focalLength = int.Parse(split[1]);
                    int boxNumber = HASH(label);
                    var box = boxes[boxNumber];
                    int lensIndex = box.FindIndex(x => x.label == label);
                    if (lensIndex >= 0)
                    {
                        box.RemoveAt(lensIndex);
                        box.Insert(lensIndex, (label, focalLength));
                    }
                    else
                    {
                        box.Add((label, focalLength));
                    }
                }
            }
            outputLabel.Content = CalculateFocusingPower(boxes);
        }

        private static int CalculateFocusingPower(List<(string label, int focalLength)>[] boxes)
        {
            int sum = 0;
            for (int i = 0; i < boxes.Length; i++)
            {
                var box = boxes[i];
                sum += CalculateFocusingPower(box, i);
            }
            return sum;
        }

        private static int CalculateFocusingPower(List<(string label, int focalLength)> box, int boxNumber)
        {
            int sum = 0;
            for (int i = 0; i < box.Count; i++)
            {
                var (_, focalLength) = box[i];
                int focusingPower = (boxNumber + 1) * (i + 1) * focalLength;
                sum += focusingPower;
            }
            return sum;
        }
        #endregion

        #region Day 16
        private static char[,] ReadInputDay16(string input)
        {
            string[] lines = input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray();
            char[,] result = new char[lines.Length, lines[0].Length];
            for (int row = 0; row < lines.Length; row++)
            {
                string line = lines[row];
                for (int col = 0; col < line.Length; col++)
                {
                    result[row, col] = line[col];
                }
            }
            return result;
        }


        [Puzzle(day: 16, part: 1)]
        public static void Day16Part1(string input, Grid display, Label outputLabel)
        {
            char[,] grid = ReadInputDay16(input);
            bool[,] energized = new bool[grid.GetLength(0), grid.GetLength(1)];
            TrackBeam(0, 0, grid, energized, CardinalDirection.East, new());
            outputLabel.Content = energized.Cast<bool>().Count(b => b);
        }

        private static void TrackBeam(int row, int col, char[,] grid, bool[,] energized, CardinalDirection facing, List<(int row, int col, CardinalDirection facing)> visited)
        {
            if (row < 0 || col < 0 || row >= grid.GetLength(0) || col >= grid.GetLength(1)) return;
            if (visited.Contains((row, col, facing))) return;
            visited.Add((row, col, facing));
            energized[row, col] = true;
            switch (grid[row, col])
            {
                case '.':
                    (row, col) = MoveInDirection(facing, row, col);
                    TrackBeam(row, col, grid, energized, facing, visited);
                    return;
                case '/':
                    switch (facing)
                    {
                        case CardinalDirection.North:
                            facing = CardinalDirection.East;
                            break;
                        case CardinalDirection.East:
                            facing = CardinalDirection.North;
                            break;
                        case CardinalDirection.South:
                            facing = CardinalDirection.West;
                            break;
                        case CardinalDirection.West:
                            facing = CardinalDirection.South;
                            break;
                    }
                    (row, col) = MoveInDirection(facing, row, col);
                    TrackBeam(row, col, grid, energized, facing, visited);
                    return;
                case '\\':
                    switch (facing)
                    {
                        case CardinalDirection.North:
                            facing = CardinalDirection.West;
                            break;
                        case CardinalDirection.East:
                            facing = CardinalDirection.South;
                            break;
                        case CardinalDirection.South:
                            facing = CardinalDirection.East;
                            break;
                        case CardinalDirection.West:
                            facing = CardinalDirection.North;
                            break;
                    }
                    (row, col) = MoveInDirection(facing, row, col);
                    TrackBeam(row, col, grid, energized, facing, visited);
                    return;
                case '-':
                    switch (facing)
                    {
                        case CardinalDirection.East:
                        case CardinalDirection.West:
                            (row, col) = MoveInDirection(facing, row, col);
                            TrackBeam(row, col, grid, energized, facing, visited);
                            return;
                        case CardinalDirection.North:
                        case CardinalDirection.South:
                            (int newRow, int newCol) = MoveInDirection(CardinalDirection.West, row, col);
                            TrackBeam(newRow, newCol, grid, energized, CardinalDirection.West, visited);
                            (newRow, newCol) = MoveInDirection(CardinalDirection.East, row, col);
                            TrackBeam(newRow, newCol, grid, energized, CardinalDirection.East, visited);
                            return;
                    }
                    return;
                case '|':
                    switch (facing)
                    {
                        case CardinalDirection.North:
                        case CardinalDirection.South:
                            (row, col) = MoveInDirection(facing, row, col);
                            TrackBeam(row, col, grid, energized, facing, visited);
                            return;
                        case CardinalDirection.East:
                        case CardinalDirection.West:
                            (int newRow, int newCol) = MoveInDirection(CardinalDirection.North, row, col);
                            TrackBeam(newRow, newCol, grid, energized, CardinalDirection.North, visited);
                            (newRow, newCol) = MoveInDirection(CardinalDirection.South, row, col);
                            TrackBeam(newRow, newCol, grid, energized, CardinalDirection.South, visited);
                            return;
                    }
                    return;
            }
        }

        private static (int row, int col) MoveInDirection(CardinalDirection facing, int row, int col)
        {
            return facing switch
            {
                CardinalDirection.North => (row - 1, col),
                CardinalDirection.East => (row, col + 1),
                CardinalDirection.South => (row + 1, col),
                CardinalDirection.West => (row, col - 1),
                _ => throw new NotImplementedException()
            };
        }


        [Puzzle(day: 16, part: 2)]
        public static void Day16Part2(string input, Grid display, Label outputLabel)
        {
            char[,] grid = ReadInputDay16(input);
            int maxEnergized = 0;
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                bool[,] energized = new bool[grid.GetLength(0), grid.GetLength(1)];
                TrackBeam(row, 0, grid, energized, CardinalDirection.East, new());
                int tilesEnergized = energized.Cast<bool>().Count(b => b);
                if (tilesEnergized > maxEnergized) maxEnergized = tilesEnergized;
                energized = new bool[grid.GetLength(0), grid.GetLength(1)];
                TrackBeam(row, grid.GetLength(1) - 1, grid, energized, CardinalDirection.West, new());
                tilesEnergized = energized.Cast<bool>().Count(b => b);
                if (tilesEnergized > maxEnergized) maxEnergized = tilesEnergized;
            }
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                bool[,] energized = new bool[grid.GetLength(0), grid.GetLength(1)];
                TrackBeam(0, col, grid, energized, CardinalDirection.South, new());
                int tilesEnergized = energized.Cast<bool>().Count(b => b);
                if (tilesEnergized > maxEnergized) maxEnergized = tilesEnergized;
                energized = new bool[grid.GetLength(0), grid.GetLength(1)];
                TrackBeam(grid.GetLength(0) - 1, col, grid, energized, CardinalDirection.North, new());
                tilesEnergized = energized.Cast<bool>().Count(b => b);
                if (tilesEnergized > maxEnergized) maxEnergized = tilesEnergized;
            }
            outputLabel.Content = maxEnergized;
        }
        #endregion

        #region Day 17
        private static int[,] ReadInputDay17(string input)
        {
            string[] lines = input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray();
            int[,] result = new int[lines.Length, lines[0].Length];
            for (int row = 0; row < lines.Length; row++)
            {
                string line = lines[row];
                for (int col = 0; col < line.Length; col++)
                {
                    result[row, col] = int.Parse(line[col] + "");
                }
            }
            return result;
        }


        [Puzzle(day: 17, part: 1)]
        public static void Day17Part1(string input, Grid display, Label outputLabel)
        {
            int[,] heatMap = ReadInputDay17(input);
            long progressValue = 0;
            long maximumProgress = (long)heatMap.GetLength(0) * heatMap.GetLength(1) * 12;
            ProgressBar progress = new() { Minimum = 0, Maximum = maximumProgress, MaxWidth = 200, MaxHeight = 40 };
            display.Children.Add(progress);
            var dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal, progress.Dispatcher);
            dispatcherTimer.Tick += (sender, e) =>
            {
                progress.Value = progressValue;
            };
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(10);
            dispatcherTimer.Start();
            Task.Run(() =>
            {
                int result = HeatDijkstra(heatMap, (0, 0), (heatMap.GetLength(0) - 1, heatMap.GetLength(1) - 1), ref progressValue);
                progressValue = maximumProgress;
                outputLabel.Dispatcher.Invoke(() => outputLabel.Content = result);
            });
        }

        private static int HeatDijkstra(int[,] heatMap, (int row, int col) startNode, (int row, int col) goalNode, ref long progressValue)
        {
            var previous = new Dictionary<(int row, int col, int direction, int steps), (int row, int col, int direction, int steps)>();
            Dictionary<(int row, int col, int direction, int steps), int> distance = new();
            var Q = new PriorityQueue<(int row, int col, int direction, int steps), int>();
            HashSet<(int row, int col, int direction, int steps)> solved = new();
            for (int row = 0; row < heatMap.GetLength(0); row++)
            {
                for (int col = 0; col < heatMap.GetLength(1); col++)
                {
                    for (int direction = 0; direction < 4; direction++)
                    {
                        for (int steps = 1; steps <= 3; steps++)
                        {
                            Q.Enqueue((row, col, direction, steps), int.MaxValue);
                            distance[(row, col, direction, steps)] = int.MaxValue;
                        }
                    }
                }
            }
            Q.Enqueue((startNode.row, startNode.col, 1, 0), 0);
            distance[(startNode.row, startNode.col, 1, 0)] = 0;
            while (Q.TryDequeue(out var currentNode, out int currentDistance))
            {
                if (solved.Contains(currentNode)) continue;
                solved.Add(currentNode);
                Interlocked.Increment(ref progressValue);
                if (currentNode.row == goalNode.row && currentNode.col == goalNode.col) return currentDistance;
                if (currentNode.steps < 3)
                {
                    var forward = StepForward(currentNode);
                    if (!solved.Contains(forward) && forward.row >= 0 && forward.row < heatMap.GetLength(0) && forward.col >= 0 && forward.col < heatMap.GetLength(1))
                    {
                        UpdateDistance(heatMap, distance, previous, currentNode, forward, Q);
                    }
                }
                var left = TurnLeft(currentNode);
                if (!solved.Contains(left) && left.row >= 0 && left.row < heatMap.GetLength(0) && left.col >= 0 && left.col < heatMap.GetLength(1))
                {
                    UpdateDistance(heatMap, distance, previous, currentNode, left, Q);
                }
                var right = TurnRight(currentNode);
                if (!solved.Contains(right) && right.row >= 0 && right.row < heatMap.GetLength(0) && right.col >= 0 && right.col < heatMap.GetLength(1))
                {
                    UpdateDistance(heatMap, distance, previous, currentNode, right, Q);
                }
            }
            throw new IndexOutOfRangeException();
        }

        private static (int row, int col, int direction, int steps) StepForward((int row, int col, int direction, int steps) currentNode)
        {
            return currentNode.direction switch
            {
                0 => (currentNode.row - 1, currentNode.col, currentNode.direction, currentNode.steps + 1),
                1 => (currentNode.row, currentNode.col + 1, currentNode.direction, currentNode.steps + 1),
                2 => (currentNode.row + 1, currentNode.col, currentNode.direction, currentNode.steps + 1),
                3 => (currentNode.row, currentNode.col - 1, currentNode.direction, currentNode.steps + 1),
                _ => throw new IndexOutOfRangeException()
            };
        }

        private static (int row, int col, int direction, int steps) TurnRight((int row, int col, int direction, int steps) currentNode)
        {
            return currentNode.direction switch
            {
                0 => (currentNode.row, currentNode.col + 1, 1, 1),
                1 => (currentNode.row + 1, currentNode.col, 2, 1),
                2 => (currentNode.row, currentNode.col - 1, 3, 1),
                3 => (currentNode.row - 1, currentNode.col, 0, 1),
                _ => throw new IndexOutOfRangeException()
            };
        }

        private static (int row, int col, int direction, int steps) TurnLeft((int row, int col, int direction, int steps) currentNode)
        {
            return currentNode.direction switch
            {
                0 => (currentNode.row, currentNode.col - 1, 3, 1),
                1 => (currentNode.row - 1, currentNode.col, 0, 1),
                2 => (currentNode.row, currentNode.col + 1, 1, 1),
                3 => (currentNode.row + 1, currentNode.col, 2, 1),
                _ => throw new IndexOutOfRangeException()
            };
        }

        private static void UpdateDistance(int[,] heatMap, Dictionary<(int row, int col, int direction, int steps), int> distance, Dictionary<(int row, int col, int direction, int steps), (int row, int col, int direction, int steps)> previous, (int row, int col, int direction, int steps) currentNode, (int row, int col, int direction, int steps) forward, PriorityQueue<(int row, int col, int direction, int steps), int> Q)
        {
            int newDistance = distance[currentNode] + heatMap[forward.row, forward.col];

            if (newDistance < distance[forward])
            {
                distance[forward] = newDistance;
                previous[forward] = currentNode;
                Q.Enqueue(forward, newDistance);
            }
        }


        [Puzzle(day: 17, part: 2)]
        public static void Day17Part2(string input, Grid display, Label outputLabel)
        {
            int[,] heatMap = ReadInputDay17(input);
            long progressValue = 0;
            long maximumProgress = (long)heatMap.GetLength(0) * heatMap.GetLength(1) * 12;
            ProgressBar progress = new() { Minimum = 0, Maximum = maximumProgress, MaxWidth = 200, MaxHeight = 40 };
            display.Children.Add(progress);
            var dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal, progress.Dispatcher);
            dispatcherTimer.Tick += (sender, e) =>
            {
                progress.Value = progressValue;
            };
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(10);
            dispatcherTimer.Start();
            Task.Run(() =>
            {
                int result = UltraHeatDijkstra(heatMap, (0, 0), (heatMap.GetLength(0) - 1, heatMap.GetLength(1) - 1), ref progressValue);
                progressValue = maximumProgress;
                outputLabel.Dispatcher.Invoke(() => outputLabel.Content = result);
            });
        }

        private static int UltraHeatDijkstra(int[,] heatMap, (int row, int col) startNode, (int row, int col) goalNode, ref long progressValue)
        {
            var previous = new Dictionary<(int row, int col, int direction), (int row, int col, int direction)>();
            Dictionary<(int row, int col, int direction), int> distance = new();
            var Q = new PriorityQueue<(int row, int col, int direction), int>();
            HashSet<(int row, int col, int direction)> solved = new();
            for (int row = 0; row < heatMap.GetLength(0); row++)
            {
                for (int col = 0; col < heatMap.GetLength(1); col++)
                {
                    for (int direction = 0; direction < 4; direction++)
                    {
                        Q.Enqueue((row, col, direction), int.MaxValue);
                        distance[(row, col, direction)] = int.MaxValue;
                    }
                }
            }
            Q.Enqueue((startNode.row, startNode.col, 1), 0);
            distance[(startNode.row, startNode.col, 1)] = 0;
            while (Q.TryDequeue(out var currentNode, out int currentDistance))
            {
                if (solved.Contains(currentNode)) continue;
                solved.Add(currentNode);
                Interlocked.Increment(ref progressValue);
                if (currentNode.row == goalNode.row && currentNode.col == goalNode.col) return currentDistance;
                var left = (currentNode != (0, 0, 1)) ? TurnLeftAndMove3(currentNode) : Move3(currentNode);
                for (int i = 0; i < 7; i++)
                {
                    left = StepForward(left);
                    if (!solved.Contains(left) && left.row >= 0 && left.row < heatMap.GetLength(0) && left.col >= 0 && left.col < heatMap.GetLength(1))
                    {
                        UpdateDistance(heatMap, distance, previous, currentNode, left, Q);
                    }
                }
                var right = TurnRightAndMove3(currentNode);
                for (int i = 0; i < 7; i++)
                {
                    right = StepForward(right);
                    if (!solved.Contains(right) && right.row >= 0 && right.row < heatMap.GetLength(0) && right.col >= 0 && right.col < heatMap.GetLength(1))
                    {
                        UpdateDistance(heatMap, distance, previous, currentNode, right, Q);
                    }
                }
            }
            throw new IndexOutOfRangeException();
        }

        private static (int row, int col, int direction) TurnLeftAndMove3((int row, int col, int direction) currentNode)
        {
            return currentNode.direction switch
            {
                0 => (currentNode.row, currentNode.col - 3, 3),
                1 => (currentNode.row - 3, currentNode.col, 0),
                2 => (currentNode.row, currentNode.col + 3, 1),
                3 => (currentNode.row + 3, currentNode.col, 2),
                _ => throw new IndexOutOfRangeException()
            };
        }

        private static (int row, int col, int direction) Move3((int row, int col, int direction) currentNode)
        {
            return currentNode.direction switch
            {
                1 => (currentNode.row, currentNode.col + 3, 1),
                _ => throw new NotImplementedException()
            };
        }

        private static (int row, int col, int direction) TurnRightAndMove3((int row, int col, int direction) currentNode)
        {
            return currentNode.direction switch
            {
                0 => (currentNode.row, currentNode.col + 3, 1),
                1 => (currentNode.row + 3, currentNode.col, 2),
                2 => (currentNode.row, currentNode.col - 3, 3),
                3 => (currentNode.row - 3, currentNode.col, 0),
                _ => throw new IndexOutOfRangeException()
            };
        }

        private static (int row, int col, int direction) StepForward((int row, int col, int direction) currentNode)
        {
            return currentNode.direction switch
            {
                0 => (currentNode.row - 1, currentNode.col, currentNode.direction),
                1 => (currentNode.row, currentNode.col + 1, currentNode.direction),
                2 => (currentNode.row + 1, currentNode.col, currentNode.direction),
                3 => (currentNode.row, currentNode.col - 1, currentNode.direction),
                _ => throw new IndexOutOfRangeException()
            };
        }

        private static void UpdateDistance(int[,] heatMap, Dictionary<(int row, int col, int direction), int> distance, Dictionary<(int row, int col, int direction), (int row, int col, int direction)> previous, (int row, int col, int direction) currentNode, (int row, int col, int direction) next, PriorityQueue<(int row, int col, int direction), int> Q)
        {
            int newDistance = distance[currentNode] + heatMap[next.row, next.col];
            if (currentNode.row < next.row)
            {
                for (int row = currentNode.row + 1; row < next.row; row++)
                {
                    newDistance += heatMap[row, next.col];
                }
            }
            if (currentNode.row > next.row)
            {
                for (int row = currentNode.row - 1; row > next.row; row--)
                {
                    newDistance += heatMap[row, next.col];
                }
            }
            if (currentNode.col < next.col)
            {
                for (int col = currentNode.col + 1; col < next.col; col++)
                {
                    newDistance += heatMap[next.row, col];
                }
            }
            if (currentNode.col > next.col)
            {
                for (int col = currentNode.col - 1; col > next.col; col--)
                {
                    newDistance += heatMap[next.row, col];
                }
            }

            if (newDistance < distance[next])
            {
                distance[next] = newDistance;
                previous[next] = currentNode;
                Q.Enqueue(next, newDistance);
            }
        }
        #endregion

        #region Day 18
        private static (char direction, int distance, string color)[] ReadInputDay18(string input)
        {
            return input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).Select(line =>
            {
                string[] split = line.Split(' ');
                char direction = split[0][0];
                int distance = int.Parse(split[1]);
                string color = split[2][1..^1];
                return (direction, distance, color);
            }).ToArray();
        }


        [Puzzle(day: 18, part: 1)]
        public static void Day18Part1(string input, Grid display, Label outputLabel)
        {
            var digPlan = ReadInputDay18(input);
            int row = 0;
            int col = 0;
            HashSet<(int row, int col)> hole = new() { (0, 0) };
            Dictionary<(int row, int col), string> colors = new();
            foreach (var (direction, distance, color) in digPlan)
            {
                int dr;
                int dc;
                switch (direction)
                {
                    case 'U':
                        dr = -1;
                        dc = 0;
                        break;
                    case 'R':
                        dr = 0;
                        dc = 1;
                        break;
                    case 'D':
                        dr = 1;
                        dc = 0;
                        break;
                    case 'L':
                        dr = 0;
                        dc = -1;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                for (int i = 0; i < distance; i++)
                {
                    row += dr;
                    col += dc;
                    hole.Add((row, col));
                    colors.Add((row, col), color);
                }
            }
            TextBlock textBlock = new() { FontFamily = new System.Windows.Media.FontFamily("Cascadia Code") };
            display.Children.Add(textBlock);
            int minRow = hole.Min(x => x.row);
            int minCol = hole.Min(x => x.col);
            int maxRow = hole.Max(x => x.row);
            int maxCol = hole.Max(x => x.col);
            HashSet<(int row, int col)> holeEdge = hole.Copy();
            for (int row1 = minRow; row1 <= maxRow; row1++)
            {
                bool inside = false;
                for (int col1 = minCol; col1 <= maxCol; col1++)
                {
                    if (hole.Contains((row1, col1)))
                    {
                        inside = !inside;
                        string color = colors.ContainsKey((row1, col1)) ? colors[(row1, col1)] : "#000000";
                        bool above = holeEdge.Contains((row1 - 1, col1));
                        bool below = holeEdge.Contains((row1 + 1, col1));
                        int length = 0;
                        do
                        {
                            textBlock.Inlines.Add(new Run("#") { Foreground = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom(color)! });
                            col1++;
                            length++;
                        } while (hole.Contains((row1, col1)));
                        if ((length > 1 && above && holeEdge.Contains((row1 - 1, col1 - 1))) ||
                            (length > 1 && below && holeEdge.Contains((row1 + 1, col1 - 1)))) inside = !inside;
                        if (col1 > maxCol || !hole.Any(x => x.row == row1 && x.col > col1)) break;
                    }
                    if (inside) hole.Add((row1, col1));
                    else textBlock.Inlines.Add(" ");
                }
                textBlock.Inlines.Add("\n");
            }
            outputLabel.Content = hole.Count;
        }

        private static (char direction, int distance, string color)[] ReadInputDay18Part2(string input)
        {
            return input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).Select(line =>
            {
                string[] split = line.Split(' ');
                string color = split[2][1..^1];
                return (DigitToDirection(color[^1..]), Convert.ToInt32(color[1..^1], 16), "#ff0000");
            }).ToArray();
        }

        private static char DigitToDirection(string v)
        {
            return v switch
            {
                "0" => 'R',
                "1" => 'D',
                "2" => 'L',
                "3" => 'U',
                _ => throw new IndexOutOfRangeException()
            };
        }

        [Puzzle(day: 18, part: 2)]
        public static void Day18Part2(string input, Grid display, Label outputLabel)
        {
            var digPlan = ReadInputDay18Part2(input);
            long row = 0;
            long col = 0;
            HashSet<(long row, long col0, long col1)> horizontalEdges = new();
            HashSet<(long row0, long row1, long col)> verticalEdges = new();
            foreach (var (direction, distance, color) in digPlan)
            {
                switch (direction)
                {
                    case 'U':
                        verticalEdges.Add((row - distance, row, col));
                        row -= distance;
                        break;
                    case 'R':
                        horizontalEdges.Add((row, col, col + distance));
                        col += distance;
                        break;
                    case 'D':
                        verticalEdges.Add((row, row + distance, col));
                        row += distance;
                        break;
                    case 'L':
                        horizontalEdges.Add((row, col - distance, col));
                        col -= distance;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            long minRow = horizontalEdges.Min(x => x.row);
            long minCol = verticalEdges.Min(x => x.col);
            long maxRow = horizontalEdges.Max(x => x.row);
            long maxCol = verticalEdges.Max(x => x.col);
            BigInteger holeSize = 0;
            BigInteger lastRowSize = 0;
            long lastRow = minRow;
            var x = verticalEdges.Where(edge => edge.row0 == 110 || edge.row1 == 109);
            foreach (long row1 in verticalEdges.SelectMany(edge => new long[] {edge.row0, edge.row0 + 1, edge.row1, edge.row1 + 1}).Distinct().OrderBy(l => l))
            {
                holeSize += lastRowSize * (row1 - lastRow);
                lastRow = row1;
                lastRowSize = CalculateRowSize(row1, verticalEdges, horizontalEdges, minCol, maxCol);
            }
            holeSize += lastRowSize * (maxRow - lastRow);

            outputLabel.Content = holeSize;
        }

        private static BigInteger CalculateRowSize(long row, HashSet<(long row0, long row1, long col)> verticalEdges, HashSet<(long row, long col0, long col1)> horizontalEdges, long minCol, long maxCol)
        {
            BigInteger rowSize = 0;
            bool inside = false;
            long lastCol = minCol;
            foreach (long col in verticalEdges.Where(edge => edge.row0 <= row && edge.row1 >= row).Select(edge => edge.col).Distinct().OrderBy(l => l))
            {
                if (horizontalEdges.Contains((row, lastCol, col)))
                {
                    rowSize += col - lastCol;
                    if (verticalEdges.Any(edge => edge.col == lastCol && edge.row0 == row) && verticalEdges.Any(edge => edge.col == col && edge.row0 == row)) inside = !inside;
                    else if (verticalEdges.Any(edge => edge.col == lastCol && edge.row1 == row) && verticalEdges.Any(edge => edge.col == col && edge.row1 == row)) inside = !inside;
                }
                else
                {
                    if (inside) rowSize += col - lastCol;
                    else rowSize++;
                    inside = !inside;
                }
                lastCol = col;
            }
            if (inside) throw new NotImplementedException();
            return rowSize;
        }
        #endregion

        #region Day 19
        private static (Dictionary<string, MachinePartWorkflow> workflows, MachinePart[] parts) ReadInputDay19(string input)
        {
            string[] split = input.Split("\n\n");
            Dictionary<string, MachinePartWorkflow> workflows = split[0].Split('\n').Select(l => new MachinePartWorkflow(l)).ToDictionary(w => w.Name);
            MachinePart[] parts = split[1].Split("\n").Where(x => !string.IsNullOrEmpty(x)).Select(l => new MachinePart(l)).ToArray();
            return (workflows, parts);
        }


        [Puzzle(day: 19, part: 1)]
        public static void Day19Part1(string input, Grid display, Label outputLabel)
        {
            var (workflows, parts) = ReadInputDay19(input);
            List<MachinePart> accepted = new();
            foreach (var part in parts)
            {
                bool accept = SendToWorkflow("in", part, workflows);
                if (accept) accepted.Add(part);
            }
            outputLabel.Content = accepted.Select(part => part.Properties.Sum(x => x.Value)).Sum();
        }

        private static bool SendToWorkflow(string workflowName, MachinePart part, Dictionary<string, MachinePartWorkflow> workflows)
        {
            if (workflowName == "A") return true;
            if (workflowName == "R") return false;
            MachinePartWorkflow workflow = workflows[workflowName];
            foreach (var rule in workflow.Rules)
            {
                if (rule.Operation.HasValue && rule.Property.HasValue && rule.Compare.HasValue)
                {
                    switch (rule.Operation.Value)
                    {
                        case MachinePartWorkflow.Operation.lt:
                            if (part.Properties[rule.Property.Value] < rule.Compare.Value) return SendToWorkflow(rule.TargetWorkflow, part, workflows);
                            break;
                        case MachinePartWorkflow.Operation.gt:
                            if (part.Properties[rule.Property.Value] > rule.Compare.Value) return SendToWorkflow(rule.TargetWorkflow, part, workflows);
                            break;
                    }
                }
                else
                {
                    return SendToWorkflow(rule.TargetWorkflow, part, workflows);
                }
            }
            throw new NotImplementedException();
        }


        [Puzzle(day: 19, part: 2)]
        public static void Day19Part2(string input, Grid display, Label outputLabel)
        {
            var (workflows, _) = ReadInputDay19(input);
            long accepted = WorkThroughWorkflows("in", conditions: Array.Empty<(MachinePart.Property property, MachinePartWorkflow.Operation operation, int compare)>(), workflows);
            outputLabel.Content = accepted;
        }

        private static long WorkThroughWorkflows(string workflowName, (MachinePart.Property property, MachinePartWorkflow.Operation operation, int compare)[] conditions, Dictionary<string, MachinePartWorkflow> workflows)
        {
            if (workflowName == "R") return 0;
            if (workflowName == "A") return CalculatePossibilities(conditions);
            MachinePartWorkflow workflow = workflows[workflowName];
            long sum = 0;
            List<(MachinePart.Property property, MachinePartWorkflow.Operation operation, int compare)> negatedConditions = new();
            foreach (var rule in workflow.Rules)
            {
                if (rule.Operation.HasValue && rule.Property.HasValue && rule.Compare.HasValue)
                {
                    var condition = (rule.Property.Value, rule.Operation.Value, rule.Compare.Value);
                    sum += WorkThroughWorkflows(rule.TargetWorkflow, conditions.Concat(negatedConditions).Append(condition).ToArray(), workflows);
                    negatedConditions.Add(NegateCondition(condition));
                }
                else
                {
                    sum += WorkThroughWorkflows(rule.TargetWorkflow, conditions.Concat(negatedConditions).ToArray(), workflows);
                }
            }
            return sum;
        }

        private static (MachinePart.Property property, MachinePartWorkflow.Operation operation, int compare) NegateCondition((MachinePart.Property property, MachinePartWorkflow.Operation operation, int compare) condition)
        {
            return condition.operation switch
            {
                MachinePartWorkflow.Operation.lt => (condition.property, MachinePartWorkflow.Operation.gt, condition.compare - 1),
                MachinePartWorkflow.Operation.gt => (condition.property, MachinePartWorkflow.Operation.lt, condition.compare + 1),
                _ => throw new NotImplementedException(),
            };
        }

        private static long CalculatePossibilities((MachinePart.Property property, MachinePartWorkflow.Operation operation, int compare)[] conditions)
        {
            Dictionary<MachinePart.Property, int> ltet = new()
            {
                { MachinePart.Property.x, 4000 },
                { MachinePart.Property.m, 4000 },
                { MachinePart.Property.a, 4000 },
                { MachinePart.Property.s, 4000 }
            };
            Dictionary<MachinePart.Property, int> gtet = new()
            {
                { MachinePart.Property.x, 1 },
                { MachinePart.Property.m, 1 },
                { MachinePart.Property.a, 1 },
                { MachinePart.Property.s, 1 }
            };
            foreach (var (property, operation, compare) in conditions)
            {
                switch (operation)
                {
                    case MachinePartWorkflow.Operation.lt:
                        if (ltet[property] > compare - 1) ltet[property] = compare - 1;
                        break;
                    case MachinePartWorkflow.Operation.gt:
                        if (gtet[property] < compare + 1) gtet[property] = compare + 1;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            long xRange = Math.Max(0, ltet[MachinePart.Property.x] - gtet[MachinePart.Property.x] + 1);
            long mRange = Math.Max(0, ltet[MachinePart.Property.m] - gtet[MachinePart.Property.m] + 1);
            long aRange = Math.Max(0, ltet[MachinePart.Property.a] - gtet[MachinePart.Property.a] + 1);
            long sRange = Math.Max(0, ltet[MachinePart.Property.s] - gtet[MachinePart.Property.s] + 1);
            return xRange * mRange * aRange * sRange;
        }
        #endregion

        #region Day 20
        private static Dictionary<string, MachineModule> ReadInputDay20(string input)
        {
            return input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).Select(l => MachineModule.From(l)).ToDictionary(m => m.Name);
        }


        [Puzzle(day: 20, part: 1)]
        public static void Day20Part1(string input, Grid display, Label outputLabel)
        {
            Dictionary<string, MachineModule> modules = ReadInputDay20(input);
            foreach (var module in modules.Values)
            {
                if (module is not ConjunctionModule conjunctionModule) continue;
                foreach (var sourceModule in modules.Values.Where(m => m.DestinationModules.Contains(conjunctionModule.Name)))
                {
                    conjunctionModule.StoredPulses.Add(sourceModule.Name, MachineModule.Pulse.Low);
                }
            }
            Queue<(MachineModule.Pulse pulse, string destination, string origin)> pulses = new();
            long lowCount = 0;
            long highCount = 0;
            for (int i = 0; i < 1000; i++)
            {
                QueuePulse(MachineModule.Pulse.Low, "broadcaster", "button", pulses, ref lowCount, ref highCount);
                ProcessAllPulses(modules, pulses, ref lowCount, ref highCount);
            }
            outputLabel.Content = lowCount * highCount;
        }

        private static void ProcessAllPulses(Dictionary<string, MachineModule> modules, Queue<(MachineModule.Pulse pulse, string destination, string origin)> pulses, ref long lowCount, ref long highCount)
        {
            while (pulses.Count > 0)
            {
                var pulse = pulses.Dequeue();
                if (!modules.ContainsKey(pulse.destination)) continue;
                var destinationModule = modules[pulse.destination];
                var outputPulse = destinationModule.ProcessPulse(pulse.pulse, pulse.origin);
                if (outputPulse.HasValue)
                {
                    foreach (string destination in destinationModule.DestinationModules)
                    {
                        QueuePulse(outputPulse.Value, destination, destinationModule.Name, pulses, ref lowCount, ref highCount);
                    }
                }
            }
        }

        private static void QueuePulse(MachineModule.Pulse pulse, string destination, string origin, Queue<(MachineModule.Pulse pulse, string destination, string origin)> pulses, ref long lowCount, ref long highCount)
        {
            if (pulse == MachineModule.Pulse.Low) lowCount++;
            else if (pulse == MachineModule.Pulse.High) highCount++;
            pulses.Enqueue((pulse, destination, origin));
        }


        [Puzzle(day: 20, part: 2)]
        public static void Day20Part2(string input, Grid display, Label outputLabel)
        {
            Dictionary<string, MachineModule> modules = ReadInputDay20(input);
            foreach (var module in modules.Values)
            {
                if (module is not ConjunctionModule conjunctionModule) continue;
                foreach (var sourceModule in modules.Values.Where(m => m.DestinationModules.Contains(conjunctionModule.Name)))
                {
                    conjunctionModule.StoredPulses.Add(sourceModule.Name, MachineModule.Pulse.Low);
                }
            }
            Queue<(MachineModule.Pulse pulse, string destination, string origin)> pulses = new();
            Dictionary<string, long> previousLoops = new();
            string finalConj = modules.Values.Where(m => m.DestinationModules.Contains("rx")).First().Name;
            foreach (var module in modules.Values.Where(m => m.DestinationModules.Contains(finalConj)))
            {
                previousLoops.Add(module.Name, -1);
            }
            long buttonPresses = 0;
            while (previousLoops.Values.Any(v => v < 0))
            {
                QueuePulse(MachineModule.Pulse.Low, "broadcaster", "button", pulses, previousLoops, buttonPresses);
                buttonPresses++;
                ProcessAllPulses(modules, pulses, previousLoops, buttonPresses);
            }
            outputLabel.Content = Lcm(previousLoops.Values.ToArray());
        }

        private static void ProcessAllPulses(Dictionary<string, MachineModule> modules, Queue<(MachineModule.Pulse pulse, string destination, string origin)> pulses, Dictionary<string, long> previousLoops, long buttonPresses)
        {
            while (pulses.Count > 0)
            {
                var pulse = pulses.Dequeue();
                if (!modules.ContainsKey(pulse.destination)) continue;
                var destinationModule = modules[pulse.destination];
                var outputPulse = destinationModule.ProcessPulse(pulse.pulse, pulse.origin);
                if (outputPulse.HasValue)
                {
                    foreach (string destination in destinationModule.DestinationModules)
                    {
                        QueuePulse(outputPulse.Value, destination, destinationModule.Name, pulses, previousLoops, buttonPresses);
                    }
                }
            }
        }

        private static void QueuePulse(MachineModule.Pulse pulse, string destination, string origin, Queue<(MachineModule.Pulse pulse, string destination, string origin)> pulses, Dictionary<string, long> previousLoops, long buttonPresses)
        {
            if (pulse == MachineModule.Pulse.High && previousLoops.ContainsKey(origin) && previousLoops[origin] < 0) previousLoops[origin] = buttonPresses;
            pulses.Enqueue((pulse, destination, origin));
        }
        #endregion
    }
}
