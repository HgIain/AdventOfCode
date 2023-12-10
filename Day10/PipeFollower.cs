using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10
{
    public class PipeFollower(string filename)
    {
        enum PipeType
        {
            Start,
            LeftRight,
            UpDown,
            UpLeft,
            UpRight,
            DownLeft,
            DownRight,
            None
        }

        static private PipeType GetPipeTypeForLetter(char letter) => letter switch
        {
            'S' => PipeType.Start,
            '-' => PipeType.LeftRight,
            '|' => PipeType.UpDown,
            'J' => PipeType.UpLeft,
            'L' => PipeType.UpRight,
            '7' => PipeType.DownLeft,
            'F' => PipeType.DownRight,
            '.' => PipeType.None,
            _ => throw new Exception("Unexpected letter")
        };

        private record Position(int X, int Y);

        private record Pipe(PipeType PipeType, bool Visited);

        [AllowNull]
        private Pipe[,] _pipeMap;

        private readonly List<Position> _visited = [];

        private void AddVisited(Position position)
        {
            if (_visited.Contains(position))
            {
                throw new Exception("Already visited");
            }

            _visited.Add(position);

            _pipeMap[position.X, position.Y] = _pipeMap[position.X, position.Y] with { Visited = true };
        }

        public int Process(bool getInsideCount = false)
        {
            var lines = System.IO.File.ReadAllLines(filename);
            _pipeMap = new Pipe[lines[0].Length, lines.Length];

            Position startPosition = new (-1, -1);

            for (int j = 0; j < lines.Length; j++)
            {
                var chars = lines[j].ToCharArray();
                for(int i = 0; i < chars.Length; i++)
                {
                    PipeType pipeType = GetPipeTypeForLetter(chars[i]);
                    _pipeMap[i, j] = new(pipeType, false);

                    if (pipeType == PipeType.Start)
                    {
                        startPosition = new Position(i, j);
                    }
                }
            }

            if (startPosition.X < 0 || startPosition.Y < 0)
            {
                throw new Exception("Failed to find start");
            }

            var position = GetFirstConnection(startPosition);

            AddVisited(startPosition);
            AddVisited(position);

            while (true)
            {
                var nextPosition = GetNextConnection(_visited[^2], _visited[^1]);

                if (nextPosition == startPosition)
                {
                    break;
                }

                AddVisited(nextPosition);
            }

            if (!getInsideCount)
            {
                return _visited.Count / 2;
            }

            var insideCount = 0;

            for(int j = 0; j < _pipeMap.GetLength(1); j++)
            {
                insideCount += GetLineInsideCount(j);
            }

            return insideCount;
        }

        int GetLineInsideCount(int y)
        {
            if (y == 0 || y == _pipeMap.GetLength(1) - 1)
            {
                // on the edge, can't be inside
                return 0;
            }

            // project a ray from the position to any edge of the map
            // if the ray intersects an odd number of pipes, the position is inside
            // if the ray intersects an even number of pipes, the position is outside
            int intersectionCount = 0;
            bool currentlyIntersecting = false;
            bool enteredUp = false;
            int insideCount = 0;

            for (int i = 0; i <_pipeMap.GetLength(0); i++)
            {
                if (!currentlyIntersecting)
                {
                    if (_pipeMap[i, y].Visited)
                    {
                        //intersectionCount++;
                        var pipeType = _pipeMap[i, y].PipeType;

                        if (pipeType == PipeType.LeftRight)
                        {
                            throw new Exception("Unexpected pipe type");
                        }

                        if (pipeType == PipeType.UpRight)
                        {
                            enteredUp = true;
                            currentlyIntersecting = true;
                        }
                        else if (pipeType == PipeType.DownRight)
                        {
                            enteredUp = false;
                            currentlyIntersecting = true;
                        }
                        else if(pipeType == PipeType.UpDown)
                        {
                            intersectionCount++;
                        }
                        else
                        {
                            throw new Exception("Unexpected pipe type");
                        }
                    }
                    else if(intersectionCount %2 == 1)
                    {
                        insideCount++;
                    }
                }
                else
                {
                    var pipeType = _pipeMap[i, y].PipeType;
                    if(pipeType == PipeType.UpLeft)
                    {
                        currentlyIntersecting = false;
                        if(!enteredUp)
                        {
                            intersectionCount++;
                        }
                    }
                    else if(pipeType == PipeType.DownLeft)
                    {
                        currentlyIntersecting = false;
                        if(enteredUp)
                        {
                            intersectionCount++;
                        }
                    }
                    else if(pipeType != PipeType.LeftRight)
                    {
                        throw new Exception("Unexpected pipe type");
                    }
                }
            }

            return insideCount;
        }   

        (Position one, Position two) GetConnections(Position position)
        {
            var positionType = _pipeMap[position.X, position.Y].PipeType;

            return positionType switch
            {
                PipeType.Start => throw new Exception("Unexpected pipe type"),
                PipeType.LeftRight => (new Position(position.X - 1, position.Y), new Position(position.X + 1, position.Y)),
                PipeType.UpDown => (new Position(position.X, position.Y - 1), new Position(position.X, position.Y + 1)),
                PipeType.UpLeft => (new Position(position.X - 1, position.Y), new Position(position.X, position.Y - 1)),
                PipeType.UpRight => (new Position(position.X + 1, position.Y), new Position(position.X, position.Y - 1)),
                PipeType.DownLeft => (new Position(position.X - 1, position.Y), new Position(position.X, position.Y + 1)),
                PipeType.DownRight => (new Position(position.X + 1, position.Y), new Position(position.X, position.Y + 1)),
                PipeType.None => throw new Exception("Unexpected pipe type"),
                _ => throw new Exception("Unexpected pipe type"),
            };
            throw new Exception("Failed to find connection");
        }

        Position GetNextConnection(Position prev, Position current)
        {
            (Position one, Position two) = GetConnections(current);

            if(prev == one)
            {
                return two;
            }
            else if(prev == two)
            {
                return one;
            }

            throw new Exception("Failed to find connection");
        }

        Position GetFirstConnection(Position start)
        {
            Position[] positions = [new(start.X, start.Y - 1), new(start.X, start.Y + 1), new(start.X - 1, start.Y), new(start.X + 1, start.Y)];

            List<Position> connections = [];

            foreach(var position in positions)
            {
                if(CheckConnection(position, start))
                {
                    connections.Add(position);
                }
            }

            if (connections.Count != 2)
            {
                throw new Exception("Invalid pipes");
            }

            PipeType pipeType = PipeType.None;

            if (connections[0].X == connections[1].X)
            {
                pipeType = PipeType.UpDown;
            }
            else if (connections[0].Y == connections[1].Y)
            {
                pipeType = PipeType.LeftRight;
            }
            else if (connections[0].X < start.X || connections[1].X < start.X)
            {
                if (connections[0].Y < start.Y || connections[1].Y < start.Y)
                {
                    pipeType = PipeType.UpLeft;
                }
                else
                {
                    pipeType = PipeType.DownLeft;
                }
            }
            else
            {
                if (connections[0].Y < start.Y || connections[1].Y < start.Y)
                {
                    pipeType = PipeType.UpRight;
                }
                else
                {
                    pipeType = PipeType.DownRight;
                }
            }


            if(pipeType == PipeType.None)
            {
                throw new Exception("Invalid pipes");
            }

            _pipeMap[start.X, start.Y] = _pipeMap[start.X, start.Y] with { PipeType = pipeType };

            return connections[0];
        }

        bool CheckConnection(Position from, Position to)
        {
            if (from.X < 0 || from.Y < 0 || from.X >= _pipeMap.GetLength(0) || from.Y >= _pipeMap.GetLength(1))
            {
                return false;
            }

            var pipeType = _pipeMap[from.X, from.Y].PipeType;

            return pipeType switch
            {
                PipeType.Start => throw new Exception("Unexpected pipe type"),
                PipeType.LeftRight => (to.X == from.X - 1 || to.X == from.X + 1),
                PipeType.UpDown => (to.Y == from.Y - 1 || to.Y == from.Y + 1),
                PipeType.UpLeft => (to.X == from.X - 1 || to.Y == from.Y - 1),
                PipeType.UpRight => (to.X == from.X + 1 || to.Y == from.Y - 1),
                PipeType.DownLeft => (to.X == from.X - 1 || to.Y == from.Y + 1),
                PipeType.DownRight => (to.X == from.X + 1 || to.Y == from.Y + 1),
                PipeType.None => false,
                _ => throw new Exception("Unexpected pipe type"),
            };
        }
    }
}
