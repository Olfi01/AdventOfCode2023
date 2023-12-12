using AdventOfCode2023.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Puzzles.Classes_2023
{
    public record SpringRecord
    {
        public SpringCondition[] Conditions { get; }
        public int[] DamagedSpringGroups { get; }
        public SpringRecord(string line)
        {
            string[] split = line.Split(' ');
            Conditions = split[0].Select(c =>
            {
                return c switch
                {
                    '.' => SpringCondition.Operational,
                    '#' => SpringCondition.Damaged,
                    '?' => SpringCondition.Unknown,
                    _ => throw new IndexOutOfRangeException()
                };
            }).ToArray();
            DamagedSpringGroups = split[1].Split(',').Select(i => int.Parse(i)).ToArray();
        }

        public static string UnfoldLine(string line)
        {
            string[] split = line.Split(' ');
            return string.Join("?", split[0], split[0], split[0], split[0], split[0]) + " " + string.Join(",", split[1], split[1], split[1], split[1], split[1]);
        }

        public static string OptimizeLine(string line)
        {
            while (line.Contains("..")) line = line.Replace("..", ".");
            line = "." + line.Replace(" ", ". ");
            return line;
        }

        public override string ToString()
        {
            return string.Join("", Conditions.Select(c => c.AsChar())) + " " + string.Join(",", DamagedSpringGroups);
        }

        internal class IntArrayEqualityComparer : IEqualityComparer<int[]>
        {
            public bool Equals(int[]? x, int[]? y)
            {
                if (x == null) return y == null;
                if (y == null) return false;
                if (x.Length != y.Length)
                {
                    return false;
                }
                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i] != y[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(int[] obj)
            {
                int result = 17;
                for (int i = 0; i < obj.Length; i++)
                {
                    unchecked
                    {
                        result = result * 23 + obj[i];
                    }
                }
                return result;
            }
        }
    }
    
    public enum SpringCondition
    {
        Operational,
        Damaged,
        Unknown
    }
}
