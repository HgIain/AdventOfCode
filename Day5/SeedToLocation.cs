using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5
{
    public class SeedToLocation(string filename)
    {
        private record RangeMap(long srcStart, long destStart, long length);

        private IEnumerable<long> seeds = [];
        private readonly List<List<RangeMap>> rangeMaps = [];

        public void ProcessMaps(string[] text)
        {
            seeds = text[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(c=> long.Parse(c));

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
                    rangeMap = new();
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
                rangeMaps[i] = rangeMaps[i].OrderBy(c=>c.srcStart).ToList();
            }

        }

        private long LookupValue(long value, List<RangeMap> rangeMap)
        {
            int index = rangeMap.FindIndex(c=>c.srcStart <= value && c.srcStart + c.length > value);

            if (index == -1)
            {
                return value;
            }

            var range = rangeMap[index];

            return rangeMap[index].destStart + (value - rangeMap[index].srcStart);
        }

        public long Process()
        {
            var text = File.ReadAllLines(filename);

            ProcessMaps(text);

            long minLocation = long.MaxValue;

            foreach (var seed in seeds)
            {
                var result = ProcessSeed(seed);
                
                if(result < minLocation)
                {
                    minLocation = result;
                }
            }

            Console.WriteLine($"Lowest location: {minLocation}");

            return minLocation;
        }

        public long ProcessSeed(long seed)
        {
            long value = seed;

            foreach (var rangeMap in rangeMaps)
            {
                var oldvalue = value;
                value = LookupValue(value,rangeMap);

                Console.WriteLine($"Seed {seed} -> {oldvalue} -> {value}");
            }

            return value;
        }
    }
}
