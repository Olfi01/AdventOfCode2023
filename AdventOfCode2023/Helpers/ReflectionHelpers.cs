using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Helpers
{
    public class ReflectionHelpers
    {
        public static List<YearObj> GetAvailableYears()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && t.Namespace == "AdventOfCode2023.Puzzles" && t.Name.StartsWith("_") && int.TryParse(t.Name[1..], out _))
                .Select(t => new YearObj(int.Parse(t.Name[1..]), t))
                .ToList();
        }

        public class YearObj
        {
            public int Year { get; }
            public Type Type { get; }

            public YearObj(int year, Type type)
            {
                Year = year;
                Type = type;
            }
        }
    }
}
