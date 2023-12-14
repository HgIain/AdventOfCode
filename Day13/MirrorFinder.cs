using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    public class MirrorFinder
    {
        public static int CountBits(int value)
        {
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= value - 1;
            }
            return count;
        }

        public static int GetMirrorCount(string filename, bool allowSmudge)
        {
            var lines = System.IO.File.ReadAllLines(filename);

            int lastSplit = 0;
            int total = 0;

            for (int i = 0; i<lines.Length; i++)
            {
                var line = lines[i];
                
                if(line.Length == 0)
                {
                    // reached the end of a group
                    int numRows = i - lastSplit;
                    int numCols = lines[i-1].Length;

                    if(numRows > 32 || numCols > 32)
                    {
                        throw new Exception("Too many rows or columns");
                    } 

                    int[] rows = new int[numRows];
                    int[] cols = new int[numCols];

                    for(int x = 0; x < numCols; x++)
                    {
                        for(int y = 0; y < numRows; y++)
                        {
                            if (lines[lastSplit + y][x] == '#')
                            {
                                rows[y] |= (1 << x);
                                cols[x] |= (1 << y);
                            }
                        }
                    }

                    bool foundMirror = false;

                    for (int y = 0; y < numRows - 1; y++)
                    {
                        bool foundInconsistency = false;
                        bool foundSmudge = false;

                        for(int offset = 0; y - offset >= 0 && y + offset + 1 < numRows ;offset ++)
                        {
                            if (allowSmudge && !foundSmudge)
                            {
                                int diff = rows[y + offset + 1] ^ rows[y - offset];

                                if (CountBits(diff) == 1)
                                {
                                    foundSmudge = true;
                                }
                                else
                                {
                                    if (diff != 0)
                                    {
                                        foundInconsistency = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (rows[y + offset + 1] != rows[y - offset])
                                {
                                    foundInconsistency = true;
                                    break;
                                }
                            }
                        }

                        if (!foundInconsistency &&
                            (!allowSmudge || foundSmudge))
                        {
                            foundMirror = true;
                            total += (y + 1) * 100;
                            Console.WriteLine($"Found mirror at row {y + 1} for line {i}");
                            break;
                        }
                    }

                    if (!foundMirror)
                    {
                        for (int x = 0; x < numCols - 1; x++)
                        {
                            bool foundInconsistency = false;
                            bool foundSmudge = false;
                            for (int offset = 0; x - offset >= 0 && x + offset + 1 < numCols; offset++)
                            {
                                if (allowSmudge && !foundSmudge)
                                {
                                    int diff = cols[x + offset + 1] ^ cols[x - offset];

                                    if (CountBits(diff) == 1)
                                    {
                                        foundSmudge = true;
                                    }
                                    else
                                    {
                                        if (diff != 0)
                                        {
                                            foundInconsistency = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (cols[x + offset + 1] != cols[x - offset])
                                    {
                                        foundInconsistency = true;
                                        break;
                                    }
                                }
                            }

                            if (!foundInconsistency &&
                            (!allowSmudge || foundSmudge))
                            {
                                foundMirror = true;
                                total += x + 1;
                                Console.WriteLine($"Found mirror at column {x + 1} for line {i}");
                                break;
                            }
                        }
                    }

                    if(!foundMirror)
                    {
                        throw new Exception("No mirror found");
                    }

                    lastSplit = i + 1;

                }
            }

            return total;
        }
    }
}
