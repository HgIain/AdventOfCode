﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    public class Galaxy
    {
        public static long Process(string filename,int expansionValue)
        {       
            var lines = File.ReadAllLines(filename);

            var nonEmptyRows = new bool[lines.Length];
            var nonEmptyColumns = new bool[lines[0].Length];
            List<(int x, int y)> galaxies = [];

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        nonEmptyRows[y] = true;
                        nonEmptyColumns[x] = true;
                        galaxies.Add((x, y));
                    }
                }
            }

            long total = 0;

            for(int i = 0; i< galaxies.Count; i++)
            {
                for(int j = i + 1; j < galaxies.Count; j++)
                {

                    int minX = Math.Min(galaxies[i].x, galaxies[j].x);
                    int maxX = Math.Max(galaxies[i].x, galaxies[j].x);
                    int minY = Math.Min(galaxies[i].y, galaxies[j].y);
                    int maxY = Math.Max(galaxies[i].y, galaxies[j].y);

                    total += (maxX - minX) + (maxY - minY);

                    for (int x = minX + 1; x < maxX; x++)
                    {
                        if (!nonEmptyColumns[x])
                        {
                            total+= expansionValue - 1;
                        }
                    }
                    for (int y = minY + 1; y < maxY; y++)
                    {
                        if (!nonEmptyRows[y])
                        {
                            total += expansionValue - 1;
                        }
                    }
                }
            }

            return total;
        }
    }
}
