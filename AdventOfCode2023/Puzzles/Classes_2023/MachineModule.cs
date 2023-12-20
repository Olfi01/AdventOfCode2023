using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Puzzles.Classes_2023
{
    public abstract class MachineModule
    {
        public string Name { get; }
        public string[] DestinationModules { get; }

        public abstract Pulse? ProcessPulse(Pulse input, string from);

        public static MachineModule From(string input)
        {
            string[] split = input.Split(" -> ");
            if (split[0] == "broadcaster")
            {
                return new BroadcasterModule(split[0], split[1].Split(", "));
            }
            else if (split[0].StartsWith('%'))
            {
                return new FlipFlopModule(split[0][1..], split[1].Split(", "));
            }
            else if (split[0].StartsWith('&'))
            {
                return new ConjunctionModule(split[0][1..], split[1].Split(", "));
            }
            else throw new NotImplementedException();
        }

        public MachineModule(string name, string[] destinationModules)
        {
            Name = name;
            DestinationModules = destinationModules;
        }

        public enum Pulse
        {
            Low,
            High
        }
    }

    public class FlipFlopModule : MachineModule
    {
        public FlipFlopModule(string name, string[] destinationModules) : base(name, destinationModules)
        {
        }

        public bool On { get; private set; }
        public override Pulse? ProcessPulse(Pulse input, string from)
        {
            if (input == Pulse.Low)
            {
                On = !On;
                return On ? Pulse.High : Pulse.Low;
            }
            return null;
        }
    }

    public class ConjunctionModule : MachineModule
    {
        public ConjunctionModule(string name, string[] destinationModules) : base(name, destinationModules)
        {
        }

        public Dictionary<string, Pulse> StoredPulses { get; } = new();

        public override Pulse? ProcessPulse(Pulse input, string from)
        {
            StoredPulses[from] = input;
            return StoredPulses.Values.All(x => x == Pulse.High) ? Pulse.Low : Pulse.High;
        }
    }

    public class BroadcasterModule : MachineModule
    {
        public BroadcasterModule(string name, string[] destinationModules) : base(name, destinationModules)
        {
        }

        public override Pulse? ProcessPulse(Pulse input, string from)
        {
            return input;
        }
    }
}
