using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day09
{
    public class Differentiator
    {
        public static long Processor(string filename, bool backwards = false)
        {
            var lines = File.ReadLines(filename);

            long total = 0;

            foreach (var line in lines)
            {
                List<long> numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(c=>long.Parse(c)).ToList();

                List<List<long>> allNumbers = [numbers];

                while(numbers.Any(c=>c!=0))
                {
                    List<long> nextNumbers = [];

                    for(int i = 0; i < numbers.Count-1; i++)
                    {
                        nextNumbers.Add(numbers[i + 1] - numbers[i]);
                    }

                    allNumbers.Add(nextNumbers);

                    numbers = nextNumbers;
                }

                allNumbers.Reverse();

                long tempTotal = 0;

                foreach(var number in allNumbers)
                {
                    if(backwards)
                    {
                        tempTotal = number.First() - tempTotal;
                    }
                    else
                    {
                        tempTotal = number.Last() + tempTotal;
                    }
                }

                Console.WriteLine($"Total: {tempTotal}");

                total += tempTotal;

            }

            Console.WriteLine($"Total: {total}");

            return total;
        }
    }
}
