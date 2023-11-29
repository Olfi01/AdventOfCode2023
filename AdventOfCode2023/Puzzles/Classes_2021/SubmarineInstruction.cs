using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Puzzles.Classes_2021
{
    public record SubmarineInstruction
    {
        public EDirection Direction { get; init; }
        public int Amount {  get; init; }

        public static SubmarineInstruction From(string str)
        {
            string[] split = str.Split(' ');
            if (split.Length != 2) throw new ArgumentException("Invalid submarine instruction", str);
            EDirection direction;
            int amount;
            switch (split[0])
            {
                case "forward":
                    direction = EDirection.Forward;
                    break;
                case "down":
                    direction = EDirection.Down;
                    break;
                case "up":
                    direction = EDirection.Up;
                    break;
                default: throw new ArgumentException("Invalid submarine instruction", str);
            }
            if (!int.TryParse(split[1], out amount)) throw new ArgumentException("Invalid submarine instruction", str);
            return new SubmarineInstruction { Direction = direction, Amount = amount };
        }

        public enum EDirection
        {
            Forward, Down, Up
        }
    }
}
