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

        private List<Position> _visited = [];

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

            while (_visited[^1] != startPosition)
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
                for (int i = 0; i < _pipeMap.GetLength(0); i++)
                {
                    if (IsInside(new(i,j)))
                    {
                        insideCount++;
                        Console.WriteLine($"Inside: {i},{j}");
                    }
                }
            }

            return insideCount;
        }

        bool IsInside(Position position)
        {
            if (position.X == 0 || position.Y == 0 || position.X == _pipeMap.GetLength(0) - 1 || position.Y == _pipeMap.GetLength(1) - 1)
            {
                // on the edge, can't be inside
                return false;
            }

            if (_pipeMap[position.X,position.Y].Visited)
            {
                // the position is on the path, not inside
                return false;
            }

            // project a ray from the position to any edge of the map
            // if the ray intersects an odd number of pipes, the position is inside
            // if the ray intersects an even number of pipes, the position is outside
            int intersectionCount = 0;
            bool currentlyIntersecting = false;
            bool enteredUp = false;

            for (int i = position.X - 1; i >= 0; i--)
            {
                if (!currentlyIntersecting)
                {
                    var testpos = position with { X = i };
                    if (_pipeMap[testpos.X, testpos.Y].Visited)
                    {
                        //intersectionCount++;
                        var pipeType = _pipeMap[i, position.Y].PipeType;

                        if (pipeType == PipeType.LeftRight)
                        {
                            throw new Exception("Unexpected pipe type");
                        }

                        if (pipeType == PipeType.UpLeft)
                        {
                            enteredUp = true;
                            currentlyIntersecting = true;
                        }
                        else if (pipeType == PipeType.DownLeft)
                        {
                            enteredUp = false;
                            currentlyIntersecting = true;
                        }
                        else
                        {
                            intersectionCount++;
                        }
                    }
                }
                else
                {
                    var pipeType = _pipeMap[i, position.Y].PipeType;
                    if(pipeType == PipeType.UpRight)
                    {
                        currentlyIntersecting = false;
                        if(!enteredUp)
                        {
                            intersectionCount++;
                        }
                    }
                    else if(pipeType == PipeType.DownRight)
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

            return intersectionCount % 2 == 1;
        }   

        (Position one, Position two) GetConnections(Position position)
        {
            var positionType = _pipeMap[position.X, position.Y].PipeType;

            switch(positionType)
            {
                case PipeType.Start:
                    throw new Exception("Unexpected pipe type");
                case PipeType.LeftRight:
                    return (new Position(position.X - 1, position.Y), new Position(position.X + 1, position.Y));
                case PipeType.UpDown:
                    return (new Position(position.X, position.Y - 1), new Position(position.X, position.Y + 1));
                case PipeType.UpLeft:
                    return (new Position(position.X - 1, position.Y), new Position(position.X, position.Y - 1));
                case PipeType.UpRight:
                    return (new Position(position.X + 1, position.Y), new Position(position.X, position.Y - 1));
                case PipeType.DownLeft:
                    return (new Position(position.X - 1, position.Y), new Position(position.X, position.Y + 1));
                case PipeType.DownRight:
                    return (new Position(position.X + 1, position.Y), new Position(position.X, position.Y + 1));
                case PipeType.None:
                    throw new Exception("Unexpected pipe type");
                default:
                    throw new Exception("Unexpected pipe type");
            }

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

            foreach(var position in positions)
            {
                if(CheckConnection(position, start))
                {
                    return position;
                }
            }

            throw new Exception("Failed to find connection");
        }

        bool CheckConnection(Position from, Position to)
        {
            if (from.X < 0 || from.Y < 0 || from.X >= _pipeMap.GetLength(0) || from.Y >= _pipeMap.GetLength(1))
            {
                return false;
            }

            var pipeType = _pipeMap[from.X, from.Y].PipeType;

            switch(pipeType)
            {
                case PipeType.Start:
                    throw new Exception("Unexpected pipe type");
                case PipeType.LeftRight:
                    return (to.X == from.X - 1 || to.X == from.X + 1);
                case PipeType.UpDown:
                    return (to.Y == from.Y - 1 || to.Y == from.Y + 1);
                case PipeType.UpLeft:
                    return (to.X == from.X - 1 || to.Y == from.Y - 1);
                case PipeType.UpRight:
                    return (to.X == from.X + 1 || to.Y == from.Y - 1);
                case PipeType.DownLeft:
                    return (to.X == from.X - 1 || to.Y == from.Y + 1);
                case PipeType.DownRight:
                    return (to.X == from.X + 1 || to.Y == from.Y + 1);
                case PipeType.None:
                    return false;
                default:
                    throw new Exception("Unexpected pipe type");
            }
        }
    }
}
