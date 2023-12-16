namespace Day16
{
    public class MirrorPathFollower
    {
        [Flags]
        private enum Direction
        {
            None = 0,
            Up = 0b1,
            Down = 0b10,
            Left = 0b100,
            Right = 0b1000
        }

        private readonly string[] lines;

        public MirrorPathFollower(string fileName)
        {
            lines = File.ReadAllLines(fileName);

            numCols = lines[0].Length;
            numRows = lines.Length;
        }

        private readonly int numCols;
        private readonly int numRows;

        public int GetEnergisedTileCount(bool getMaxEnergised = true)
        {
            if(!getMaxEnergised)
            {
                int total = GetEnergisedTileCountForDirection(0, 0, Direction.Right);

                Console.WriteLine($"Total: {total}");

                return total;
            }

            int max = 0;

            for(int x = 0; x < numCols; x++)
            {
                int result = GetEnergisedTileCountForDirection(x, 0, Direction.Down);

                if(result > max)
                {
                    max = result;
                }
                
                result = GetEnergisedTileCountForDirection(x, numRows -1, Direction.Up);

                if (result > max)
                {
                    max = result;
                }
            }

            for (int y = 0; y < numRows; y++)
            {
                int result = GetEnergisedTileCountForDirection(0, y, Direction.Right);

                if (result > max)
                {
                    max = result;
                }

                result = GetEnergisedTileCountForDirection(numCols - 1, y, Direction.Left);

                if (result > max)
                {
                    max = result;
                }
            }

            Console.WriteLine($"Max: {max}");

            return max;

        }

        private int GetEnergisedTileCountForDirection(int x, int y, Direction direction)
        {
            var entryDirections = new Direction[numCols, numRows];
            FollowPath(x, y, direction, entryDirections);

            int total = 0;

            foreach(var d in entryDirections)
            {
                if (d != Direction.None)
                {
                    total++;
                }
            }

            return total;
        }

        private void FollowPath(int startx, int starty, Direction direction, Direction[,] entryDirections)
        {
            if(startx < 0 || startx >= numCols || starty < 0 || starty >= numRows)
            {
                //check for out of bounds
                return;
            }

            if ((entryDirections[startx, starty] & direction) == direction )
            {
                // we've already been here, going this way
                return;
            }


            int xOffset = 0;
            int yOffset = 0;

            switch (direction)
            {
                case Direction.Up:
                    yOffset = -1;
                    break;
                case Direction.Down:
                    yOffset = 1;
                    break;
                case Direction.Left:
                    xOffset = -1;
                    break;
                case Direction.Right:
                    xOffset = 1;
                    break;
            }

            for (int x = startx, y = starty; x >= 0 && x < numCols && y >= 0 && y < numRows; x += xOffset, y += yOffset)
            {
                char c = lines[y][x];

                entryDirections[x, y] |= direction;

                if (c == '.')
                {
                    continue;
                }

                if (c == '|')
                {
                    if (direction == Direction.Up || direction == Direction.Down)
                    {
                        continue;
                    }
                    else
                    {
                        // split
                        FollowPath(x, y - 1, Direction.Up, entryDirections);
                        FollowPath(x, y + 1, Direction.Down, entryDirections);
                        return;
                    }
                }

                if(c == '-')
                {
                    if (direction == Direction.Left || direction == Direction.Right)
                    {
                        continue;
                    }
                    else
                    {
                        // split
                        FollowPath(x - 1, y, Direction.Left, entryDirections);
                        FollowPath(x + 1, y, Direction.Right, entryDirections);
                        return;
                    }
                }
                
                if(c == '/')
                {
                    if (direction == Direction.Up)
                    {
                        FollowPath(x + 1, y, Direction.Right, entryDirections);
                    }
                    else if (direction == Direction.Down)
                    {
                        FollowPath(x - 1, y, Direction.Left, entryDirections);
                    }
                    else if (direction == Direction.Left)
                    {
                        FollowPath(x, y + 1, Direction.Down, entryDirections);
                    }
                    else if (direction == Direction.Right)
                    {
                        FollowPath(x, y - 1, Direction.Up, entryDirections);
                    }
                    else
                    {
                        throw new Exception("Unknown direction");
                    }
                    return;
                }
                
                if(c == '\\')
                {
                    if (direction == Direction.Up)
                    {
                        FollowPath(x - 1, y, Direction.Left, entryDirections);
                    }
                    else if (direction == Direction.Down)
                    {
                        FollowPath(x + 1, y, Direction.Right, entryDirections);
                    }
                    else if (direction == Direction.Left)
                    {
                        FollowPath(x, y - 1, Direction.Up, entryDirections);
                    }
                    else if (direction == Direction.Right)
                    {
                        FollowPath(x, y + 1, Direction.Down, entryDirections);
                    }
                    else
                    {
                        throw new Exception("Unknown direction");
                    }

                    return;
                }

                throw new Exception("Unknown character in grid");
            }
            
        }
    }
}
