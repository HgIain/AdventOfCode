using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day18
{
    public partial class PoolDigger
    {
        [Flags]
        private enum Direction
        {
            None = 0,
            Up = 0b1,
            Down= 0b10,
            Left = 0b100,
            Right = 0b1000,
            Special = 0b10000,
            UpDown = Up | Down,
            LeftRight = Left | Right,
            UpLeft = Up | Left,
            UpRight = Up | Right,
            DownLeft = Down | Left,
            DownRight = Down | Right,
        }

        private record DigInstruction(Direction direction, int distance);

        private readonly List<DigInstruction> instructions = [];
        private readonly Dictionary<int, SortedDictionary<int , Direction>> dug = [];
        private readonly (int x, int y) start;

        private int currMinX = 0;
        private int currMaxX = 0;
            
        private int currMinY = 0;
        private int currMaxY = 0;

        [GeneratedRegex(@"(\d)")]
        private static partial Regex InstrcutionRegex();


        private void GetOriginalInstructions(string[] lines)
        {
            foreach (var line in lines)
            {
                var split = line.Split(' ');

                var direction = split[0].First() switch
                {
                    'U' => Direction.Up,
                    'D' => Direction.Down,
                    'L' => Direction.Left,
                    'R' => Direction.Right,
                    _ => throw new InvalidEnumArgumentException()
                };

                var distance = int.Parse(split[1]);

                instructions.Add(new DigInstruction(direction, distance));
            }
        }

        private void GetBigInstructions(string[] lines)
        {
            char[] splitChars = [' ', '(',')', '#'];
            foreach (var line in lines)
            {
                var split = line.Split(splitChars, StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries);

                if(split.Length != 3)
                {
                    throw new Exception("Invalid instruction");
                }

                var distAsHexString = split[2][..^1];
                var distance = Convert.ToInt32(distAsHexString, 16);
                var directions = split[2][^1] - '0';
                 
                var direction = split[2][^1] switch
                {
                    '0' => Direction.Right,
                    '1' => Direction.Down,
                    '2' => Direction.Left,
                    '3' => Direction.Up,
                    _ => throw new InvalidEnumArgumentException()
                };

                instructions.Add(new DigInstruction(direction, distance));
            }
        }

        public PoolDigger(string fileName, bool bigMode = true)
        {
            var lines = File.ReadAllLines(fileName);

            if(!bigMode)
            {
                GetOriginalInstructions(lines);
            }
            else
            {
                GetBigInstructions(lines);
            }

            start = (0,0);
        }

        private long GetHoleSizeReally(bool print = false)
        {
            long holeSize = 0;
            for (int y = currMinY; y <= currMaxY; y++)
            {
                var row = dug[y];
                bool inHole = false;
                var currentDirection = Direction.None;
                
                int prevX = currMinX;

                foreach(var (x, direction) in row)
                {
                    if (inHole && currentDirection == Direction.None)
                    {
                        holeSize += x - prevX - 1;
                    }

                    holeSize++;

                    if ((direction & Direction.LeftRight) == Direction.None)
                    {
                        inHole = !inHole;
                    }
                    else if (currentDirection == Direction.None)
                    {
                        currentDirection = (direction & Direction.UpDown);
                    }
                    else
                    {
                        var newDirection = direction & Direction.UpDown;

                        if (newDirection != Direction.None)
                        {
                            if (newDirection == currentDirection)
                            {
                                inHole = !inHole;
                            }
                            currentDirection = Direction.None;

                        }
                    }

                    prevX = x;
                }

                //Console.WriteLine();
            }

            return holeSize;
        }

        private void PrintGrid(int startY = int.MaxValue, int endY = int.MaxValue)
        {
            if(startY == int.MaxValue)
            {
                startY = currMinY;
            }
            if(endY == int.MaxValue)
            {
                endY = currMaxY;
            }

            for (int y = startY; y <= endY; y++)
            {
                var row = dug[y];

                for (int x = currMinX; x <= currMaxX; x++)
                {
                    if (row.TryGetValue(x, out var direction))
                    {
                        if (direction == Direction.Up || direction == Direction.Down)
                        {
                            Console.Write('|');
                        }
                        else if (direction == Direction.Left || direction == Direction.Right)
                        {
                            Console.Write('-');
                        }
                        else if (direction == Direction.UpLeft || direction == Direction.UpRight)
                        {
                            Console.Write('*');
                        }
                        else if (direction == Direction.DownLeft || direction == Direction.DownRight)
                        {
                            Console.Write('^');
                        }
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }

                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private void SetDug(int x, int y, Direction direction)
        {
            if(!dug.TryGetValue(y, out var row))
            {
                row = [];
                dug[y] = row;
            }

            if(row.TryGetValue(x, out var existing))
            {
                row[x] = existing | direction;
            }
            else
            {
                row[x] = direction;
            }
        }

        public long GetHoleSize()
        {
            (int x, int y) = start;

            foreach(var instruction in instructions)
            {
                (int x,int y) offset = instruction.direction switch
                {
                    Direction.Up => (0, -1),
                    Direction.Down => (0, 1),
                    Direction.Left => (-1, 0),
                    Direction.Right => (1, 0),
                    _ => throw new InvalidEnumArgumentException()
                };


                SetDug(x,y, instruction.direction);

                for (int i = 0; i < instruction.distance; i++)
                {
                    x += offset.x;
                    y += offset.y;

                    SetDug(x, y, instruction.direction);
                }

                if (x < currMinX)
                {
                    currMinX = x;
                }
                else if(x > currMaxX)
                {
                    currMaxX = x;
                }
                if(y < currMinY)
                {
                    currMinY = y;
                }
                else if(y > currMaxY)
                {
                    currMaxY = y;
                }
            }
            //PrintGrid();
            long holeSize = GetHoleSizeReally(true);
            //PrintGrid();

            Console.WriteLine($"Hole size: {holeSize}");

            return holeSize;

            //92153 is too low

        }
    }
}
