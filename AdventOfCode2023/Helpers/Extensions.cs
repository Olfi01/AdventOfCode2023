using AdventOfCode2023.Puzzles.Classes_2023;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AdventOfCode2023.Helpers
{
    public static class Extensions
    {
        public static T MostCommon<T> (this IEnumerable<T> ienum, T? tiebreaker = default)
        {
            var ordered = ienum.GroupBy(c => c).OrderByDescending(grp => grp.Count());
            if (ordered.Count() > 1 && ordered.ElementAt(0).Count() == ordered.ElementAt(1).Count() && tiebreaker != null) return tiebreaker;
            return ordered.Select(grp => grp.Key).First();
        }

        public static T LeastCommon<T>(this IEnumerable<T> ienum, T? tiebreaker = default)
        {
            var ordered = ienum.GroupBy(c => c).OrderBy(grp => grp.Count());
            if (ordered.Count() > 1 && ordered.ElementAt(0).Count() == ordered.ElementAt(1).Count() && tiebreaker != null) return tiebreaker;
            return ordered.Select(grp => grp.Key).First();
        }

        public static Brush GetColor(this PipeMap.TileType tile)
        {
            return tile switch
            {
                PipeMap.TileType.Undecided => Brushes.Green,
                PipeMap.TileType.Pipe => Brushes.Black,
                PipeMap.TileType.Enclosed => Brushes.Green,
                PipeMap.TileType.Outer => Brushes.Yellow,
                _ => throw new NotImplementedException(),
            };
        }

        public static string AsChar(this SpringCondition condition)
        {
            return condition switch
            {
                SpringCondition.Unknown => "?",
                SpringCondition.Operational => ".",
                SpringCondition.Damaged => "#",
                _ => "!"
            };
        }

        public static List<T[]> GetRows<T>(this T[,] array)
        {
            return Enumerable.Range(0, array.GetLength(0)).Select(row => Enumerable.Range(0, array.GetLength(1)).Select(x => array[row, x]).ToArray()).ToList();
        }

        public static List<T[]> GetColumns<T>(this T[,] array)
        {
            return Enumerable.Range(0, array.GetLength(1)).Select(col => Enumerable.Range(0, array.GetLength(0)).Select(x => array[x, col]).ToArray()).ToList();
        }

        public static bool SequenceEqualExceptOne<T>(this IEnumerable<T> enumerable, IEnumerable<T> other)
        {
            if (enumerable.Count() != other.Count()) return false;
            bool oneWasDifferent = false;
            for (int i = 0; i < enumerable.Count(); i++)
            {
                if (!enumerable.ElementAt(i)!.Equals(other.ElementAt(i)))
                {
                    if (oneWasDifferent) return false;
                    oneWasDifferent = true;
                }
            }
            return oneWasDifferent;
        }
    }
}
