using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Puzzles.Classes_2023
{
    public class Galaxy
    {
        public long Row { get; set; }
        public long Column { get; set; }
        public Galaxy(long row, long column)
        {
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            return $"{Row},{Column}";
        }
    }
}
