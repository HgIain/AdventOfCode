// See https://aka.ms/new-console-template for more information
using Day1Point5;
using System.Text.RegularExpressions;

var text = await File.ReadAllLinesAsync("aocinput.txt");

int total = 0;

var regexString = @"(zero|one|two|three|four|five|six|seven|eight|nine|\d)";
var regex = new Regex(regexString);
var regexBack = new Regex(regexString, RegexOptions.RightToLeft );

foreach (var line in text)
{
    var regexResult = regex.Match(line);
    int value = 0;

    if (regexResult.Success)
    {
        // we have two groups, so we have two numbers
        value += Matcher.Match(regexResult.Groups[1].Value) * 10;
    }

    regexResult = regexBack.Match(line);

    if (regexResult.Success)
    {
        // we have two groups, so we have two numbers
        value += Matcher.Match(regexResult.Groups[1].Value);
    }


    Console.WriteLine($"Value: {value}");
    total += value;
}

Console.WriteLine($"Total: {total}");
