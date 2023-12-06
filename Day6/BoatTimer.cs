﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Day6
{
    public class BoatTimer(string filename)
    {
        private record BoatRecord(long time, long distance);

        private readonly List<BoatRecord> boatRecords = [];

        public long Process(bool singleTime = false)
        {
            var text = System.IO.File.ReadAllLines(filename);

            if(text.Length != 2)
            {
                throw new Exception("Invalid input");
            }

            if (singleTime)
            {
                var times = text[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
                var distances = text[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();

                var totalTime = long.Parse(String.Join("", times));
                var totalDistance = long.Parse(String.Join("", distances));

                boatRecords.Add(new(totalTime, totalDistance));
            }
            else
            {
                var times = text[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(c => long.Parse(c)).ToList();
                var distances = text[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(c => long.Parse(c)).ToList();

                if (times.Count != distances.Count)
                {
                    throw new Exception("Invalid input");
                }

                for (int i = 0; i < times.Count; i++)
                {
                    boatRecords.Add(new(times[i], distances[i]));
                }
            }

            long winningTimes = 1;

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            foreach(var boatRecord in boatRecords)
            {
                winningTimes *= CalculateWinningTimes(boatRecord);
            }
            stopwatch.Stop();
            Console.WriteLine($"Elapsed time {stopwatch.ElapsedMilliseconds}ms");

            Console.WriteLine($"Winning times {winningTimes}");

            return winningTimes;
        }

        private static long CalculateWinningTimes(BoatRecord boatRecord)
        {
            // solve the quadratic equation
            var minWinningTimeFloat = (boatRecord.time - Math.Sqrt(boatRecord.time * boatRecord.time - 4 * boatRecord.distance))/2;
            // exact match is a fail, so make sure to take the next highest integer
            long minWinningTime = (long)Math.Floor(minWinningTimeFloat + 1);

            // the curve will be symmetrical, so the max winning time is the time - min winning time
            var maxWinningTime = boatRecord.time - minWinningTime;
            long possibleWinningTimes = maxWinningTime - minWinningTime + 1;

            Console.WriteLine($"Winning possibilities {possibleWinningTimes}");


            return possibleWinningTimes;

        }

    }
}
