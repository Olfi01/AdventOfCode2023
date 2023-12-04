using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Puzzles.Classes_2023
{
    public class CubeGame
    {
        public int Id { get; }

        public List<List<(Color c, int n)>> Draws { get; } = new();

        public CubeGame(string input)
        {
            string[] split = input.Split(":");
            split[1] = split[1].Trim();
            Id = int.Parse(split[0][5..]);
            string[] draws = split[1].Split(";");
            foreach (string draw in draws)
            {
                List<(Color c, int n)> drawList = new();
                string[] parts = draw.Split(",");
                foreach (string part in parts)
                {
                    string[] words = part.Trim().Split(" ");
                    drawList.Add((GetColor(words[1]), int.Parse(words[0])));
                }
                Draws.Add(drawList);
            }
        }

        private static Color GetColor(string str)
        {
            return str switch
            {
                "red" => Color.Red,
                "green" => Color.Green,
                "blue" => Color.Blue,
                _ => throw new ArgumentOutOfRangeException(nameof(str)),
            };
        }

        public enum Color
        {
            Red,
            Green,
            Blue
        }
    }
}
