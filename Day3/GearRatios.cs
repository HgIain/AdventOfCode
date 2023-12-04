using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3
{
    public class GearRatios
    {
        private static Dictionary<(int x, int y), List<int>>  starParts = [];


        public static void AddToStars(int value, int row, int startColumn, int endColumn, int maxRows, int maxColumns, string[] text)
        {


            for (int i = row -1; i <= row + 1; i++)
            {
                if(i < 0 || i >= maxRows)
                {
                    continue;
                }

                for (int j = startColumn - 1; j <= endColumn + 1; j++)
                {
                    if (j < 0 || j >= maxColumns)
                    {
                        continue;
                    }

                    if(i == row && j >= startColumn && j <= endColumn)
                    {
                        continue;
                    }

                    if (starParts.TryGetValue((i, j), out var parts ))
                    {
                        parts.Add(value);
                    }
                    else if (text[i][j] == '*')
                    {
                        starParts.Add((i, j), [value]);
                    }
                }
                
            }
        }

        static public int Process(string filename)
        {
            starParts = [];
            var text = File.ReadAllLines(filename);

            var numLines = text.Length;
            var numColumns = text[0].Length;


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

                            AddToStars(number, i, startColumn, endColumn, numLines, numColumns,text);
                        }
                    }
                }

                if (lbProcessingNumber)
                {
                    lbProcessingNumber = false;
                    endColumn = numColumns - 1;

                    AddToStars(number, i, startColumn, endColumn, numLines, numColumns,text);

                }
            }

            foreach(var list in starParts.Values)
            {
                if(list.Count == 2)
                {
                    partTotal += list[0] * list[1];
                }
            }

            Console.WriteLine($"Total: {partTotal}");

            return partTotal;
        }
    }
}
