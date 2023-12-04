using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day2
{
    public partial class Matcher
    {
        static readonly Dictionary<string, int> MaxNumberByColour = new()
        {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 }
        };

        public static int Process(string filename)
        {
            var text = File.ReadAllLines(filename);

            int total = 0;

            foreach(var line in text)
            {
                var result = ProcessLine(line);
                if(result > 0)
                {
                    Console.WriteLine($"Game {result} is possible");
                    total += result;
                }
            }

            Console.WriteLine($"Total: {total}");

            return total;
        }

        public static int ProcessLine(string line)
        {
            var splitString = line.Split([':',';']);

            var regexResult = GameNumberRegex().Match(splitString[0]);

            if(!regexResult.Success)
            {
                throw new Exception("Regex failed");
            }

            int gameNumber = int.Parse(regexResult.Groups[1].Value);

            for(int i = 1; i < splitString.Length; i++)
            {
                var colourResult = ColourRegex().Matches(splitString[i]);

                if(colourResult.Count == 0)
                {
                    throw new Exception("Regex failed");
                }

                foreach(Match match in colourResult)
                {
                    int count = int.Parse(match.Groups[1].Value);

                    if (!MaxNumberByColour.TryGetValue(match.Groups[2].Value, out int maxNumber))
                    {
                        throw new Exception("Color not found");
                    }

                    if(count > maxNumber)
                    {
                        Console.WriteLine($"Game {gameNumber} is NOT possible, it has {count} {match.Groups[2].Value}s");
                        return 0;
                    }
                }
            }

            return gameNumber;
        }

        [GeneratedRegex(@"Game (\d+)")]
        private static partial Regex GameNumberRegex();

        [GeneratedRegex(@"(\d+) (red|green|blue)")]
        private static partial Regex ColourRegex();
    }
}
