using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    public class PoolDigger
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
        private readonly Direction[,] dug;
        private readonly (int x, int y) start;

        private int currMinX = int.MaxValue;
        private int currMaxX = 0;
            
        private int currMinY = int.MaxValue;
        private int currMaxY = 0;

        public PoolDigger(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            foreach(var line in lines)
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

            Dictionary<Direction, int> bounds = [];

            foreach(var instruction in instructions)
            {
                if (!bounds.ContainsKey(instruction.direction))
                {
                    bounds[instruction.direction] = instruction.distance;
                }
                else
                {
                    bounds[instruction.direction] += instruction.distance;
                }
            }

            var minX = -bounds[Direction.Left];
            var maxX = bounds[Direction.Right];
            var minY = -bounds[Direction.Up];
            var maxY = bounds[Direction.Down];

            var xTotal = maxX - minX + 1;
            var yTotal = maxY - minY + 1;

            dug = new Direction[xTotal, yTotal];

            start = (-minX, -minY);
        }

        private int GetHoleSizeReally(bool print = false)
        {
            int holeSize = 0;
            for (int y = currMinY; y <= currMaxY; y++)
            {
                bool inHole = false;
                var currentDirection = Direction.None;
                for (int x = currMinX; x <= currMaxX; x++)
                {
                    if (dug[x, y] != Direction.None)
                    {
                        holeSize++;

                        if ((dug[x, y] & Direction.LeftRight) == Direction.None)
                        {
                            inHole = !inHole;
                        }
                        else if (currentDirection == Direction.None)
                        {
                            currentDirection = (dug[x, y] & Direction.UpDown);
                        }
                        else
                        {
                            var newDirection = dug[x, y] & Direction.UpDown;

                            if(newDirection != Direction.None)
                            {
                                if(newDirection == currentDirection)
                                {
                                    inHole = !inHole;
                                }
                                currentDirection = Direction.None;

                            }
                        }

                        //Console.Write('#');
                    }
                    else
                    {
                        if(currentDirection != Direction.None)
                        {
                            PrintGrid(y, y + 1);
                            throw new Exception("Hole not closed");
                        }

                        if (inHole)
                        {
                            holeSize++;
                            //Console.Write('#');
                        }
                        else
                        {
                            //Console.Write('.');
                        }
                    }
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
                for (int x = currMinX; x <= currMaxX; x++)
                {
                    if (dug[x,y] == Direction.Up || dug[x,y] == Direction.Down)
                    {
                        Console.Write('|');
                    }
                    else if (dug[x, y] == Direction.Left || dug[x, y] == Direction.Right)
                    {
                        Console.Write('-');
                    }
                    else if (dug[x,y] == Direction.UpLeft || dug[x,y] == Direction.UpRight)
                    {
                        Console.Write('*');
                    }
                    else if (dug[x, y] == Direction.DownLeft || dug[x, y] == Direction.DownRight)
                    {
                        Console.Write('^');
                    }
                    else if (dug[x, y] != Direction.None)
                    {
                        throw new Exception("Invalid direction");
                        //Console.Write('#');
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

        public int GetHoleSize()
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

                dug[x, y] |= instruction.direction;

                for (int i = 0; i < instruction.distance; i++)
                {
                    x += offset.x;
                    y += offset.y;

                    dug[x, y] |= instruction.direction;
                }

                if(x < currMinX)
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
            int holeSize = GetHoleSizeReally(true);
            //PrintGrid();

            Console.WriteLine($"Hole size: {holeSize}");

            return holeSize;

            //92153 is too low

        }
    }
}
