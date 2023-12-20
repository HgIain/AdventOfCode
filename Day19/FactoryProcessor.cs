using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Day19
{
    public class FactoryProcessor
    {
        private enum Comparison
        {
            None,
            GreaterThan,
            LessThan,
        }

        private enum Action
        {
            Accept,
            Reject,
            Forward,
        }

        private record Instruction(char type, Comparison comparison, int comparedValue, Action action, string forwardWorkflow);
        private record Workflow(string name, List<Instruction> instructions);

        private record ValueRange(int start, int end)
        {
            public bool Contains(int value)
            {
                return value >= start && value < end;
            }

            public (ValueRange start,ValueRange end) Split(int value)
            {
                if (!Contains(value))
                {
                    throw new Exception("Value not in range");
                }

                return (new ValueRange(start, value), new ValueRange(value, end));
            }

            public ulong Length => (ulong)(end - start);
        }

        private record ItemPart(ValueRange x, ValueRange m, ValueRange a, ValueRange s);



        private readonly Dictionary<string, Workflow> workflows = [];
        private readonly Queue<(ItemPart part, string nextFlow)> itemParts = [];
        private readonly List<ItemPart> acceptedItemParts = [];
        private readonly bool bUseRange = true;

        private void ProcessWorkflow(string line)
        {
            char[] chars = ['{', '}'];
            var parts = line.Split(chars, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (parts.Length != 2)
            {
                throw new Exception("Invalid workflow");
            }

            var workflowName = parts[0];
            var instructions = parts[1].Split(',');
            var workflow = new Workflow(workflowName, []);


            foreach (var instruction in instructions)
            {
                char[] splitchars;
                Comparison comparison;

                if (instruction.Contains('>'))
                {
                    splitchars = ['>', ':'];
                    comparison = Comparison.GreaterThan;
                }
                else if (instruction.Contains('<'))
                {
                    splitchars = ['<', ':'];
                    comparison = Comparison.LessThan;
                }
                else
                {
                    if (instruction == "A")
                    {
                        workflow.instructions.Add(new Instruction('a', Comparison.None, 0, Action.Accept, ""));
                    }
                    else if (instruction == "R")
                    {
                        workflow.instructions.Add(new Instruction('a', Comparison.None, 0, Action.Reject, ""));
                    }
                    else
                    {
                        workflow.instructions.Add(new Instruction('a', Comparison.None, 0, Action.Forward, instruction));
                    }
                    continue;
                }

                var instructionParts = instruction.Split(splitchars, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (instructionParts.Length != 3)
                {
                    throw new Exception("Invalid instruction");
                }

                var value = int.Parse(instructionParts[1]);
                var forwardWorkflow = instructionParts[2];
                char type = instructionParts[0][0];

                if (forwardWorkflow == "A")
                {
                    workflow.instructions.Add(new Instruction(type, comparison, value, Action.Accept, ""));
                }
                else if (forwardWorkflow == "R")
                {
                    workflow.instructions.Add(new Instruction(type, comparison, value, Action.Reject, ""));
                }
                else
                {
                    workflow.instructions.Add(new Instruction(type, comparison, value, Action.Forward, forwardWorkflow));
                }
            }

            workflows.Add(workflowName, workflow);

        }

        public FactoryProcessor(string fileName, bool _bUseRange = true)
        {
            var lines = File.ReadAllLines(fileName);
            int i = 0;

            bUseRange = _bUseRange;

            for (; i < lines.Length; i++)
            {
                var line = lines[i];
                if(string.IsNullOrWhiteSpace(line))
                {
                    i++;
                    break;
                }

                ProcessWorkflow(line);
            }

            if (!bUseRange)
            {
                for (; i < lines.Length; i++)
                {
                    char[] splitchars = [',', '{', '}'];
                    var itemStringParts = lines[i].Split(splitchars, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    if (itemStringParts.Length != 4)
                    {
                        throw new Exception("Invalid item part");
                    }

                    int[] values = new int[4];

                    for (int j = 0; j < 4; j++)
                    {
                        var itemSubParts = itemStringParts[j].Split('=');

                        if (itemSubParts.Length != 2)
                        {
                            throw new Exception("Invalid item part");
                        }

                        values[j] = int.Parse(itemSubParts[1]);
                    }
                    itemParts.Enqueue((new ItemPart(new ValueRange(values[0], values[0] + 1), new ValueRange(values[1], values[1] + 1), new ValueRange(values[2], values[2] + 1), new ValueRange(values[3], values[3] + 1)), "in"));
                }
            }
            else
            {
                itemParts.Enqueue((new ItemPart(new ValueRange(1, 4001), new ValueRange(1, 4001), new ValueRange(1, 4001), new ValueRange(1, 4001)), "in"));
            }
        }

        public ulong GetValue()
        {
            ulong total = 0;

            while (itemParts.Count > 0)
            {
                var (part, flow) = itemParts.Dequeue();
                RunWorkflowForItem(workflows[flow], part);
            }

            if (bUseRange)
            {
                foreach (var itemPart in acceptedItemParts)
                {
                    total += itemPart.x.Length * itemPart.m.Length * itemPart.a.Length * itemPart.s.Length;
                }
            }
            else
            {
                foreach (var itemPart in acceptedItemParts)
                {
                    total += (ulong)(itemPart.x.start + itemPart.m.start + itemPart.a.start + itemPart.s.start);
                }
            }

            Console.WriteLine($"Total: {total}");

            return total;
        }

        private void FinaliseItem(ItemPart itemPart, Instruction instruction)
        {
            if (instruction.action == Action.Accept)
            {
                acceptedItemParts.Add(itemPart);
            }
            else if (instruction.action == Action.Forward)
            {
                // add it back on the queue
                itemParts.Enqueue((itemPart, instruction.forwardWorkflow));
            }
        }

        ItemPart UpdateItemPartRange(ItemPart part, char type, ValueRange range)
        {
            return type switch
            {
                'x' => new ItemPart(range, part.m, part.a, part.s),
                'm' => new ItemPart(part.x, range, part.a, part.s),
                'a' => new ItemPart(part.x, part.m, range, part.s),
                's' => new ItemPart(part.x, part.m, part.a, range),
                _ => throw new Exception("Invalid instruction"),
            };
        }

        private void RunWorkflowForItem(Workflow flow,ItemPart itemPart)
        {
            foreach(var instruction in flow.instructions)
            {
                if(instruction.comparison == Comparison.None)
                {
                    FinaliseItem(itemPart, instruction);

                    return;
                }

                ValueRange comparingValue = instruction.type switch
                {
                    'x' => itemPart.x,
                    'm' => itemPart.m,
                    'a' => itemPart.a,
                    's' => itemPart.s,
                    _ => throw new Exception("Invalid instruction"),
                };

                if(instruction.comparison == Comparison.GreaterThan)
                {
                    if(comparingValue.start > instruction.comparedValue)
                    {
                        FinaliseItem(itemPart, instruction);

                        return;
                    }
                    else if(comparingValue.Contains(instruction.comparedValue) )
                    {
                        var (start, end) = comparingValue.Split(instruction.comparedValue + 1);

                        var partToEnqueue = UpdateItemPartRange(itemPart, instruction.type, end);

                        FinaliseItem(partToEnqueue, instruction);

                        // split the item and add the second part back on the queue
                        itemPart = UpdateItemPartRange(itemPart, instruction.type, start);
                        continue;
                    }
                }
                else if(instruction.comparison == Comparison.LessThan)
                {
                    if(comparingValue.end - 1  < instruction.comparedValue)
                    {
                        FinaliseItem(itemPart, instruction);

                        return;
                    }
                    else if (comparingValue.Contains(instruction.comparedValue))
                    {
                        var (start, end) = comparingValue.Split(instruction.comparedValue);

                        var partToEnqueue = UpdateItemPartRange(itemPart, instruction.type, start);

                        FinaliseItem(partToEnqueue, instruction);

                        // split the item and add the second part back on the queue
                        itemPart = UpdateItemPartRange(itemPart, instruction.type, end);
                        continue;
                    }
                }
            }

            throw new Exception("Invalid workflow");
        }



    }
}
