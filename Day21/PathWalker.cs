using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    public class PathWalker
    {
        readonly bool[,] grid;
        (int x, int y) start = (0, 0);

        readonly HashSet<(int, int)> validEndPoints = [];
        readonly HashSet<(int x, int y)>[] newlyVisitedPositions;
        readonly HashSet<(int, int)> visitedPositions = [];


        public PathWalker(string filename)
        {
            var lines = System.IO.File.ReadAllLines(filename);
            grid = new bool[lines.Length, lines[0].Length];
            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    grid[x, y] = line[x] == '.';

                    if (line[x] == 'S')
                    {
                        start = (x, y);
                        grid[x, y] = true;
                    }
                }
            }

            newlyVisitedPositions = new HashSet<(int x, int y)>[2];
            newlyVisitedPositions[0] = [];
            newlyVisitedPositions[1] = [];
        }

        void VisitPosition(int x, int y, int stepNum, bool infiniteGrid)
        {
            int gridX = x;
            int gridY = y;

            if (!infiniteGrid)
            {
                if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1))
                {
                    return;
                }
            }
            else
            {
                gridX = x % grid.GetLength(0);
                gridY = y % grid.GetLength(1);

                while (gridX < 0)
                {
                    gridX += grid.GetLength(0);
                }
                while (gridY < 0)
                {
                    gridY += grid.GetLength(1);
                }
            }

            if (!grid[gridX, gridY])
            {
                return;
            }
            int evenOdd = stepNum & 1;

            bool previouslyVisited = visitedPositions.Contains((x, y));

            if (evenOdd == 0)
            {
                validEndPoints.Add((x, y));
            }

            if (!previouslyVisited)
            {
                visitedPositions.Add((x, y));
                newlyVisitedPositions[evenOdd].Add((x, y));
            }
        }

        public int GetPossibleEndings(int numSteps, bool infiniteGrid)
        {
            VisitPosition(start.x, start.y, 0, infiniteGrid);

            for(int stepNum = 1; stepNum <= numSteps; stepNum++)
            {
                int evenOdd = (stepNum + 1) & 1;

                foreach (var (x, y) in newlyVisitedPositions[evenOdd])
                {
                    VisitPosition(x - 1, y, stepNum, infiniteGrid);
                    VisitPosition(x + 1, y, stepNum, infiniteGrid);
                    VisitPosition(x, y - 1, stepNum, infiniteGrid);
                    VisitPosition(x, y + 1, stepNum, infiniteGrid);
                }

                newlyVisitedPositions[evenOdd].Clear();
            }

            int totalEndpoints = validEndPoints.Count;
            Console.WriteLine($"Total endpoints: {totalEndpoints}");

            return totalEndpoints;
        }
    }
}
