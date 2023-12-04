using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
