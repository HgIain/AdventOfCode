using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day1Point5
{
    public class Matcher
    {
        static readonly Dictionary<string, int> _numbers = new()
        {
            { "zero", 0 },
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 },
        };

        private static int Match(string line)
        {
            if(_numbers.TryGetValue(line, out int number))
            {
                return(number);
            }

            return int.Parse(line);
        }

        public static int Process(string filename)
        {
            var text = File.ReadAllLines(filename);

            int total = 0;

            var regexString = @"(zero|one|two|three|four|five|six|seven|eight|nine|\d)";
            var regex = new Regex(regexString);
            var regexBack = new Regex(regexString, RegexOptions.RightToLeft);

            foreach (var line in text)
            {
                var regexResult = regex.Match(line);
                int value = 0;

                if (!regexResult.Success)
                {
                    throw new Exception("Regex failed");
                }

                value += Match(regexResult.Groups[1].Value) * 10;

                regexResult = regexBack.Match(line);
                if (!regexResult.Success)
                {
                    throw new Exception("Regex failed");
                }
                value += Match(regexResult.Groups[1].Value);

                Console.WriteLine($"Value: {value}");
                total += value;
            }

            Console.WriteLine($"Total: {total}");

            return total;
        }
    }
}
