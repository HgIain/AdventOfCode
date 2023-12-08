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
        private string _directions = "";

        public int Process()
        {
            string _current = "AAA";
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

        private bool IsOnFinishPosition(string position)
        {
            return position.EndsWith('Z');
        }

        public ulong ProcessMultiple()
        {
            List<ulong> results = [];

            var lines = File.ReadLines(filename);

            _directions = lines.First();

            lines = lines.Skip(2);

            char[] separators = [' ', '=', '(', ')', ','];

            var positions = new List<string>();

            foreach (var line in lines)
            {
                var parts = line.Split(separators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (parts.Length != 3)
                {
                    throw new Exception("Invalid line");
                }
                var start = parts[0];
                var left = parts[1];
                var right = parts[2];

                if(start.Length != 3)
                {
                    throw new Exception("Invalid start");
                }
                if (left.Length != 3)
                {
                    throw new Exception("Invalid left");
                }
                if (right.Length != 3)
                {
                    throw new Exception("Invalid right");
                }

                _network[start] = new Destinations(left, right);

                if(start.EndsWith('A'))
                {
                    positions.Add(start);
                }
            }

            ulong directionsLength = (ulong)_directions.Length;

            for(int i = 0;i<positions.Count;i++)
            {
                var position = positions[i];
                ulong result = 0;

                while (!IsOnFinishPosition(position))
                {
                    int directionIndex = (int)(result % directionsLength);
                    char direction = _directions[directionIndex];

                    if (direction == 'L')
                    {
                        position = _network[position].left;
                    }
                    else if (direction == 'R')
                    {
                        position = _network[position].right;
                    }
                    else
                    {
                        throw new Exception("Invalid direction");
                    }

                    result++;
                }

                results.Add(result);
            }

            ulong lcmResult = results.Aggregate(LowestCommonMultiple);

            Console.WriteLine($"Result: {lcmResult}");

            return lcmResult;
        }

        public static ulong GreatestCommonDivisor(ulong a, ulong b)
        {
            while (b != 0)
            {
                ulong temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static ulong LowestCommonMultiple(ulong a, ulong b)
        {
            return (a / GreatestCommonDivisor(a, b)) * b;
        }

    }


}
