using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    public class OperationalSpring
    {
        enum SpringState
        {
            Unknown,
            Broken,
            Working,
            None,
        }

        private record Spring(SpringState state, int run);

        static private SpringState GetSpringState(char c) => c switch
        {
            '?' => SpringState.Unknown,
            '#' => SpringState.Broken,
            '.' => SpringState.Working,
            _ => throw new Exception("Unexpected spring state")
        };

        public static ulong AddBits(ulong mask, int offset, int length)
        {
            for (int i = 0; i < length; i++)
            {
                mask |= ((ulong)1 << (offset + i));
            }

            return mask;
        }

        public static void PrintMask(ulong mask, int length = 32)
        {
            for (int i = 0; i < length; i++)
            {
                if ((mask & ((ulong)1 << i)) != 0)
                {
                    Console.Write("1");
                }
                else
                {
                    Console.Write("0");
                }
            }
            Console.WriteLine();
        }


        static public long CheckVariantLevel(int level, List<int> runs, ulong andMask, ulong orMask, ulong currMask, int startingOffset, int maxLength)
        {
            long total = 0;
            int run = runs[level];

            int biggestOffset = maxLength - startingOffset - run + 1;

            if (level == runs.Count - 1)
            {
                for (int i = 0; i < biggestOffset; i++)
                {
                    ulong newMask = AddBits(currMask, startingOffset + i, run);

                    if ((andMask & newMask) != andMask)
                    {
                        continue;
                    }

                    if ((orMask & newMask) != newMask)
                    {
                        continue;
                    }
                    total++;
                }
            }
            else
            {
                for (int i = 0; i < biggestOffset; i++)
                {
                    ulong newMask = AddBits(currMask, startingOffset + i, run);

                    total += CheckVariantLevel(level + 1, runs, andMask, orMask, newMask, startingOffset + i + run + 1, maxLength);
                }
            }

            return total;
        }

        static public long GetVariants(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename);

            long total = 0;

            foreach (var line in lines)
            {
                var parts = line.Split([' ', ',']);

                var runs = parts.Skip(1).Select(x => int.Parse(x)).ToList();
                var totalWorking = runs.Sum();

                var source = parts[0];

                if(source.Length > 64)
                {
                    throw new Exception("Bugger!");
                }

                ulong andMask = 0;
                ulong orMask = 0;

                for (int i = 0; i < source.Length; i++)
                {
                    char c = source[i];
                    var state = GetSpringState(c);

                    if (state == SpringState.Broken || state == SpringState.Unknown)
                    {
                        orMask |= ((uint)1 << i);
                    }

                    if(state == SpringState.Broken)
                    {
                        andMask |= ((uint)1 << i);
                    }
                }

                long thisTotal = CheckVariantLevel(0, runs, andMask, orMask, 0, 0, source.Length);

                Console.WriteLine($"Variant total {thisTotal}");

                total += thisTotal;
            }
            Console.WriteLine($"Total {total}");

            return total;
        }
    }
}
