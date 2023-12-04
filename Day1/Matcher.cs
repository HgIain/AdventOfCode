using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day1
{
    public partial class Matcher
    {
        public static int Process(string filename)
        {
            var text = File.ReadAllLines(filename);

            int total = 0;

            var regex = NumberRegex();

            foreach (var line in text)
            {
                var regexResult = regex.Matches(line);
                int value = 0;

                if (regexResult.Count == 0)
                {
                    throw new Exception("Regex failed");
                }

                value += int.Parse(regexResult[0].Value) * 10;
                value += int.Parse(regexResult[^1].Value);

                Console.WriteLine($"Value: {value}");
                total += value;
            }

            Console.WriteLine($"Total: {total}");

            return total;
        }

        [GeneratedRegex(@"(\d)")]
        private static partial Regex NumberRegex();
    }
}
