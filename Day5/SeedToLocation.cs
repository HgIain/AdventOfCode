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
        private record RangeMap(long srcStart, long destStart, long length);
        private record SeedRange(long start, long length);

        private readonly List<SeedRange> seeds = [];
        private readonly List<List<RangeMap>> rangeMaps = [];
        List<RangeMap> combined = [];


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

            // smoosh them together
            combined = rangeMaps.Aggregate((a, b) => CombineRanges(a, b));

        }

        private record Offset(long length, long offset);

        private static Offset GetLengthAndOffsetForIndex(List<RangeMap> rangeMap, long index)
        {
            for(int i=0; i<rangeMap.Count; i++)
            {
                var range = rangeMap[i];

                if (index >= range.srcStart && index < range.srcStart + range.length)
                {
                    return new(range.length - (index - range.srcStart), range.destStart - range.srcStart);
                }

                if (range.srcStart > index)
                {
                    return new(range.srcStart - index, 0);
                }
            }

            // off the end
            return new (0, 0);
        }

        private List<RangeMap> CombineRanges(List<RangeMap> one, List<RangeMap> two)
        {
            var maxOne = one[^1].srcStart + one[^1].length;
            var maxTwo = two[^1].srcStart + two[^1].length;
            var overallMax = Math.Max(maxOne, maxTwo);

            List<RangeMap> combined = [];

            long index = 0;

            while(index < overallMax)
            {
                Offset oneOffset = GetLengthAndOffsetForIndex(one, index);
                Offset twoOffset = GetLengthAndOffsetForIndex(two, index + oneOffset.offset);

                if(oneOffset.length == 0 && twoOffset.length == 0)
                {
                    // reached the end
                    break;
                }

                if(twoOffset.length == 0)
                {
                    // off the end of two, just use the one offset
                    combined.Add(new RangeMap(index, oneOffset.offset + index, oneOffset.length));
                    index += oneOffset.length;
                    continue;
                }
                if (oneOffset.length == 0)
                {
                    // off the end of one, just use the two offset
                    combined.Add(new RangeMap(index, twoOffset.offset + index, twoOffset.length));
                    index += twoOffset.length;
                    continue;
                }

                long minLen = Math.Min(oneOffset.length, twoOffset.length);

                if (oneOffset.offset == 0 && twoOffset.offset == 0)
                {
                     // neither offset, so just skip
                     index += minLen;
                    continue;
                }


                // just add the offsets together??
                combined.Add(new RangeMap(index, oneOffset.offset + twoOffset.offset + index, minLen));
                index += minLen;

            }

            return combined;

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
            long min = long.MaxValue;
            long index = seedRange.start;
            while(index < seedRange.start + seedRange.length)
            {
                var offset = GetLengthAndOffsetForIndex(combined, index);

                var value = index + offset.offset;
                if (value < min)
                {
                    min = value;
                }

                if(offset.length == 0)
                {
                    // off the end of the remapper, we're done
                    break;
                }
                // skip over lots of values if we can
                index += offset.length;
            }

            return min;
        }
    }
}
