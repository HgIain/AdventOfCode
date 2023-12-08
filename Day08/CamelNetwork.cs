using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Day08
{
    public class CamelNetwork(string filename)
    {
        private record Destinations(string left, string right);

        private readonly Dictionary<string, Destinations> _network = [];
        private string _current = "AAA";
        private string _directions = "";

        public int Process()
        {
            int result = 0;

            var lines = File.ReadLines(filename);

            _directions = lines.First();

            lines = lines.Skip(2);

            char[] separators = [' ', '=', '(', ')', ','];

            foreach (var line in lines)
            {
                var parts = line.Split(separators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if(parts.Length != 3)
                {
                    throw new Exception("Invalid line");
                }
                var start = parts[0];
                var left = parts[1];
                var right = parts[2];

                _network[start] = new Destinations(left, right);
            }

            var directionsLength = _directions.Length;

            while(_current != "ZZZ")
            {
                char direction = _directions[result % directionsLength];

                if(direction == 'L')
                {
                    _current = _network[_current].left;
                }
                else if(direction == 'R')
                {
                    _current = _network[_current].right;
                }
                else
                {
                    throw new Exception("Invalid direction");
                }

                result++;
            }

            Console.WriteLine($"Result: {result}");

            return result;
        }
    }
}
