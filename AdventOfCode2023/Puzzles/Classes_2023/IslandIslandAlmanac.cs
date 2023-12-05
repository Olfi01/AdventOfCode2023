using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Puzzles.Classes_2023
{
    public record IslandIslandAlmanac
    {
        public long[] Seeds { get; }
        public (long start, long length)[] SeedRanges { get; }
        public (long destRangeStart, long sourceRangeStart, long rangeLength)[] SeedToSoil { get; }
        public (long destRangeStart, long sourceRangeStart, long rangeLength)[] SoilToFertilizer { get; }
        public (long destRangeStart, long sourceRangeStart, long rangeLength)[] FertilizerToWater { get; }
        public (long destRangeStart, long sourceRangeStart, long rangeLength)[] WaterToLight { get; }
        public (long destRangeStart, long sourceRangeStart, long rangeLength)[] LightToTemperature { get; }
        public (long destRangeStart, long sourceRangeStart, long rangeLength)[] TemperatureToHumidity { get; }
        public (long destRangeStart, long sourceRangeStart, long rangeLength)[] HumidityToLocation { get; }

        public IslandIslandAlmanac(string input)
        {
            string[] blocks = input.Split("\n\n");
            Seeds = blocks[0][7..].Split(' ').Select(s => long.Parse(s)).ToArray();
            SeedRanges = ReadSeedRanges(blocks[0][7..]);

            SeedToSoil = blocks[1].Split('\n').Skip(1).Select(l => ReadMapLine(l)).OrderBy(me => me.sourceRangeStart).ToArray();
            SoilToFertilizer = blocks[2].Split('\n').Skip(1).Select(l => ReadMapLine(l)).OrderBy(me => me.sourceRangeStart).ToArray();
            FertilizerToWater = blocks[3].Split('\n').Skip(1).Select(l => ReadMapLine(l)).OrderBy(me => me.sourceRangeStart).ToArray();
            WaterToLight = blocks[4].Split('\n').Skip(1).Select(l => ReadMapLine(l)).OrderBy(me => me.sourceRangeStart).ToArray();
            LightToTemperature = blocks[5].Split('\n').Skip(1).Select(l => ReadMapLine(l)).OrderBy(me => me.sourceRangeStart).ToArray();
            TemperatureToHumidity = blocks[6].Split('\n').Skip(1).Select(l => ReadMapLine(l)).OrderBy(me => me.sourceRangeStart).ToArray();
            HumidityToLocation = blocks[7].Split('\n').Where(s => !string.IsNullOrEmpty(s)).Skip(1).Select(l => ReadMapLine(l)).OrderBy(me => me.sourceRangeStart).ToArray();
        }

        private static (long start, long length)[] ReadSeedRanges(string input)
        {
            string[] split = input.Split(' ');
            (long start, long length)[] result = new (long start, long length)[split.Length / 2];
            for (int i = 0; i < split.Length; i += 2)
            {
                result[i / 2] = (long.Parse(split[i]), long.Parse(split[i + 1]));
            }
            return result;
        }

        private (long destRangeStart, long sourceRangeStart, long rangeLength) ReadMapLine(string line)
        {
            string[] split = line.Split(' ');
            return (long.Parse(split[0]), long.Parse(split[1]), long.Parse(split[2]));
        }

        public static long Convert(long input, (long destRangeStart, long sourceRangeStart, long rangeLength)[] map)
        {
            foreach (var (destRangeStart, sourceRangeStart, rangeLength) in map)
            {
                if (input >= sourceRangeStart && input < sourceRangeStart + rangeLength)
                {
                    return input + (destRangeStart - sourceRangeStart);
                }
            }
            return input;
        }

        public static long[] Convert(long[] numbers, Dictionary<long, long> map)
        {
            return numbers.Select(n => map.GetValueOrDefault(n, n)).ToArray();
        }

        public static (long start, long length)[] Convert((long start, long length)[] input, (long destRangeStart, long sourceRangeStart, long rangeLength)[] map)
        {
            List<(long start, long length)> output = new List<(long start, long length)>();
            foreach (var range in input)
            {
                MapRange(range, map, output);
            }
            return output.ToArray();
        }

        private static void MapRange((long start, long length) range, (long destRangeStart, long sourceRangeStart, long rangeLength)[] map, List<(long start, long length)> output)
        {
            List<(long destRangeStart, long sourceRangeStart, long rangeLength)> mapEntries = new(map);
            while (range.start < range.start + range.length)
            {
                (long destRangeStart, long sourceRangeStart, long rangeLength) entry = mapEntries.First();
                while (entry.sourceRangeStart + entry.rangeLength <= range.start)
                {
                    mapEntries.RemoveAt(0);
                    if (mapEntries.Count < 1)
                    {
                        output.Add((range.start, range.length));
                        return;
                    }
                    entry = mapEntries.First();
                }

                if (range.start >= entry.sourceRangeStart && range.start < entry.sourceRangeStart + entry.rangeLength)
                {
                    long newRangeStart = range.start + (entry.destRangeStart - entry.sourceRangeStart);
                    long newRangeLength = Math.Min(range.length, entry.rangeLength - (range.start - entry.sourceRangeStart));
                    output.Add((newRangeStart, newRangeLength));
                    range.start += newRangeLength;
                    range.length -= newRangeLength;
                }
                else if (entry.sourceRangeStart < range.start + range.length)
                {
                    long newRangeStart = range.start;
                    long newRangeLength = entry.sourceRangeStart - range.start;
                    output.Add((newRangeStart, newRangeLength));
                    range.start += newRangeLength;
                    range.length -= newRangeLength;
                }
                else
                {
                    output.Add((range.start, range.length));
                    return;
                }
            }
        }
    }
}
