using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        public static BigInteger AddBits(BigInteger mask, int offset, int length)
        {
            mask |= GetBitmask(length) << offset;

            return mask;
        }

        public static void PrintMask(BigInteger mask, int length = 128)
        {
            for (int i = 0; i < length; i++)
            {
                if ((mask & (BigInteger)((BigInteger)1 << i)) != 0)
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

        static private BigInteger[] _cache = [];

        static private BigInteger GetBitmask(int bits)
        {
            return _cache[bits];
        }

        static bool CheckMask(BigInteger currMask, BigInteger andMask, BigInteger orMask, int bitLength)
        {
            BigInteger bitmask = GetBitmask(bitLength);

            //currMask &= bitmask;
            //andMask &= bitmask;
            //orMask &= bitmask;


            if ((andMask & currMask) != (andMask&bitmask))
            {
                return false;
            }

            if ((orMask & currMask) != currMask)
            {
                return false;
            }

            return true;
        }

        static public long CheckVariantLevel(int level, List<int> runs, BigInteger andMask, BigInteger orMask, BigInteger currMask, int startingOffset, int maxLength, List<int> remainingTotals)
        {
            long total = 0;
            int run = runs[level];
            int remainingTotal = remainingTotals[level];

            int biggestOffset = maxLength - startingOffset - run + 1 - remainingTotal;

            if (level == runs.Count - 1)
            {
                for (int i = 0; i < biggestOffset; i++)
                {
                    BigInteger newMask = AddBits(currMask, startingOffset + i, run);

                    //PrintMask(newMask);
                    //PrintMask(andMask);
                    //PrintMask(orMask);

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
                    BigInteger newMask = AddBits(currMask, startingOffset + i, run);

                    if (!CheckMask(newMask, andMask, orMask, startingOffset + i + run))
                    {
                        // early out
                        continue;
                    }

                    total += CheckVariantLevel(level + 1, runs, andMask, orMask, newMask, startingOffset + i + run + 1, maxLength, remainingTotals);
                }
            }

            return total;
        }

        static public long GetVariants(string filename, int expansionFactor = 5)
        {
            var lines = System.IO.File.ReadAllLines(filename);

            long total = 0;

            int maxBits = 512;

            _cache = new BigInteger[maxBits];

            BigInteger mask = 0;

            for (int i = 0; i < maxBits; i++)
            {
                _cache[i] = mask;
                mask |= ((BigInteger)1 << (i));
            }

#if false
            Parallel.ForEach(lines, line =>
#else
            foreach (var line in lines)
#endif
            {
                var parts = line.Split([' ', ',']);

                List<int> runs = [];
                List<int> remainingTotal = [];
                var copyRuns = parts.Skip(1).Select(x => int.Parse(x));

                var source = parts[0];

                BigInteger andMask = 0;
                BigInteger orMask = 0;

                int currentIndex = 0;

                for (int j = 0; j < expansionFactor; j++)
                {
                    runs.AddRange(copyRuns);

                    if (j != 0)
                    {
                        // unknown separator
                        orMask |= ((BigInteger)1 << currentIndex);
                        currentIndex++;
                    }

                    for (int i = 0; i < source.Length; i++, currentIndex++)
                    {
                        char c = source[i];
                        var state = GetSpringState(c);

                        if (state == SpringState.Broken || state == SpringState.Unknown)
                        {
                            orMask |= ((BigInteger)1 << currentIndex);
                        }

                        if (state == SpringState.Broken)
                        {
                            andMask |= ((BigInteger)1 << currentIndex);
                        }
                    }

                    //PrintMask(andMask,128);
                    //PrintMask(orMask,128);
                }

                for(int i = 0; i<runs.Count - 1;i++)
                {
                    var skipRuns = runs.Skip(i + 1);
                    var run = skipRuns.Sum(c => c) + skipRuns.Count();
                    remainingTotal.Add(run);
                }
                remainingTotal.Add(0);

                long thisTotal = CheckVariantLevel(0, runs, andMask, orMask, 0, 0, (source.Length * expansionFactor) + expansionFactor - 1, remainingTotal);

                Console.WriteLine($"Variant total {thisTotal}");

                total += thisTotal;
            }
#if false
            );
#endif
            Console.WriteLine($"Total {total}");

            return total;
        }
    }
}
