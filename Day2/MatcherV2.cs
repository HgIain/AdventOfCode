using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day2
{
    public partial class MatcherV2
    {

        public static int Process(string filename)
        {
            var text = File.ReadAllLines(filename);

            int total = 0;

            foreach(var line in text)
            {
                var result = ProcessLine(line);
                Console.WriteLine($"Game power is {result}");
                total += result;
            }

            Console.WriteLine($"Total: {total}");

            return total;
        }

        public static int ProcessLine(string line)
        {
            Dictionary<string, int> minByColour = new()
            {
                { "red", 0 },
                { "green", 0 },
                { "blue", 0 }
            };

            var colourResult = ColourRegex().Matches(line);

            if (colourResult.Count == 0)
            {
                throw new Exception("Regex failed");
            }

            foreach (Match match in colourResult)
            {
                int count = int.Parse(match.Groups[1].Value);

                if (!minByColour.TryGetValue(match.Groups[2].Value, out int minNumber))
                {
                    throw new Exception("Color not found");
                }

                if (count > minNumber)
                {
                    minByColour[match.Groups[2].Value] = count;
                }
            }

            int power = minByColour["red"] * minByColour["green"] * minByColour["blue"];

            return power;
        }

        [GeneratedRegex(@"(\d+) (red|green|blue)")]
        private static partial Regex ColourRegex();
    }
}
