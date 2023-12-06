using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Puzzles.Classes_2023
{
    public record BoatRace
    {
        public int Time { get; }
        public long Distance { get; }
        public BoatRace(int time, long distance)
        {
            this.Time = time;
            this.Distance = distance;
        }

        public static BoatRace[] ReadRaces(string input)
        {
            string[] split = input.Split('\n');
            string[] line0 = split[0].Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            string[] line1 = split[1].Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            BoatRace[] result = new BoatRace[line0.Length - 1];
            for (int i = 1; i < line0.Length; i++)
            {
                result[i - 1] = new BoatRace(int.Parse(line0[i]), int.Parse(line1[i]));
            }
            return result;
        }

        public static BoatRace ReadRace(string input)
        {
            string[] split = input.Split('\n');
            int time = int.Parse(string.Concat(split[0].Where(c => char.IsDigit(c))));
            long distance = long.Parse(string.Concat(split[1].Where(c => char.IsDigit(c))));
            return new BoatRace(time, distance);
        }
    }
}
