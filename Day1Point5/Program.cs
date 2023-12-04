// See https://aka.ms/new-console-template for more information
using Day1Point5;
using System.Text.RegularExpressions;

var text = await File.ReadAllLinesAsync("testinput.txt");

int total = 0;

var regexString = @"(zero|one|two|three|four|five|six|seven|eight|nine|\d)";
var regex = new Regex(regexString);

foreach (var line in text)
{
    var regexResult = regex.Matches(line);
    int value = 0;

    if(regexResult.Count == 0)
    {
        throw new Exception("Regex failed");
    }

    value += Matcher.Match(regexResult[0].Value) * 10;
    value += Matcher.Match(regexResult[^1].Value);

    Console.WriteLine($"Value: {value}");
    total += value;
}

Console.WriteLine($"Total: {total}");
