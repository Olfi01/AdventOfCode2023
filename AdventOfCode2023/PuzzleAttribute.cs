using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PuzzleAttribute : Attribute
    {
        public int Day { get; }
        public int Part { get; }
        public PuzzleAttribute(int day, int part)
        {
            Day = day;
            Part = part;
        }
    }
}
