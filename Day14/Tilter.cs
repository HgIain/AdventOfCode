using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    public class Tilter
    {
        private readonly string[] _lines;

        public Tilter(string filename)
        {
            _lines = System.IO.File.ReadAllLines(filename);
        }

        public int GetTotalStress()
        {
            int total = 0;

            int rowCount = _lines.Length;
            int colCount = _lines[0].Length;

            char[,] grid = new char[colCount, rowCount];

            for (int y = 0; y < rowCount; y++)
            {
                var line = _lines[y];
                for (int x = 0; x < colCount; x++)
                {
                    grid[x, y] = line[x];
                }
            }

            for(int y = rowCount - 1; y >= 1; y--)
            {
                for(int x = 0; x < colCount; x++)
                {
                    for(int currRow = y; currRow < rowCount; currRow++)
                    {
                        if (grid[x,currRow] == 'O')
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

            Console.WriteLine($"Total stress {total}");

            return total;
        }
    }
}
