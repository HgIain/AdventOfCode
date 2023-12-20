using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    public class PulseTracker
    {
        private readonly Dictionary<string, Module> _modules = [];

        private readonly Queue<(PulseType type, string from, string destination)> _pulseQueue = [];

        private readonly Dictionary<string, List<long>> rxInputsHighPulseTime = [];

        public PulseTracker(string filename)
        {
            var lines = File.ReadAllLines(filename);

            char[] splitChars = [' ', '-', '>',','];

            foreach (var line in lines)
            {
                var parts = line.Split(splitChars, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var name = parts[0];

                if(name == "broadcaster")
                {
                    var outputs = parts[1..].ToList();

                    var module = Module.Create(name, outputs, ModuleType.Broadcaster);
                    _modules.Add(name, module);
                }
                else
                {
                    var type =  name[0] switch
                    {
                        '&' => ModuleType.Conjunction,
                        '%' => ModuleType.FlipFlop,
                        _ => throw new Exception("Unknown node type")
                    };
                    var module = Module.Create(name[1..], parts[1..].ToList(), type);
                    _modules.Add(name[1..], module);
                }
            }

            foreach (var module in _modules.Values)
            {
                foreach (var output in module.outputs)
                {
                    if (_modules.TryGetValue(output, out var outputModule))
                    {
                        outputModule.AddInput(module.name);
                    }

                    if(output == "th")
                    {
                        rxInputsHighPulseTime.Add(module.name, []);
                    }
                }
            }

            Console.WriteLine("Loaded {0} modules", _modules.Count);
        }

        public long ProcessPulses()
        {
            int high = 0;
            int low = 0;
            for (int i = 0; i < 1000; i++)
            {
                _pulseQueue.Enqueue((PulseType.Low, "", "broadcaster"));
                low++;
                while (_pulseQueue.Count > 0)
                {
                    var (pulse, from, destination) = _pulseQueue.Dequeue();
                    if (_modules.TryGetValue(destination, out var module))
                    {
                        var (newlow, newhigh) = module.ProcessPulse(pulse, from, _pulseQueue);
                        high += newhigh;
                        low += newlow;
                    }
                }
            }
       

            long result = high * low;

            Console.WriteLine("High: {0}, Low: {1}, Result: {2}", high, low, result);

            return result;
        }

        public long MinimumPulsesToRx()
        {
            long i;
            for (i = 0; ; i++)
            {
                _pulseQueue.Enqueue((PulseType.Low, "", "broadcaster"));
                while (_pulseQueue.Count > 0)
                {
                    var (pulse, from, destination) = _pulseQueue.Dequeue();

                    if(destination == "rx")
                    {
                        if (pulse == PulseType.Low)
                        {
                            Console.WriteLine($"Button presses = {i + 1}");
                            return i + 1;
                        }
                    }

                    if(destination == "th")
                    {
                        if (pulse == PulseType.High)
                        {
                            rxInputsHighPulseTime[from].Add(i + 1);
                        }

                        bool done = true;
                        long cycleTime = 1;

                        foreach(var pulseTime in rxInputsHighPulseTime)
                        {
                            if (pulseTime.Value.Count == 0)
                            {
                                done = false;
                                break;
                            }
                            else
                            {
                                cycleTime *= pulseTime.Value[0];
                            }
                        }

                        if (done)
                        {
                            return cycleTime;
                        }
                    }

                    if (_modules.TryGetValue(destination, out var module))
                    {
                        var (newlow, newhigh) = module.ProcessPulse(pulse, from, _pulseQueue);
                    }
                }
            }




        }
    }
}
