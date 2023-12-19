using System;
using System.Collections.Generic;
using System.Linq;
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

        private record ItemPart(int x, int m, int a, int s);

        private readonly Dictionary<string, Workflow> workflows = [];
        private readonly List<ItemPart> itemParts = [];

        public FactoryProcessor(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            int i = 0;

            for (; i < lines.Length; i++)
            {
                var line = lines[i];
                if(string.IsNullOrWhiteSpace(line))
                {
                    i++;
                    break;
                }

                char[] chars = ['{','}'];
                var parts = line.Split(chars, StringSplitOptions.RemoveEmptyEntries| StringSplitOptions.TrimEntries);

                if(parts.Length != 2)
                {
                    throw new Exception("Invalid workflow");
                }

                var workflowName = parts[0];
                var instructions = parts[1].Split(',');
                var workflow = new Workflow(workflowName, []);


                foreach(var instruction in instructions)
                {
                    char[] splitchars;
                    Comparison comparison;

                    if (instruction.Contains('>'))
                    {
                        splitchars = ['>', ':'];
                        comparison = Comparison.GreaterThan;
                    }
                    else if(instruction.Contains('<'))
                    {
                        splitchars = ['<', ':'];
                        comparison = Comparison.LessThan;
                    }
                    else
                    {
                        if(instruction == "A")
                        { 
                            workflow.instructions.Add(new Instruction('a', Comparison.None, 0, Action.Accept, ""));
                        }
                        else if(instruction == "R")
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

                    if(instructionParts.Length != 3)
                    {
                        throw new Exception("Invalid instruction");
                    }

                    var value = int.Parse(instructionParts[1]);
                    var forwardWorkflow = instructionParts[2];
                    char type = instructionParts[0][0];

                    if(forwardWorkflow == "A")
                    {
                        workflow.instructions.Add(new Instruction(type, comparison, value, Action.Accept, ""));
                    }
                    else if(forwardWorkflow == "R")
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
                itemParts.Add(new ItemPart(values[0], values[1], values[2], values[3]));
            }
        }

        public int GetValue()
        {
            int total = 0;

            foreach(var itemPart in itemParts)
            {
                if(isAccepted(itemPart))
                {
                    total += itemPart.x + itemPart.m + itemPart.a + itemPart.s;
                }
            }

            Console.WriteLine($"Total: {total}");

            return total;
        }

        private Action RunWorkflowForItem(Workflow flow,ItemPart itemPart, out string forwardingWorkflow)
        {
            foreach(var instruction in flow.instructions)
            {
                if(instruction.comparison == Comparison.None)
                {
                    forwardingWorkflow = instruction.forwardWorkflow;
                    return instruction.action;
                }

                int comparingValue = instruction.type switch
                {
                    'x' => itemPart.x,
                    'm' => itemPart.m,
                    'a' => itemPart.a,
                    's' => itemPart.s,
                    _ => throw new Exception("Invalid instruction"),
                };

                if(instruction.comparison == Comparison.GreaterThan)
                {
                    if(comparingValue > instruction.comparedValue)
                    {
                        forwardingWorkflow = instruction.forwardWorkflow;
                        return instruction.action;
                    }
                }
                else if(instruction.comparison == Comparison.LessThan)
                {
                    if(comparingValue < instruction.comparedValue)
                    {
                        forwardingWorkflow = instruction.forwardWorkflow;
                        return instruction.action;
                    }
                }
            }

            throw new Exception("Invalid workflow");
        }

        bool isAccepted(ItemPart itemPart)
        {
            var flow = workflows["in"];

            while(true)
            {
                var action = RunWorkflowForItem(flow, itemPart, out string forwardingWorkflow);

                if(action == Action.Accept)
                {
                    return true;
                }
                else if(action == Action.Reject)
                {
                    return false;
                }

                flow = workflows[forwardingWorkflow];
            }
        }

    }
}
