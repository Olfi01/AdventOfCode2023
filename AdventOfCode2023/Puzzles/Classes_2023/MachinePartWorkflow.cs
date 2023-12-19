using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023.Puzzles.Classes_2023
{
    public class MachinePartWorkflow
    {
        public string Name { get; }
        public Rule[] Rules { get; }

        public MachinePartWorkflow(string input)
        {
            string[] split = input.Split('{');
            Name = split[0];
            string[] rules = split[1][..^1].Split(',');
            Rules = rules.Select(x => new Rule(x)).ToArray();
        }

        public class Rule
        {
            public MachinePart.Property? Property { get; }
            public Operation? Operation { get; }
            public int? Compare { get; }
            public string TargetWorkflow { get; }
            public Rule(string input)
            {
                string[] split = input.Split(':');
                if (split.Length < 2) TargetWorkflow = split[0];
                else
                {
                    TargetWorkflow = split[1];
                    if (split[0].Contains('>'))
                    {
                        Operation = MachinePartWorkflow.Operation.gt;
                        string[] operators = split[0].Split('>');
                        Compare = int.Parse(operators[1]);
                        Property = GetProperty(operators[0]);
                    }
                    else if (split[0].Contains('<'))
                    {
                        Operation = MachinePartWorkflow.Operation.lt;
                        string[] operators = split[0].Split('<');
                        Compare = int.Parse(operators[1]);
                        Property = GetProperty(operators[0]);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            private static MachinePart.Property GetProperty(string v)
            {
                return v switch
                {
                    "x" => MachinePart.Property.x,
                    "m" => MachinePart.Property.m,
                    "a" => MachinePart.Property.a,
                    "s" => MachinePart.Property.s,
                    _ => throw new NotImplementedException()
                };
            }
        }

        public enum Operation
        {
            lt,
            gt
        }
    }

    public class MachinePart
    {
        public Dictionary<Property, int> Properties { get; } = new();

        private static readonly Regex machinePartRegex = new(@"{x=(?<x>\d+),m=(?<m>\d+),a=(?<a>\d+),s=(?<s>\d+)}");

        public MachinePart(string input)
        {
            Match match = machinePartRegex.Match(input);
            Properties[Property.x] = int.Parse(match.Groups["x"].Value);
            Properties[Property.m] = int.Parse(match.Groups["m"].Value);
            Properties[Property.a] = int.Parse(match.Groups["a"].Value);
            Properties[Property.s] = int.Parse(match.Groups["s"].Value);
        }

        public enum Property
        {
            x,
            m,
            a,
            s
        }
    }
}
