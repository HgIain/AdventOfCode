using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3
{
    public class PartFinder
    {
        public static bool IsValidPart(int row, int startColumn, int endColumn, bool[,] symbolLookup)
        {


            for (int i = row -1; i <= row + 1; i++)
            {
                if(i < 0 || i >= symbolLookup.GetLength(0))
                {
                    continue;
                }

                for (int j = startColumn - 1; j <= endColumn + 1; j++)
                {
                    if (j < 0 || j >= symbolLookup.GetLength(1))
                    {
                        continue;
                    }

                    if(i == row && j >= startColumn && j <= endColumn)
                    {
                        continue;
                    }

                    if (symbolLookup[i, j])
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }

        static public int Process(string filename)
        {
            var text = File.ReadAllLines(filename);

            var numLines = text.Length;
            var numColumns = text[0].Length;

            var symbolLookup = new bool[numLines, numColumns];

            string invalidSymbol = ".0123456789";

            for(int i = 0; i < numLines; i++)
            {
                for (int j = 0; j < numColumns; j++)
                {
                    // it's a valid symbol if it's not a digit or a dot
                    symbolLookup[i, j] = !invalidSymbol.Contains(text[i][j]);
                }
            }

            string digitString = "0123456789";

            int partTotal = 0;


            for (int i = 0; i < numLines; i++)
            {
                bool lbProcessingNumber = false;
                int number = 0;
                int startColumn = 0;
                int endColumn = 0;

                for(int j = 0; j < numColumns; j++)
                {
                    bool isDigit = digitString.Contains(text[i][j]);

                    if(isDigit)
                    {
                        if(!lbProcessingNumber)
                        {
                            lbProcessingNumber = true;
                            startColumn = j;
                            number = int.Parse(text[i][j].ToString());
                        }
                        else
                        {
                            number = number * 10 + int.Parse(text[i][j].ToString());
                        }
                    }
                    else
                    {
                        if(lbProcessingNumber)
                        {
                            lbProcessingNumber = false;
                            endColumn = j - 1;

                            if(IsValidPart(i,startColumn, endColumn, symbolLookup))
                            {
                                partTotal += number;
                            }
                            else
                            {
                                Console.WriteLine($"Part {number} line {i} is Invalid");
                            }
                        }
                    }
                }

                if (lbProcessingNumber)
                {
                    lbProcessingNumber = false;
                    endColumn = numColumns - 1;

                    if (IsValidPart(i, startColumn, endColumn, symbolLookup))
                    {
                        partTotal += number;
                    }
                    else
                    {
                        Console.WriteLine($"Part {number} line {i} is Invalid");
                    }
                }
            }

            Console.WriteLine($"Total: {partTotal}");

            return partTotal;
        }
    }
}
