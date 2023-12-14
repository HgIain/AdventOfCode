using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Day14
{
    public class Tilter
    {
        private readonly string[] _lines;
        private readonly char[,] grid;
        private readonly int rowCount;
        private readonly int colCount;

        public Tilter(string filename)
        {
            _lines = System.IO.File.ReadAllLines(filename);
            rowCount = _lines.Length;
            colCount = _lines[0].Length;

            grid = new char[colCount, rowCount];
        }

        private enum Direction
        {
            North,
            South,
            East,
            West
        }

        private void TiltInDirection(Direction direction)
        {
            switch(direction)
            {
                case Direction.North:
                    for (int y = rowCount - 1; y >= 1; y--)
                    {
                        for (int x = 0; x < colCount; x++)
                        {
                            for (int currRow = y; currRow < rowCount; currRow++)
                            {
                                if (grid[x, currRow] == 'O')
                                {
                                    if (grid[x, currRow - 1] == '.')
                                    {
                                        // free space north of us, move up
                                        grid[x, currRow] = '.';
                                        grid[x, currRow - 1] = 'O';
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    break;

                    case Direction.South:
                    for (int y = 0; y < rowCount - 1; y++)
                    {
                        for (int x = 0; x < colCount; x++)
                        {
                            for (int currRow = y; currRow >=0; currRow--)
                            {
                                if (grid[x, currRow] == 'O')
                                {
                                    if (grid[x, currRow + 1] == '.')
                                    {
                                        // free space north of us, move up
                                        grid[x, currRow] = '.';
                                        grid[x, currRow + 1] = 'O';
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case Direction.West:
                    for (int x = colCount - 1; x >= 1; x--)
                    {
                        for (int y = 0; y < rowCount; y++)
                        {
                            for (int currCol = x; currCol < colCount; currCol++)
                            {
                                if (grid[currCol, y] == 'O')
                                {
                                    if (grid[currCol -1, y] == '.')
                                    {
                                        // free space north of us, move up
                                        grid[currCol, y] = '.';
                                        grid[currCol -1, y] = 'O';
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case Direction.East:
                    for (int x = 0; x < colCount - 1; x++)
                    {
                        for (int y = 0; y < rowCount; y++)
                        {
                            for (int currCol = x; currCol >= 0; currCol--)
                            {
                                if (grid[currCol, y] == 'O')
                                {
                                    if (grid[currCol + 1, y] == '.')
                                    {
                                        // free space north of us, move up
                                        grid[currCol, y] = '.';
                                        grid[currCol + 1, y] = 'O';
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    break;
            }
        }

#pragma warning disable IDE0051 // Remove unused private members
        private void PrintGrid()
#pragma warning restore IDE0051 // Remove unused private members
        {
            for(int y = 0; y < rowCount; y++)
            {
                for(int x = 0; x < colCount; x++)
                {
                    Console.Write(grid[x, y]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public long GetGridHash()
        {
            int p = 16777619;
            long hash = 2166136261L;

            for (int y = 0; y < rowCount; y++)
            {
                for (int x = 0; x < colCount; x++)
                {
                    hash = (hash ^ grid[x,y] ^ (x * y)) * p;
                }
            }
            hash += hash << 13;
            hash ^= hash >> 7;
            hash += hash << 3;
            hash ^= hash >> 17;
            hash += hash << 5;

            return hash;
        }

        public int CalculateStress()
        {
            int total = 0;
            int multiple = 1;
            for (int y = rowCount - 1; y >= 0; y--, multiple++)
            {
                for (int x = 0; x < colCount; x++)
                {
                    if (grid[x, y] == 'O')
                    {
                        total += multiple;
                    }
                }
            }
            return total;
        }

        public int GetTotalStress(bool bigLoop)
        {
            for (int y = 0; y < rowCount; y++)
            {
                var line = _lines[y];
                for (int x = 0; x < colCount; x++)
                {
                    grid[x, y] = line[x];
                }
            }

            if (bigLoop)
            {
                var hashedStates = new HashSet<long>();
                var ListOfHashes = new List<long>();
                var ListOfStresses = new List<int>();

                int totalCycles = 1000000000;

                for (int i = 0; i < totalCycles; i++)
                {
                    TiltInDirection(Direction.North);
                    TiltInDirection(Direction.West);
                    TiltInDirection(Direction.South);
                    TiltInDirection(Direction.East);

                    //PrintGrid();
                    var hash = GetGridHash();

                    //Console.WriteLine($"Stress at {i} is {CalculateStress()}");
                    ListOfHashes.Add(hash);
                    ListOfStresses.Add(CalculateStress());

                    if (!hashedStates.Add(hash))
                    {
                        Console.WriteLine($"Found duplicate state at {i}");
                        break;
                    }
                }

                int startOfLoop = 0;
                var clashedHash = ListOfHashes.Last();
                for (int i = 0; i < ListOfHashes.Count - 1; i++)
                {
                    if (ListOfHashes[i] == clashedHash)
                    {
                        startOfLoop = i;
                    }
                }

                int loopLength = ListOfHashes.Count - startOfLoop - 1;

                int loopRemainder = (totalCycles - startOfLoop - 1) % loopLength;



                int total = ListOfStresses[loopRemainder + startOfLoop];

                Console.WriteLine($"Total stress {total}");

                return total;
            }
            else
            {
                TiltInDirection(Direction.North);
                int total = CalculateStress();

                Console.WriteLine($"Total stress {total}");

                return total;

            }
        }
    }
}
