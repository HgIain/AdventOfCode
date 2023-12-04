// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

var text = await File.ReadAllLinesAsync("testinput.txt");

int total = 0;

var regexString = @"(\d)";
var regex1 = new Regex(regexString);
var regex2 = new Regex(regexString, RegexOptions.RightToLeft);

foreach (var line in text)
{
    int value = 0;

    var regexResult = regex1.Match(line);
    if (!regexResult.Success)
    {
        throw new Exception("Regex failed");
    }
    value += int.Parse(regexResult.Groups[1].Value) * 10;

    regexResult = regex2.Match(line);
    if (!regexResult.Success)
    {
        throw new Exception("Regex failed");
    }
    value += int.Parse(regexResult.Groups[1].Value);

    Console.WriteLine($"Value: {value}");
    total += value;
}

Console.WriteLine($"Total: {total}");
