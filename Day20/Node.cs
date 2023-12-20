using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    public enum ModuleType
    {
        Broadcaster,
        FlipFlop,
        Conjunction,
    }

    public enum PulseType
    {
        Low,
        High,
        None
    }

    internal abstract class Module(string _name, List<string> _output)
    {
        public static Module Create(string _name, List<string> _output, ModuleType type)
        {
            switch(type)
            {
                case ModuleType.Broadcaster:
                    return new BroadcastModule(_name, _output);
                case ModuleType.Conjunction:
                    return new ConjunctionModule(_name, _output);
                case ModuleType.FlipFlop:
                    return new FlipFlopModule(_name, _output);
                default:
                    throw new Exception("Unknown node type");
            }
        }


        public readonly string name = _name;
        public readonly List<string> outputs = _output;

        public abstract (int lowPulses, int highPulses) ProcessPulse(PulseType pulse, string from, Queue<(PulseType type, string from, string destination)> queue);
        public abstract void AddInput(string input);
    }

    internal class FlipFlopModule(string _name, List<string> _output) : Module(_name, _output)
    {
        bool isOn = false;

        public override (int lowPulses, int highPulses) ProcessPulse(PulseType pulse, string from, Queue<(PulseType type, string from, string destination)> queue)
        {             
            if (pulse == PulseType.Low)
            {
                if(isOn)
                {
                    isOn = false;
                    foreach (var output in outputs)
                    {
                        queue.Enqueue((PulseType.Low, name, output));
                    }
                    return (outputs.Count, 0);
                }
                else
                {
                    isOn = true;
                    foreach (var output in outputs)
                    {
                        queue.Enqueue((PulseType.High, name, output));
                    }
                    return (0, outputs.Count);
                }
            }

            return (0,0);
        }

        public override void AddInput(string input)
        {
        }
    }

    internal class ConjunctionModule(string _name, List<string> _output) : Module(_name, _output)
    {
        readonly Dictionary<string, PulseType> inputs = [];

        public override (int lowPulses, int highPulses) ProcessPulse(PulseType pulse, string from, Queue<(PulseType type, string from, string destination)> queue)
        {
            inputs[from] = pulse;

            if(pulse == PulseType.Low)
            {
                foreach (var output in outputs)
                {
                    queue.Enqueue((PulseType.High, name, output));
                }
                return (0,outputs.Count);
            }
        
            foreach(var input in inputs)
            {
                if(input.Value == PulseType.Low)
                {
                    foreach (var output in outputs)
                    {
                        queue.Enqueue((PulseType.High, name, output));
                    }
                    return (0, outputs.Count);
                }
            }

            foreach (var output in outputs)
            {
                queue.Enqueue((PulseType.Low, name, output));
            }
            return (outputs.Count, 0);
        }

        public override void AddInput(string input)
        {
            if(inputs.ContainsKey(input))
            {
                throw new Exception("ConjunctionModule already has input");
            }
            inputs.Add(input, PulseType.Low);
        }
    }

    internal class  BroadcastModule(string _name, List<string> _output): Module(_name, _output)
    {
        public override (int lowPulses, int highPulses) ProcessPulse(PulseType pulse, string from, Queue<(PulseType type, string from, string destination)> queue)
        {
            foreach(var output in outputs)
            {
                queue.Enqueue((pulse, name, output));
            }

            if(pulse == PulseType.Low)
            {
                return (outputs.Count,0);
            }
            else
            {
                return (0, outputs.Count);
            }
        }

        public override void AddInput(string input)
        {

        }

    }


}
