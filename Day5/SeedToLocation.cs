using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5
{
    public class SeedToLocation(string filename)
    {
        private record RangeMap(long srcStart, long destStart, long length) :IComparable<RangeMap>
        {
            public int CompareTo(RangeMap? other)
            {
                if(other is null)
                {
                    return 1;
                }

                if (other.srcStart < srcStart)
                {
                    return 1;
                }

                if (other.srcStart >= srcStart + length)
                {
                    return -1;
                }


                return 0;
            }
        }
        private record SeedRange(long start, long length);

        private readonly List<SeedRange> seeds = [];
        private readonly List<List<RangeMap>> rangeMaps = [];

        public void ProcessMaps(string[] text, bool seedRange)
        {
            if (!seedRange)
            {
                var singleseeds = text[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(c => long.Parse(c)).ToList();

                foreach (var seed in singleseeds)
                {
                    seeds.Add(new(seed, 1));
                }
            }
            else
            {
                var seedRanges = text[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(c => long.Parse(c)).ToList();

                for(int i=0; i<seedRanges.Count; i+=2)
                {
                    seeds.Add(new(seedRanges[i], seedRanges[i + 1]));
                }
            }

            List<RangeMap>? rangeMap = null;

            for(int i = 1; i<text.Length; i++)
            {
                var line = text[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.EndsWith("map:"))
                {
                    rangeMap = [];
                    rangeMaps.Add(rangeMap);
                    continue;
                }

                var range = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c=>long.Parse(c)).ToList();

                if (range.Count != 3)
                {
                    throw new Exception("Unexpected string format");
                }

                if (rangeMap is null)
                {
                    throw new Exception("Unexpected map format");
                }

                rangeMap.Add(new RangeMap(range[1], range[0], range[2]));
            }

            for(int i =0; i<rangeMaps.Count;i++)
            {
                rangeMaps[i] = [.. rangeMaps[i].OrderBy(c=>c.srcStart)];
            }

            //CombineRanges(rangeMaps[0], rangeMaps[1]);

        }

        private List<RangeMap> CombineRanges(List<RangeMap> one, List<RangeMap> two)
        {
            var maxOne = one[^1].srcStart + one[^1].length;
            var maxTwo = two[^1].srcStart + two[^1].length;
            var overallMax = Math.Max(maxOne, maxTwo);

            List<RangeMap> combined = [];

            int oneIndex = 0;
            int twoIndex = 0;

            while(oneIndex < maxOne && twoIndex < maxTwo)
            {
                var oneRange = one[oneIndex];
                var twoRange = two[twoIndex];

                if (oneRange.srcStart < twoRange.srcStart)
                {
                    combined.Add(oneRange);
                    oneIndex++;
                }
                else
                {
                    combined.Add(twoRange);
                    twoIndex++;
                }
            }

            return combined;

        }


        private static long LookupValue(long value, List<RangeMap> rangeMap)
        {
            var compareRange = new RangeMap(value, 0, 1);

            int index = rangeMap.BinarySearch(compareRange);

            if (index < 0)
            {
                return value;
            }

            var range = rangeMap[index];

            return range.destStart + (value - range.srcStart);
        }

        public long Process(bool useSeedRanges = false)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var text = File.ReadAllLines(filename);

            ProcessMaps(text, useSeedRanges);

            ConcurrentBag<long> allMins = [];


            Parallel.ForEach(seeds, (seed) =>
            {
                var result = ProcessSeedRange(seed);

                allMins.Add(result);
            });


            long minLocation = allMins.Min();
            Console.WriteLine($"Lowest location: {minLocation}");

            stopwatch.Stop();
            Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds}ms");

            return minLocation;
        }

        private long ProcessSeedRange(SeedRange seedRange)
        {
            ConcurrentBag<long> allMins = [];

            Parallel.For<long> ( 0, seedRange.length, () => long.MaxValue,
                (i, loop, localMin) =>
                {
                    long value = seedRange.start + i;

                    foreach (var rangeMap in rangeMaps)
                    {
                        var oldvalue = value;
                        value = LookupValue(value, rangeMap);
                    }

                    if (value < localMin)
                    {
                        localMin = value;
                    }

                    return localMin;
                },
                (x) =>
                {
                    allMins.Add(x);
                }
            );


            return allMins.Min();
        }
    }
}
