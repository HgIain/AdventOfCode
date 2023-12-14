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
        private static readonly Dictionary<string, long> cache = [];

        static private string GetKey(string sourceData, List<int> runs)
        {
            var key = sourceData + ":" + string.Join(",", runs);
            return key;
        }

        static public long CheckVariantLevel(string sourceData, List<int> runs, List<int> remainingTotal)
        {
            long total = 0;
            string cacheKey = GetKey(sourceData, runs);
            if(cache.TryGetValue(cacheKey, out var cachedValue))
            {
                return cachedValue;
            }

            if (runs.Count == 0)
            {
                // no more runs to fit
                if(sourceData.Contains('#'))
                {
                    // still have a mandatory run to fit
                    total = 0;
                }
                else
                {
                    // we have filled all the runs
                    total = 1;
                }
            }
            else if (sourceData.Length < remainingTotal.First())
            {
                // not enough space to fit the next run
                total = 0;
            }
            else if (sourceData[0] == '.')
            {
                // skip this character
                total = CheckVariantLevel(sourceData[1..], runs, remainingTotal);
            }
            else
            {
                if (sourceData[0] == '?')
                {
                    // we have a choice, choose both
                    total = CheckVariantLevel(sourceData[1..], runs, remainingTotal);
                }


                // try to fit the next run
                var run = runs.First();
                var remaining = runs[1..];
                var remainingTotal2 = remainingTotal[1..];

                bool fits = true;

                for (int i = 0; i < run; i++)
                {
                    if (sourceData[i] == '.')
                    {
                        fits = false;
                        break;
                    }
                }

                if (fits)
                {
                    if(sourceData.Length == run)
                    {
                        // we have filled all the runs
                        total += 1;
                    }
                    else
                    {
                        // move onto the next run
                        if (sourceData[run] != '#')
                        {
                            total += CheckVariantLevel(sourceData[(run + 1)..], remaining, remainingTotal2);
                        }
                    }
                }
                    
            }

            cache[cacheKey] = total;
            return total;
        }

        static public long GetVariants(string filename, int expansionFactor = 5)
        {
            var lines = System.IO.File.ReadAllLines(filename);

            long total = 0;



            foreach (var line in lines)
            {
                var parts = line.Split([' ', ',']);

                List<int> runs = [];
                List<int> remainingTotal = [];
                var copyRuns = parts.Skip(1).Select(x => int.Parse(x));

                var source = parts[0];
                runs.AddRange(copyRuns);

                for (int j = 1; j < expansionFactor; j++)
                {
                    source += "?" + parts[0];
                    runs.AddRange(copyRuns);
                }

                for(int i = 0; i<runs.Count;i++)
                {
                    var skipRuns = runs.Skip(i);
                    var run = skipRuns.Sum(c => c) + skipRuns.Count() - 1;
                    remainingTotal.Add(run);
                }

                long thisTotal = CheckVariantLevel(source, runs, remainingTotal);

                Console.WriteLine($"Variant total {thisTotal}");

                total += thisTotal;
            }

            Console.WriteLine($"Total {total}");

            return total;
        }
    }
}
