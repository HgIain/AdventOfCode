using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day4
{
    public class ScratchcardsV2(string filename)
    {
        readonly List<int> cardCount = [];

        public int Process()
        {
            var text = File.ReadAllLines(filename);

            for (int i = 0; i<text.Length; i++)
            {
                cardCount.Add(1);
            }

            for(int i = 0; i<text.Length;i++)
            {
                var line = text[i];
                ProcessLine(i, line);
            }

            int total = cardCount.Sum();

            Console.WriteLine($"Total: {total}");

            return total;
        }

        public void ProcessLine(int index, string line)
        {
            var splitString = line.Split([':','|']);

            if (splitString.Length != 3)
            {
                throw new Exception("Unexpected string format");
            }

            var winningNumbers = splitString[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c=>int.Parse(c));

            var myNumbers = splitString[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c=>int.Parse(c));

            int currentCardCount = cardCount[index];
            index++;

            foreach (var number in myNumbers)
            {
                if(index >= cardCount.Count)
                {
                    break;
                }
                if (winningNumbers.Contains(number))
                {
                    cardCount[index++] += currentCardCount;
                }
            }
        }

    }
}
