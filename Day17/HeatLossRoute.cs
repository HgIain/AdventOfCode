using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    public class HeatLossRoute
    {
        private readonly string[] lines;
        private readonly int[,] heatLoss;
        private readonly int numCols;
        private readonly int numRows;

        private readonly int cap = 1500;

        public HeatLossRoute(string fileName, int minMovement = 0, int maxMovement = 3)
        {
            lines = File.ReadAllLines(fileName);

            numCols = lines[0].Length;
            numRows = lines.Length;

            heatLoss = new int[numCols, numRows];
            nodes = new List<Node>[numCols, numRows];

            for (int y = 0; y < numRows; y++)
            {
                for(int x = 0; x < numCols; x++)
                {
                    heatLoss[x, y] = lines[y][x] - '0';
                    nodes[x, y] = [];
                }
            }

            heatLoss[0, 0] = 0;
            minMoveCount = minMovement;
            maxMoveCount = maxMovement;
        }

        enum Direction
        {
            None,
            Up,
            Down,
            Left,
            Right
        }

        private class Node
        {
            public required Direction entryDirection;
            public required int directionCount;
            public required int currentTotalHeatLost;
        }

        private List<Node>[,] nodes;

        private int currMin = int.MaxValue;

        private readonly int minMoveCount;
        private readonly int maxMoveCount;

        private int GetBestDistanceFromNode(int x, int y, Direction direction, int moveCount, int currentTotal, out bool printPath)
        {
            printPath = false;
            if(x < 0 || x >= numCols || y < 0 || y >= numRows)
            {
                return int.MaxValue;
            }

            if(moveCount >= maxMoveCount)
            {
                return int.MaxValue;
            }

            int newTotal = currentTotal + heatLoss[x, y];

            if (newTotal > cap || newTotal > currMin)
            {
                return int.MaxValue;
            }


            if (x == numCols - 1 && y == numRows - 1)
            {
                if (newTotal == 47)
                {
                    Console.WriteLine($"{direction} {moveCount} {newTotal}");
                    printPath = true;
                }

                if(newTotal < currMin)
                {
                    currMin = newTotal;
                    Console.WriteLine($"new min {currMin}");
                }

                return newTotal;
            }

            var existingNode = nodes[x, y].FirstOrDefault(n => n.entryDirection == direction && n.directionCount == moveCount);

            if(existingNode != null)
            {
                if(existingNode.currentTotalHeatLost <= newTotal)
                {
                    // already been here with a better result
                    return int.MaxValue;
                }
                    
                existingNode.currentTotalHeatLost = newTotal;
            }
            else
            {
                nodes[x, y].Add(new Node { entryDirection = direction, directionCount = moveCount, currentTotalHeatLost = newTotal });
            }

            List<int> results = [];

            bool printPath1 = false;
            bool printPath2 = false;
            bool printPath3 = false;
            bool printPath4 = false;


            if (direction != Direction.Up && (direction == Direction.Down || moveCount >= minMoveCount - 1 || direction == Direction.None))
            {
                results.Add(GetBestDistanceFromNode(x, y + 1, Direction.Down, direction == Direction.Down ? moveCount + 1 : 0, newTotal, out printPath2));
            }
            if (direction != Direction.Left && (direction == Direction.Right || moveCount >= minMoveCount - 1 || direction == Direction.None))
            {
                results.Add(GetBestDistanceFromNode(x + 1, y, Direction.Right, direction == Direction.Right ? moveCount + 1 : 0, newTotal, out printPath3));
            }
            if (direction != Direction.Right && (direction == Direction.Left || moveCount >= minMoveCount - 1 || direction == Direction.None))
            {
                results.Add(GetBestDistanceFromNode(x - 1, y, Direction.Left, direction == Direction.Left ? moveCount + 1 : 0, newTotal, out printPath4));
            }
            if (direction != Direction.Down && (direction == Direction.Up || moveCount >= minMoveCount - 1 || direction == Direction.None))
            {
                results.Add(GetBestDistanceFromNode(x, y - 1, Direction.Up, direction == Direction.Up ? moveCount + 1 : 0, newTotal, out printPath1));
            }

            var minResult = results.Min();

            if(minResult == int.MaxValue)
            {
                return int.MaxValue;
            }

            printPath = printPath1 || printPath2 || printPath3 || printPath4;

            if(printPath)
            {
                Console.WriteLine($"{direction} {moveCount} {newTotal}");
            }

            return minResult;
        }


        public int GetShortestRoute()
        {
            //nodes[0,0] = new List<Node> { new Node { entryDirection = Direction.None, directionCount = 0, currentTotalHeatLost = 0 } };

            int total = GetBestDistanceFromNode(0,0, Direction.None, 0, 0, out var printPath);

            Console.WriteLine($"Total: {total}");

            return total;
        }
    }
}
