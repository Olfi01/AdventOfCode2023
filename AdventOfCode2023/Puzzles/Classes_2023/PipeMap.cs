using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Puzzles.Classes_2023
{
    public record PipeMap
    {
        public Pipe[,] Map { get; }
        public (int row, int col) StartingPos { get; }

        public PipeMap(string input)
        {
            string[] rows = input.Split('\n').Where(s => !string.IsNullOrEmpty(s)).ToArray();
            Map = new Pipe[rows.Length, rows[0].Length];
            for (int row = 0; row < rows.Length; row++)
            {
                for (int col = 0; col < rows[row].Length; col++)
                {
                    if (rows[row][col] == 'S')
                    {
                        StartingPos = (row, col);
                    }
                    Map[row, col] = new Pipe(rows[row][col]);
                }
            }
        }

        public int FindLoopLength()
        {
            if (EastOf(StartingPos)?.ConnectsWest ?? false)
            {
                if (FindLoop((StartingPos.row, StartingPos.col + 1), StartingPos, Direction.West, out int length))
                {
                    return length;
                }
            }
            if (WestOf(StartingPos)?.ConnectsEast ?? false)
            {
                if (FindLoop((StartingPos.row, StartingPos.col - 1), StartingPos, Direction.East, out int length))
                {
                    return length;
                }
            }
            if (NorthOf(StartingPos)?.ConnectsSouth ?? false)
            {
                if (FindLoop((StartingPos.row - 1, StartingPos.col), StartingPos, Direction.South, out int length))
                {
                    return length;
                }
            }
            if (SouthOf(StartingPos)?.ConnectsNorth ?? false)
            {
                if (FindLoop((StartingPos.row + 1, StartingPos.col), StartingPos, Direction.North, out int length))
                {
                    return length;
                }
            }
            throw new Exception("PANIC");
        }

        public List<(int row, int col)> FindLoop()
        {
            if (EastOf(StartingPos)?.ConnectsWest ?? false)
            {
                List<(int row, int col)> path = new();
                if (FindLoop((StartingPos.row, StartingPos.col + 1), StartingPos, Direction.West, out int length, path))
                {
                    return path;
                }
            }
            if (WestOf(StartingPos)?.ConnectsEast ?? false)
            {
                List<(int row, int col)> path = new();
                if (FindLoop((StartingPos.row, StartingPos.col - 1), StartingPos, Direction.East, out int length, path))
                {
                    return path;
                }
            }
            if (NorthOf(StartingPos)?.ConnectsSouth ?? false)
            {
                List<(int row, int col)> path = new();
                if (FindLoop((StartingPos.row - 1, StartingPos.col), StartingPos, Direction.South, out int length, path))
                {
                    return path;
                }
            }
            if (SouthOf(StartingPos)?.ConnectsNorth ?? false)
            {
                List<(int row, int col)> path = new();
                if (FindLoop((StartingPos.row + 1, StartingPos.col), StartingPos, Direction.North, out int length, path))
                {
                    return path;
                }
            }
            throw new Exception("PANIC");
        }

        public bool FindLoop((int row, int col) current, (int row, int col) goal, Direction cameFrom, out int length, List<(int row, int col)>? path = null)
        {
            length = 1;
            path?.Add(current);
            while (current != goal)
            {
                Pipe pipe = Map[current.row, current.col];
                if (pipe.ConnectsEast && cameFrom != Direction.East)
                {
                    if (!(EastOf(current)?.ConnectsWest ?? false))
                    {
                        return false;
                    }
                    current = (current.row, current.col + 1);
                    path?.Add(current);
                    length++;
                    cameFrom = Direction.West;
                    continue;
                }
                if (pipe.ConnectsWest && cameFrom != Direction.West)
                {
                    if (!(WestOf(current)?.ConnectsEast ?? false))
                    {
                        return false;
                    }
                    current = (current.row, current.col - 1);
                    path?.Add(current);
                    length++;
                    cameFrom = Direction.East;
                    continue;
                }
                if (pipe.ConnectsNorth && cameFrom != Direction.North)
                {
                    if (!(NorthOf(current)?.ConnectsSouth ?? false))
                    {
                        return false;
                    }
                    current = (current.row - 1, current.col);
                    path?.Add(current);
                    length++;
                    cameFrom = Direction.South;
                    continue;
                }
                if (pipe.ConnectsSouth && cameFrom != Direction.South)
                {
                    if (!(SouthOf(current)?.ConnectsNorth ?? false))
                    {
                        return false;
                    }
                    current = (current.row + 1, current.col);
                    path?.Add(current);
                    length++;
                    cameFrom = Direction.North;
                    continue;
                }
            }
            return true;
        }

        private Pipe? EastOf((int row, int col) pipe)
        {
            try
            {
                return Map[pipe.row, pipe.col + 1];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }
        private Pipe? WestOf((int row, int col) pipe)
        {
            try
            {
                return Map[pipe.row, pipe.col - 1];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }
        private Pipe? NorthOf((int row, int col) pipe)
        {
            try
            {
                return Map[pipe.row - 1, pipe.col];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }
        private Pipe? SouthOf((int row, int col) pipe)
        {
            try
            {
                return Map[pipe.row + 1, pipe.col];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        public string GetPipeChar(int row, int col)
        {
            Pipe pipe = Map[row, col];
            if (pipe.ConnectsSouth && pipe.ConnectsWest && pipe.ConnectsNorth && pipe.ConnectsEast) return "S";
            else if (pipe.ConnectsNorth)
            {
                if (pipe.ConnectsSouth) return "|";
                if (pipe.ConnectsWest) return "J";
                if (pipe.ConnectsEast) return "L";
            }
            else if (pipe.ConnectsSouth)
            {
                if (pipe.ConnectsWest) return "7";
                if (pipe.ConnectsEast) return "F";
            }
            else if (pipe.ConnectsWest && pipe.ConnectsEast)
            {
                return "-";
            }
            return ".";
        }

        public record Pipe
        {
            public bool ConnectsNorth { get; }
            public bool ConnectsSouth { get; }
            public bool ConnectsWest { get; }
            public bool ConnectsEast { get; }
            public Pipe(char c)
            {
                ConnectsNorth = "|LJS".Contains(c);
                ConnectsSouth = "|7FS".Contains(c);
                ConnectsWest = "-J7S".Contains(c);
                ConnectsEast = "-LFS".Contains(c);
            }
        }

        public enum Direction
        {
            North,
            South,
            East,
            West
        }

        public enum TileType
        {
            Undecided,
            Pipe,
            Enclosed,
            Outer
        }
    }
}
