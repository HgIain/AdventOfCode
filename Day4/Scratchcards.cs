using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day4
{
    public class Scratchcards
    {
        public static int Process(string filename)
        {
            var text = File.ReadAllLines(filename);

            int total = 0;

            foreach(var line in text)
            {
                var result = ProcessLine(line);
                Console.WriteLine($"Game power is {result}");
                total += result;
            }

            Console.WriteLine($"Total: {total}");

            return total;
        }

        private static int ProcessLine(string line)
        {
            var splitString = line.Split([':','|']);

            if (splitString.Length != 3)
            {
                throw new Exception("Unexpected string format");
            }

            var winningNumbers = splitString[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c=>int.Parse(c));

            var myNumbers = splitString[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c=>int.Parse(c));

            int winnings = 0;

            foreach (var number in myNumbers)
            {
                if (winningNumbers.Contains(number))
                {
                    if(winnings == 0)
                    {
                        winnings = 1;
                    }
                    else
                    {
                        winnings *= 2;
                    }
                }
            }

            return winnings;

        }
    }
}
