using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    public class InstructionHasher
    {
        static private readonly int bitMask = 0b11111111;

        static private Dictionary<string, long> hashCache = [];

        static private long calculateHashForString(string line)
        {
            if(hashCache.TryGetValue(line, out long value))
            {
                return value;
            }

            int currentHash = 0;
            long total = 0;

            foreach (char c in line)
            {
                if (c == ',')
                {
                    total += currentHash;
                    currentHash = 0;
                    continue;
                }

                if (c == '\n')
                {
                    continue;
                }

                currentHash += c;
                currentHash *= 17;
                currentHash &= bitMask;
            }

            total += currentHash;

            hashCache.Add(line, total);

            return total;
        }

        static public long GenerateHash(string fileName, bool doBoxes = true)
        {
            long total = 0;

            var line = System.IO.File.ReadAllText(fileName);

            if(!doBoxes)
            {
                return calculateHashForString(line);
            }

            var instructions = line.Split(',');

            List<(string label, int focalLength)> [] boxes = new List<(string label, int focalLength)> [256];

            for(int i = 0; i < 256; i++)
            {
                boxes[i] = [];
            }

            foreach(var instruction in instructions)
            {
                if(instruction.Contains('='))
                {
                    var parts = instruction.Split('=');
                    var lens = parts[0];
                    var focalLength = int.Parse(parts[1]);
                    var box = boxes[calculateHashForString(lens)];

                    bool found = false;

                    for(int i = 0; i < box.Count; i++)
                    {
                        var item = box[i];
                        if(item.label == lens)
                        {
                            box[i] = (lens, focalLength);
                            found = true;
                            break;
                        }
                    }

                    if(!found)
                    {
                        box.Add((lens, focalLength));
                    }
                }
                else
                {
                    var truncatedInstruction = instruction[..^1];
                    var hash = calculateHashForString(truncatedInstruction);
                    boxes[hash].RemoveAll((item) => item.label == truncatedInstruction);
                }
            }

            for(int boxIndex = 0; boxIndex < 256; boxIndex++)
            {
                var box = boxes[boxIndex];
                for(int lensPos = 0; lensPos < box.Count; lensPos++)
                {
                    var item = box[lensPos];
                    total += (boxIndex + 1) * (lensPos + 1) * item.focalLength;
                }
            }

            return total;
        }

    }
}
