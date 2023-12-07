using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Puzzles.Classes_2023
{
    public record CamelCardsHand
    {
        public CamelCardsCard[] Cards { get; }
        public int Bid { get; }
        public int Rank { get; set; }
        public int Type { get => GetTypeValue(); }
        public int Type2 { get => GetTypeValue2(); }

        public CamelCardsHand(string input)
        {
            string[] split = input.Split(' ');
            Bid = int.Parse(split[1]);
            Cards = split[0].Select(c => new CamelCardsCard(c)).ToArray();
        }

        private int GetTypeValue()
        {
            var grouped = Cards.GroupBy(c => c.Label).OrderByDescending(g => g.Count());
            if (grouped.First().Count() == 5) return 7;
            else if (grouped.First().Count() == 4) return 6;
            else if (grouped.First().Count() == 3)
            {
                if (grouped.Skip(1).First().Count() == 2) return 5;
                else return 4;
            }
            else if (grouped.First().Count() == 2)
            {
                if (grouped.Skip(1).First().Count() == 2) return 3;
                else return 2;
            }
            else return 1;
        }

        private int GetTypeValue2()
        {
            if (!Cards.Any(c => c.Label == 'J')) return GetTypeValue();
            var grouped = Cards.GroupBy(c => c.Label);
            var jokers = grouped.First(g => g.Key == 'J');
            grouped = grouped.Where(g => g.Key != 'J').OrderByDescending(g => g.Count());
            if (jokers.Count() == 5) return 7;
            if (grouped.First().Count() == 4) return 7;
            if (grouped.First().Count() == 3)
            {
                return jokers.Count() switch
                {
                    2 => 7,
                    1 => 6,
                    _ => throw new IndexOutOfRangeException("what")
                };
            }
            if (grouped.First().Count() == 2)
            {
                if (jokers.Count() == 3) return 7;
                if (jokers.Count() == 2) return 6;
                if (jokers.Count() == 1)
                {
                    if (grouped.Skip(1).First().Count() == 2) return 5;
                    else return 4;
                }
            }
            if (grouped.First().Count() == 1)
            {
                if (jokers.Count() == 4) return 7;
                if (jokers.Count() == 3) return 6;
                if (jokers.Count() == 2) return 4;
                if (jokers.Count() == 1) return 2;
            }
            throw new IndexOutOfRangeException("wat");
        }
    }

    public record CamelCardsCard
    {
        private static readonly Dictionary<char, int> LabelValueMap = new()
        {
            { '2', 1 },
            { '3', 2 },
            { '4', 3 },
            { '5', 4 },
            { '6', 5 },
            { '7', 6 },
            { '8', 7 },
            { '9', 8 },
            { 'T', 9 },
            { 'J', 10 },
            { 'Q', 11 },
            { 'K', 12 },
            { 'A', 13 }
        };
        private static readonly Dictionary<char, int> LabelValueMapPart2 = new()
        {
            { '2', 1 },
            { '3', 2 },
            { '4', 3 },
            { '5', 4 },
            { '6', 5 },
            { '7', 6 },
            { '8', 7 },
            { '9', 8 },
            { 'T', 9 },
            { 'J', 0 },
            { 'Q', 11 },
            { 'K', 12 },
            { 'A', 13 }
        };
        public char Label { get; }
        public int Value { get => LabelValueMap.GetValueOrDefault(Label, 0); }
        public int Value2 { get => LabelValueMapPart2.GetValueOrDefault(Label, 0); }
        public CamelCardsCard(char label)
        {
            Label = label;
        }
    }
}
