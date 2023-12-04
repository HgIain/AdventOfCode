// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

var text = await File.ReadAllLinesAsync("day1input.txt");

int total = 0;

var regexString = @"(\d)";
var regex = new Regex(regexString);

foreach (var line in text)
{
    var regexResult = regex.Matches(line);
    int value = 0;

    if (regexResult.Count == 0)
    {
        throw new Exception("Regex failed");
    }

    value += int.Parse(regexResult[0].Value) * 10;
    value += int.Parse(regexResult[^1].Value);

    Console.WriteLine($"Value: {value}");
    total += value;
}

Console.WriteLine($"Total: {total}");
