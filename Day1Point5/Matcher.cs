using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day1Point5
{
    internal class Matcher
    {
        static readonly Dictionary<string, int> _numbers = new Dictionary<string, int>
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

        public static int Match(string line)
        {
            if(_numbers.TryGetValue(line, out int number))
            {
                return(number);
            }

            return int.Parse(line);
        }
    }
}
